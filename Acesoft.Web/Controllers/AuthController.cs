using System;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Acesoft.Rbac;
using Acesoft.Util;
using Acesoft.Rbac.Entity;
using Acesoft.Web.Mvc;

namespace Acesoft.Web.Controllers
{
    [ApiExplorerSettings(GroupName = "RBAC")]
    [Route("api/[controller]/[action]")]
    public class AuthController : ApiControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IUserService userService;
        private readonly IScaleService scaleService;
        private readonly IUAService uAService;
        private readonly IRoleService roleService;
        private readonly IPAService pAService;
        private readonly IObjectService objectService;

        public AuthController(ILogger<AuthController> logger, 
            IUserService userService,
            IScaleService scaleService,
            IUAService uAService,
            IRoleService roleService,
            IPAService pAService,
            IObjectService objectService)
        {
            this.logger = logger;
            this.userService = userService;
            this.scaleService = scaleService;
            this.uAService = uAService;
            this.roleService = roleService;
            this.pAService = pAService;
            this.objectService = objectService;
        }

        #region login
        [HttpPost, Action("用户登录")]
        public async Task<IActionResult> Login([FromBody] JObject data)
        {
            var userName = data.GetValue<string>("username");
            var password = data.GetValue<string>("password");
            var persistent = data.GetValue("persistent", true);
            Check.Require(userName.HasValue() && password.HasValue(), "用户名或密码不能为空");

            await AppCtx.AC.Login(userName, password, persistent);
            return Ok(null);
        }

        [HttpGet, MultiAuthorize, Action("获取用户信息")]
        public IActionResult GetUser()
        {
            var user = AppCtx.AC.User;
            logger.LogDebug($"Get logined user with UserId:{user.Id}, NickName:{user.NickName}");

            return Ok(new
            {
                id = user.Id,
                loginname = user.LoginName,
                nickname = user.NickName,
                mobile = user.Mobile,
                mail = user.Mail,
                photo = App.GetWebPhoto(user.Photo, true, Constants.Url_User)
            });
        }
        #endregion

        #region mobile
        [HttpPost, MultiAuthorize, Action("修改手机")]
        public IActionResult PostMobile([FromBody] JObject data)
        {
            Check.Require(AppCtx.AC.Logined, "未登录不能重新绑定手机");

            AppCtx.AC.User.Mobile = ValidMobile(data);
            userService.Update(AppCtx.AC.User);

            return Ok(null);
        }

        private string ValidMobile(JObject data, bool valid = false)
        {
            var mobile = data.GetValue<string>("mobile");
            if (valid && mobile.HasValue() && userService.GetByMobile(mobile) != null)
            {
                throw new AceException("该手机号码已注册或绑定用户");
            }

            var validCode = data.GetValue("valid_code", "");
            if (validCode.HasValue())
            {
                string cacheCode = App.Cache.GetString("sms_" + mobile);
                if (cacheCode == null && validCode == "acecom")
                {
                    return mobile;
                }
                if (cacheCode == null || cacheCode != validCode)
                {
                    throw new AceException("短信验证码输入错误");
                }
            }
            return mobile;
        }
        #endregion

        #region mail
        [HttpPost, MultiAuthorize, Action("修改邮箱")]
        public IActionResult PostMail([FromBody] JObject data)
        {
            Check.Require(AppCtx.AC.Logined, "未登录不能修改绑定邮箱");

            AppCtx.AC.User.Mail = ValidMail(data);
            userService.Update(AppCtx.AC.User);

            return Ok(null);
        }

        private string ValidMail(JObject data, bool valid = false)
        {
            var mail = data.GetValue<string>("mail");
            if (userService.GetByMail(mail) != null)
            {
                throw new AceException("该邮箱已注册或已绑定其他用户");
            }

            var validCode = data.GetValue<string>("valid_code");
            if (validCode.HasValue())
            {
                var cacheCode = App.Cache.GetString("mail_" + mail);
                if (cacheCode == null && validCode == "acecom")
                {
                    return mail;
                }
                if (cacheCode == null || cacheCode != validCode)
                {
                    throw new AceException("邮箱验证码输入错误");
                }
            }
            return mail;
        }
        #endregion

        #region pwd
        [HttpPost, Action("找回密码")]
        public IActionResult PostPwd([FromBody] JObject data)
        {
            var mobile = ValidMobile(data);
            var user = userService.GetByMobile(mobile);
            Check.Require(user != null, "手机号未注册或未绑定用户");

            var pwd = data.GetValue("pwd", data.GetValue("password", ""));
            user.Password = CryptoHelper.ComputeMD5(user.HashId, pwd);
            user.RstPwd = false;
            user.DRstPwd = DateTime.Now;
            userService.Update(user);

            SqlMapper.CacheManager.Flush("rbac.user");
            return Ok(null);
        }

