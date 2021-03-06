﻿using System;
using System.Collections.Generic;
using System.Linq;

using static Dapper.SqlMapper;
using Acesoft.Data;
using Acesoft.Rbac.Entity;
using Acesoft.Util;

namespace Acesoft.Rbac.Services
{
    public class UserService : Service<Rbac_User>, IUserService
    {
        private IScaleService scaleService;
        private IUAService uaService;

        public UserService(IScaleService scaleService, IUAService uaService)
        {
            this.scaleService = scaleService;
            this.uaService = uaService;
        }

        #region get
        public Rbac_User GetByLoginName(string loginName)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("rbac", "get_user_by_loginname")
                .SetParam(new
                {
                    loginName
                })
            );
        }

        public Rbac_User GetByMail(string mail)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("rbac", "get_user_by_mail")
                .SetParam(new
                {
                    mail
                })
            );
        }

        public Rbac_User GetByMobile(string mobile)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("rbac", "get_user_by_mobile")
                .SetParam(new
                {
                    mobile
                })
            );
        }

        public Rbac_User GetByRefCode(string refcode)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("rbac", "get_user_by_refcode")
                .SetParam(new
                {
                    refcode
                })
            );
        }

        public Rbac_User GetByAuth(long appId, string authId)
        {
            return Session.QueryFirst<Rbac_User>(
                new RequestContext("rbac", "get_user_by_authid")
                .SetParam(new
                {
                    appId,
                    authId
                })
            );
        }
        #endregion

        #region query
        public Rbac_User QueryById(long id, string authId)
        {
            return Session.QueryMultiple<Rbac_User>(
                new RequestContext("rbac", "query_user_by_id")
                .SetParam(new
                {
                    id,
                    authId
                }),
                reader => GetUser(reader)
            );
        }

        public Rbac_User QueryByUserName(string userName, string authId = "none")
        {
            return Session.QueryMultiple<Rbac_User>(
                new RequestContext("rbac", "query_user_by_username")
                .SetParam(new
                {
                    userName,
                    authId
                }),
                reader => GetUser(reader)
            );
        }

        public Rbac_User QueryByAuth(long appId, string authId)
        {
            return Session.QueryMultiple<Rbac_User>(
                new RequestContext("rbac", "query_user_by_authid")
                .SetParam(new
                {
                    appId,
                    authId
                }),
                reader => GetUser(reader)
            );
        }
        #endregion

        #region check
        public Rbac_User CheckUser(string userName, string password)
        {
            int tryTimes = 0, lockMinutes = 60;
            var user = QueryByUserName(userName);

            Check.Assert(user == null, "登录名不存在");
            Check.Require(user.Enabled, "用户已被停用，禁止登录");
            Check.Assert(tryTimes > 0 && user.TryTimes == tryTimes, $"密码错误超限，禁止登录{lockMinutes}分钟");

            // Set try times to ZERO when after lock minutes
            if (tryTimes > 0 && user.DLogin.HasValue && user.DLogin.Value.AddMinutes(lockMinutes) < DateTime.Now)
            {
                user.TryTimes = 0;
            }

            // Check password
            if (user.Password != CryptoHelper.ComputeMD5(user.HashId, password))
            {
                if (tryTimes > 0)
                {
                    int hasTimes = tryTimes - ++user.TryTimes;
                    Check.Assert(hasTimes <= 0,
                        $"密码错误，帐号将锁定{lockMinutes}分钟",
                        $"密码错误，还有{hasTimes}次尝试机会");
                }
                else
                {
                    throw new AceException("密码错误");
                }
            }
            else
            {
                user.TryTimes = 0;
            }

            // Check having role
            Check.Assert(user.LoginName != "root" && user.Rbac_UAs.Count == 0, "用户无任何角色权限");

            // update login date and ip
            UpdateLogin(user);

            return user;
        }
        #endregion

        #region delete
        public int Delete(long id)
        {
            return Session.Execute(
                new RequestContext("rbac", "user")
                .SetCmdType(CmdType.delete)
                .SetParam(new
                {
                    id,
                    ids = new long[] { id }
                })
            );
        }

        public void Delete(string userIds)
        {
            // delete scale auto 
            var deleteScale = App.GetQuery("deletescale", false);

            try
            {
                Session.BeginTransaction();

                userIds.Split<long>().Each(userId =>
                {
                    // delete ua
                    uaService.DeleteByUser(userId);

                    // delete user
                    Delete(userId);

                    // delete scale
                    if (deleteScale)
                    {
                        scaleService.DeleteByUser(userId);
                    }
                });

                Session.Commit();
            }
            catch (Exception ex)
            {
                Session.Rollback();

                throw new AceException(ex.GetMessage());
            }
        }
        #endregion

        #region update
        public void UpdateAuth(Rbac_User user, long appId, string authId, string authType)
        {
            var auth = Session.QueryFirst<Rbac_Auth>(
                new RequestContext("rbac", "exec_user_auth")
                .SetParam(new
                {
                    newid = App.IdWorker.NextId(),
                    userId = user.Id,
                    appId,
                    authType,
                    authId
                })
            );
            user.Rbac_Auths.Add(auth);
        }

        public void UpdateLogin(Rbac_User user)
        {
            user.DLogin = DateTime.Now;
            user.LoginIP = App.Context.GetClientIp();
            Update(user);
        }
        #endregion

        #region private
        private Rbac_User GetUser(GridReader reader)
        {
            var user = reader.Read<Rbac_User>(true).SingleOrDefault();
            if (user != null)
            {
                user.Rbac_UAs = reader.Read<Rbac_UA>(true).ToList();
                user.Rbac_Params = reader.Read<Rbac_Param>(true).ToList();
                user.Rbac_Auths = reader.Read<Rbac_Auth>(true).ToList();
            }
            return user;
        }
        #endregion
    }
}
