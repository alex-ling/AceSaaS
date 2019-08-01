using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Web.WeChat.Entity;
using Microsoft.Extensions.Logging;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Menu;

namespace Acesoft.Web.WeChat.Services
{
	public class MenuService : Service<Wx_Menu>, IMenuService
	{
        private readonly ILogger<MenuService> logger;

        public MenuService(ILogger<MenuService> logger)
        {
            this.logger = logger;
        }

		public IEnumerable<Wx_Menu> GetMenus(long appId)
		{
			return Session.Query<Wx_Menu>(
                new RequestContext("wx", "get_wx_menu")
                .SetParam(new
			    {
				    appId
			    })
            );
		}

        public GetMenuResult GetMenu(Wx_App app)
        {
            return CommonApi.GetMenu(app.AppId);
        }

        public WxJsonResult CreateMenu(Wx_App app)
        {
            var menu = new ButtonGroup();
            var buttons = GetMenus(app.Id);

            var tops = from b in buttons
                         where !b.ParentId.HasValue
                         orderby b.OrderNo
                         select b;
            tops.Each(btn =>
            {
                if (btn.Type == WxButton.sub_button)
                {
                    var btns = new SubButton(btn.Name);
                    var childs = from b in buttons
                                 where b.ParentId == btn.Id
                                 orderby b.OrderNo
                                 select b;
                    childs.Each(btnc =>
                    {
                        btns.sub_button.Add(GetButton(btnc));
                    });
                    menu.button.Add(btns);
                }
                else
                {
                    menu.button.Add(GetButton(btn));
                }
            });
            
            return CommonApi.CreateMenu(app.AppId, menu);
        }

        private SingleButton GetButton(Wx_Menu b)
        {
            switch (b.Type)
            {
                case WxButton.view:
                    return new SingleViewButton
                    {
                        name = b.Name,
                        url = b.Value
                    };
                case WxButton.miniprogram:
                    {
                        string appid = b.ExtraValue.Split(':')[0];
                        string pagepath = b.ExtraValue.Split(':')[1];
                        return new SingleMiniProgramButton
                        {
                            name = b.Name,
                            appid = appid,
                            pagepath = pagepath,
                            url = b.Value
                        };
                    }
                case WxButton.scancode_push:
                    return new SingleScancodePushButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
                case WxButton.scancode_waitmsg:
                    return new SingleScancodeWaitmsgButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
                case WxButton.pic_sysphoto:
                    return new SinglePicSysphotoButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
                case WxButton.pic_photo_or_album:
                    return new SinglePicPhotoOrAlbumButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
                case WxButton.pic_weixin:
                    return new SinglePicWeixinButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
                case WxButton.location_select:
                    return new SingleLocationSelectButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
                case WxButton.media_id:
                    return new SingleMediaIdButton
                    {
                        name = b.Name,
                        media_id = b.Value
                    };
                case WxButton.view_limited:
                    return new SingleViewLimitedButton
                    {
                        name = b.Name,
                        media_id = b.Value
                    };
                default:
                    return new SingleClickButton
                    {
                        name = b.Name,
                        key = b.Value
                    };
            }
        }

        public WxJsonResult DeleteMenu(Wx_App app)
        {
            return CommonApi.DeleteMenu(app.AppId);
        }
    }
}
