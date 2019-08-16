using System;
using System.Collections.Generic;
using System.Linq;

using Acesoft.Data;
using Acesoft.Platform;
using Acesoft.Web.WeChat.Entity;
using HtmlAgilityPack;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
using Senparc.Weixin.MP.AdvancedAPIs.Media;

namespace Acesoft.Web.WeChat.Services
{
	public class NewsService : Service<Wx_News>, INewsService
	{
        private readonly IDictionary<string, object> vars;
        private IConfigService configService;
        private IMediaService mediaService;

        public NewsService(IConfigService configService, IMediaService mediaService)
        {
            this.configService = configService;
            this.mediaService = mediaService;
            this.vars = new Dictionary<string, object>
            {
                { "\t", "" }, { "\r", "" }, { "\n", "" }
            };
        }

		public IEnumerable<Wx_News> GetNews(string ids)
		{
			return base.Session.Query<Wx_News>(
                new RequestContext("wx", "get_wx_news")
                .SetParam(new
			    {
				    ids = ids.Split<long>(',')
			    })
            );
		}

        public void UpdateThumbMedia(Wx_News news)
        {
            Session.Execute(
                new RequestContext("wx", "exec_wx_news_thumb")
                .SetParam(new
                {
                    newsid = news.Id,
                    mediaid = news.ThumbMediaId,
                    wxurl = news.ThumbWxUrl
                })
            );
        }

        public void UploadNews(long appId, string newsIds, int count, string mediaId, string wxUrl)
        {
            Session.Execute(
                new RequestContext("wx", "exec_wx_news_upload")
                .SetParam(new
                {
                    appId,
                    newid = App.IdWorker.NextId(),
                    newsIds,
                    count,
                    mediaId,
                    wxUrl
                })
            );
        }

        public WxJsonResult UploadNews(Wx_App app, string newsIds, int openComment = 1)
        {
            DateTime now = DateTime.Now;
            var news = GetNews(newsIds).Select(n =>
            {
                GetThumbMedia(app, n);

                return new NewsModel
                {
                    title = n.Title,
                    author = n.Author,
                    content = GetWxHtml(app, n),
                    content_source_url = App.GetWebPath(n.SourceUrl, true),
                    digest = n.Digest,
                    show_cover_pic = "0",
                    thumb_media_id = n.ThumbMediaId,
                    need_open_comment = openComment
                };
            }).ToArray();

            var result = MediaApi.UploadNews(app.AppId, 300000, news);
            if (result.ErrorCodeValue == 0)
            {
                UploadNews(app.Id, newsIds, news.Length, result.media_id, result.url);
            }
            return result;
        }

        private string GetWxHtml(Wx_App app, Wx_News news)
        {
            var html = new HtmlDocument();
            var sysCfg = configService.GetConfig(app.Id);

            var content = news.Content.Replace(vars);
            var header = sysCfg["header_html"];
            var footer = sysCfg["footer_html"];
            var cover = "<p><img src=\"" + news.ThumbWxUrl + "\" /></p>";
            html.LoadHtml("<div>" + header + cover + content + footer + "</div>");

            var nodes = html.DocumentNode.SelectNodes("./div/p | ./div/div");
            if (nodes != null)
            {
                foreach (HtmlNode item in nodes)
                {
                    item.InnerHtml = item.InnerHtml.Trim();
                    SetStyle(item, "padding:0.25em 0");

                    var subImgs = item.SelectNodes(".//img");
                    if (subImgs == null || subImgs.Count <= 0)
                    {
                        var attr = item.Attributes["style"];
                        if (attr == null || attr.Value.IndexOf("text-align:center") < 0)
                        {
                            SetStyle(item, "text-indent:2em");
                        }
                    }
                    if (!item.HasChildNodes)
                    {
                        item.Remove();
                    }
                }
            }

            var imgs = html.DocumentNode.SelectNodes("//img");
            if (imgs != null)
            {
                foreach (HtmlNode img in imgs)
                {
                    var src = img.Attributes["src"].Value;
                    var result = WeChatApi.UploadImg(app.AppId, src);
                    if (result != null)
                    {
                        img.Attributes["src"].Value = result.url;
                    }
                    img.Attributes.Remove("width");
                    img.Attributes.Remove("height");

                    SetStyle(img, "width:100%!important;height:auto!important");
                }
            }
            return html.DocumentNode.InnerHtml;
        }

        private void GetThumbMedia(Wx_App app, Wx_News news)
        {
            if (!news.ThumbMediaId.HasValue())
            {
                var result = mediaService.UploadMedia(app, news.ThumbUrl.TrimStart(','));
                if (result.ErrorCodeValue != 0)
                {
                    throw new AceException(result.errcode.ToString());
                }
                news.ThumbMediaId = result.media_id;
                news.ThumbWxUrl = result.url;
                UpdateThumbMedia(news);
            }
        }

        private void SetStyle(HtmlNode node, string css)
        {
            var htmlAttribute = node.Attributes["style"];
            if (htmlAttribute != null)
            {
                htmlAttribute.Value = htmlAttribute.Value.TrimEnd(';') + ";" + css;
            }
            else
            {
                node.Attributes.Add("style", css);
            }
        }
    }
}