        [HttpPut, MultiAuthorize, Action("修改密码")]
        public IActionResult PutPwd([FromBody] JObject data)
        {
            Check.Require(AppCtx.AC.Logined, "未登录不能修改密码");

            var pwd = data.GetValue("pwd", data.GetValue("password", ""));
            var newPwd = data["newpwd"].Value<string>();
            Check.Assert(pwd == newPwd, "新密码不能与当前密码相同");

            var user = AppCtx.AC.User;
            var password = CryptoHelper.ComputeMD5(user.HashId, pwd);
            Check.Require(user.Password == password, "原密码输入不正确");

            user.Password = CryptoHelper.ComputeMD5(user.HashId, newPwd);
            user.RstPwd = false;
            user.DRstPwd = DateTime.Now;
            userService.Update(user);

            SqlMapper.CacheManager.Flush("rbac.user");
            return Ok(null);
        }

        [HttpPut, MultiAuthorize, Action("重置密码")]
        public IActionResult ResetPwd(long userId)
        {
            var user = userService.Get(userId);

            user.Password = CryptoHelper.ComputeMD5(user.HashId, user.LoginName ?? "123456");
            user.RstPwd = true;
            user.DRstPwd = DateTime.Now;
            userService.Update(user);

            SqlMapper.CacheManager.Flush("rbac.user");
            return Ok(null);
        }
        #endregion

        #region user
        [HttpPost, DataSource, Action("注册用户")]
        public IActionResult PostUser([FromBody] JObject data)
        {
            CheckDataSourceParameter();

            var valid = SqlMap.Params.GetValue("validmobile", false);
            var mobile = ValidMobile(data, valid);
            var loginName = data.GetValue<string>("loginname");
            if (loginName.HasValue() && userService.GetByLoginName(loginName) != null)
            {
                throw new AceException("用户名 [" + loginName + "] 已经使用");
            }            

            var user = new Rbac_User();
            user.InitializeId();
            user.DCreate = DateTime.Now;
            user.Enabled = data.GetValue("enable", 1) == 1;
            user.UserType = (UserType)data.GetValue("usertype", 1);
            user.LoginName = loginName;
            user.UserName = data.GetValue<string>("username");
            user.NickName = data.GetValue("nickname", mobile);
            user.Password = CryptoHelper.ComputeMD5(user.HashId, data["password"].Value<string>());
            user.Photo = data.GetValue<string>("photo");
            user.Mobile = mobile;
            user.Mail = data.GetValue<string>("mail");
            user.Remark = data.GetValue<string>("remark");
            user.Creator = data.GetValue("creator", SqlMap.Params.GetValue("creator", AppCtx.AC.User.Id.ToString()));
            //user.System = data.GetValue("system", 0) == 1;
            user.RegType = (RegType)data.GetValue("regtype", 9);
            user.Client_Id = data.GetValue<long?>("clientid");

            AppCtx.Session.BeginTransaction();
            try
            {
                var refcode = data.GetValue("refcode", "");
                var refname = data.GetValue("refname", "");
                var checkInsertSql = SqlMap.Params.GetValue("checkinsertsql", "");
                var checkInsertError = SqlMap.Params.GetValue("checkinserterror", "");
                if (checkInsertSql.HasValue())
                {
                    var result = AppCtx.Session.QueryFirst(checkInsertSql, new
                    {
                        refid = data.GetValue("refid", App.IdWorker.NextId()),
                        refcode,
                        refname
                    });
                    if (result.cnt > 0)
                    {
                        throw new AceException(checkInsertError.FormatWith(refcode, refname));
                    }
                    refcode = result.refcode.ToString();
                }
                else if (checkInsertError.HasValue() && userService.GetByRefCode(refcode) != null)
                {
                    throw new AceException(checkInsertError.FormatWith(refcode, refname));
                }
                user.RefCode = refcode;

                if (App.GetQuery("scale", 0) == 1)
                {
                    var refId = data.GetValue<string>("ref_id");
                    var scale = scaleService.GetByRef(refId);
                    if (scale == null)
                    {
                        scale = new Rbac_Scale();
                        scale.InitializeId();
                        scale.DCreate = DateTime.Now;
                        scale.Name = data.GetValue("scale_name", user.UserName);
                        if (refId.HasValue()) scale.Ref_Id = refId;
                        scale.ParentId = AppCtx.AC.User.Scale_Id;
                        scale.Remark = "由用户 [" + user.LoginName + "] 自动生成";
                        scaleService.Insert(scale);
                    }
                    user.Scale_Id = scale.Id;
                    user.UserType = UserType.Admin;
                }
                else
                {
                    user.Scale_Id = data.GetValue("scale_id", AppCtx.AC.GetDefaultScaleId());
                }
                userService.Insert(user);

                var roles = SqlMap.Params.GetValue("roles", "");
                if (roles.HasValue())
                {
                    roles.Split<long>().Each(roleId =>
                    {
                        var ua = new Rbac_UA();
                        ua.InitializeId();
                        ua.User_Id = user.Id;
                        ua.Role_Id = roleId;
                        ua.DCreate = DateTime.Now;
                        AppCtx.Session.Insert(ua);
                    });
                }

                AppCtx.Session.Commit();

                if (SqlMap.SqlId != "rbac.user")
                {
                    SqlMapper.CacheManager.Flush(SqlMap.SqlId);
                }
                SqlMapper.CacheManager.Flush("rbac.user");
            }
            catch (Exception ex)
            {
                AppCtx.Session.Rollback();
                throw new AceException(ex.GetMessage());
            }

            return Ok(null);
        }

        [HttpPut, MultiAuthorize, Action("修改用户")]
        public IActionResult PutUser([FromBody] JObject data)
        {
            Rbac_User user = null;
            var userId = data.GetValue<long>("id", 0);
            if (userId > 0)
            {
                user = userService.Get(userId);
            }
            else if (AppCtx.AC.Logined)
            {
                user = AppCtx.AC.User;
            }
            else
            {
                throw new AceException("未登录不能修改用户信息");
            }

            var loginName = data.GetValue<string>("loginname");
            if (loginName.HasValue())
            {
                var searchUser = userService.GetByLoginName(loginName);
                if (searchUser != null && searchUser.Id != user.Id)
                {
                    throw new AceException("登录名 [" + loginName + "] 已经使用");
                }
                user.LoginName = loginName;
            }

            var mobile = data.GetValue<string>("mobile");
            if (mobile.HasValue())
            {
                var searchUser = userService.GetByMobile(mobile);
                if (searchUser != null && searchUser.Id != user.Id)
                {
                    throw new AceException("该手机号已注册或绑定其他用户");
                }
                user.Mobile = mobile;
            }

            var mail = data.GetValue<string>("mail");
            if (mail.HasValue())
            {
                var searchUser = userService.GetByMail(mail);
                if (searchUser != null && searchUser.Id != user.Id)
                {
                    throw new AceException("该邮箱已注册或绑定其他用户");
                }
                user.Mail = mail;
            }

            var password = data.GetValue<string>("password");
            if (password.HasValue() && !password.EndsWith("==") && password.Length <= 20)
            {
                user.Password = CryptoHelper.ComputeMD5(user.HashId, password);
            }
            if (data["username"] != null)
            {
                user.UserName = data.GetValue<string>("username");
            }
            if (data["nickname"] != null)
            {
                user.NickName = data.GetValue<string>("nickname");
            }
            if (data["photo"] != null)
            {
                user.Photo = data.GetValue<string>("photo");
            }
            if (data["remark"] != null)
            {
                user.Remark = data.GetValue<string>("remark");
            }
            if (data["enable"] != null)
            {
                user.Enabled = data.GetValue("enable", 1) == 1;
            }
            //if (data["system"] != null)
            //{
            //    user.System = data.GetValue("system", 0) == 1;
            //}
            if (data["refcode"] != null)
            {
                user.RefCode = data.GetValue<string>("refcode");
            }
            if (data["province"] != null)
            {
                user.Province = data.GetValue<string>("province");
            }
            if (data["city"] != null)
            {
                user.City = data.GetValue<string>("city");
            }
            if (data["County"] != null)
            {
                user.County = data.GetValue<string>("County");
            }
            userService.Update(user);

            SqlMapper.CacheManager.Flush("rbac.user");
            return Ok(null);
        }

        [HttpDelete, MultiAuthorize, Action("删除用户")]
        public IActionResult DelUser(string id)
        {
            userService.Delete(id);

            if (SqlMap.SqlId != "rbac.user")
            {
                SqlMapper.CacheManager.Flush(SqlMap.SqlId);
            }
            SqlMapper.CacheManager.Flush("rbac.user");
            return Ok(null);
        }
        #endregion

        #region role
        [HttpPost, MultiAuthorize, Action("用户授权")]
        public IActionResult PostUA(long userId, [FromBody]JObject data)
        {
            var roleIds = data["roles"].Value<string>();
            uAService.Save(userId, roleIds);

            SqlMapper.CacheManager.Flush("rbac.user");
            return Ok(null);
        }

        [HttpDelete, MultiAuthorize, Action("删除角色")]
        public IActionResult DelRole(string id)
        {
            roleService.Delete(id);
            return Ok(null);
        }

        [HttpPost, MultiAuthorize, Action("角色授权")]
        public IActionResult PostPA(long roleId, [FromBody] JObject data)
        {
            var objectIds = data["objects"].Value<string>();
            pAService.Save(roleId, objectIds);

            SqlMapper.CacheManager.Flush("rbac.obj");
            return Ok(null);
        }

        [HttpDelete, MultiAuthorize, Action("删除资源")]
        public IActionResult DelObj(string id)
        {
            objectService.Delete(id);
            return Ok(null);
        }
        #endregion
    }
}