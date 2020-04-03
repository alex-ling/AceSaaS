using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Data;
using Acesoft.Web.UI.Timeline;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Acesoft.Web.UI.Controllers
{
	[ApiExplorerSettings(GroupName = "WebUI")]
	[Route("api/[controller]/[action]")]
	public class TimelineController : ApiControllerBase
	{
		private readonly ILogger<ChartController> logger;

		public TimelineController(ILogger<ChartController> logger)
		{
			this.logger = logger;
		}

		[HttpGet, MultiAuthorize, DataSource, Action("获取数据")]
		public IActionResult Get()
		{
			CheckDataSourceParameter();

            var proId = App.GetQuery<long>("proid");
            var project = AppCtx.Session.QueryFirst(new RequestContext(SqlScope, SqlId)
                .SetParam(new { proId }));
            var progress = AppCtx.Session.Query(new RequestContext(SqlScope, $"{SqlId}_progress")
                .SetParam(new { proId }));
            var pro_start = project.kgrq != null ? (DateTime?)project.kgrq : null;
            var pro_end = project.jgrq != null ? (DateTime?)project.jgrq : null;

            var now = DateTime.Now;
            var tl = new TimelineV3();
            tl.Title = new Slide();
            var kgdate = "开工日期";
            var jgdate = "竣工日期";
            if (pro_start.HasValue)
            {
                kgdate = pro_start.Value.ToString("yyyy-MM-dd");
                tl.Title.Start_date = new SlideDate
                {
                    Year = pro_start.Value.Year,
                    Month = pro_start.Value.Month,
                    Day = pro_start.Value.Day
                };
            }
            if (pro_end.HasValue)
            {
                jgdate = pro_end.Value.ToString("yyyy-MM-dd");
                tl.Title.End_date = new SlideDate
                {
                    Year = pro_end.Value.Year,
                    Month = pro_end.Value.Month,
                    Day = pro_end.Value.Day
                };
            }
            tl.Title.Text = new SlideText
            {
                Headline = project.name,
                Text = project.jdap ?? "项目总体进度(未填写)"
            };
            tl.Title.Display_date = $"{kgdate} 至 {jgdate}";

            // 设置当前月份
            tl.Eras.Add(new Era
            {
                Start_date = new SlideDate { Year = now.Year, Month = now.Month, Day = 1 },
                End_date = new SlideDate { Year = now.Year, Month = now.Month, Day = 2 }
            });

            if (progress.Any())
            {
                foreach (var p in progress)
                {
                    var finish = (bool)p.finish;
                    var task = new Slide();
                    tl.Events.Add(task);

                    task.Unique_id = $"{p.id}";
                    task.Start_date = new SlideDate
                    {
                        Year = p.year,
                        Month = p.month,
                    };

                    task.Text = new SlideText
                    {
                        Text = $"<div class=\"evt-cont\">{p.content ?? "节点进度内容(未填写)"}</div>"
                    };

                    if (finish || (task.Start_date.Year <= now.Year && task.Start_date.Month < now.Month))
                    {
                        if (finish)
                        {
                            task.Text.Headline = $"<div class=\"success\">{p.title}</div>";
                            task.Text.Text += $"<div class=\"evt-status success\">已完成</div>";
                        }
                        else
                        {
                            task.Text.Headline = $"<div class=\"fail\">{p.title}</div>";
                            task.Text.Text += $"<div class=\"evt-status fail\">未完成</div>";
                        }
                    }
                    else
                    {
                        task.Text.Headline = $"<div>{p.title}</div>";
                    }

                    task.Text.Text += $"<div class=\"evt-mark\">{p.remark}</div>";
                }
            }
            else
            {
                var task = new Slide();
                tl.Events.Add(task);
                task.Unique_id = "empty";
                task.Start_date = new SlideDate { Year = now.Year, Month = now.Month };
                task.Text = new SlideText { Headline = "项目节点进度(未填写)", Text = "节点进度内容(未填写)" };
            }

            return Json(tl, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()                
            });
		}

        [HttpGet, MultiAuthorize, DataSource, Action("获取数据")]
        public IActionResult GetPlan()
        {
            CheckDataSourceParameter();

            var main = AppCtx.Session.QueryFirst(new RequestContext(SqlScope, SqlId));
            var progress = AppCtx.Session.Query(new RequestContext(SqlScope, $"{SqlId}_progress"));
            var start = main.dstart != null ? (DateTime?)main.dstart : null;
            var end = main.dend != null ? (DateTime?)main.dend : null;

            var now = DateTime.Now;
            var tl = new TimelineV3();
            tl.Title = new Slide();
            var startdate = "开工时间";
            var enddate = "结束时间";
            if (start.HasValue)
            {
                startdate = start.Value.ToString("yyyy-MM-dd");
                tl.Title.Start_date = new SlideDate
                {
                    Year = start.Value.Year,
                    Month = start.Value.Month,
                    Day = start.Value.Day
                };
            }
            if (end.HasValue)
            {
                enddate = end.Value.ToString("yyyy-MM-dd");
                tl.Title.End_date = new SlideDate
                {
                    Year = end.Value.Year,
                    Month = end.Value.Month,
                    Day = end.Value.Day
                };
            }
            tl.Title.Text = new SlideText
            {
                Headline = main.name,
                Text = main.text ?? "总体进度安排(未填写)"
            };
            tl.Title.Display_date = $"{startdate} 至 {enddate}";

            // 设置当前月份
            var next = now.AddDays(1);
            tl.Eras.Add(new Era
            {
                Start_date = new SlideDate { Year = now.Year, Month = now.Month, Day = now.Day },
                End_date = new SlideDate { Year = next.Year, Month = next.Month, Day = next.Day }
            });

            if (progress.Any())
            {
                foreach (var p in progress)
                {
                    var finish = (bool)p.finish;
                    var task = new Slide();
                    tl.Events.Add(task);

                    task.Unique_id = $"{p.id}";
                    if (p.dstart != null)
                    {
                        var d = (DateTime)p.dstart;
                        task.Start_date = new SlideDate
                        {
                            Year = d.Year,
                            Month = d.Month,
                            Day = d.Day
                        };
                    }
                    if (p.dend != null)
                    {
                        var d = (DateTime)p.dend;
                        task.End_date = new SlideDate
                        {
                            Year = d.Year,
                            Month = d.Month,
                            Day = d.Day
                        };
                    }

                    task.Text = new SlideText
                    {
                        Text = $"<div class=\"evt-cont\">{p.text ?? "节点进度内容(未填写)"}</div>"
                    };

                    if (finish || (task.Start_date.Year <= now.Year && task.Start_date.Month < now.Month))
                    {
                        if (finish)
                        {
                            task.Text.Headline = $"<div class=\"success\">{p.title}</div>";
                            task.Text.Text += $"<div class=\"evt-status success\">已完成</div>";
                        }
                        else
                        {
                            task.Text.Headline = $"<div class=\"fail\">{p.title}</div>";
                            task.Text.Text += $"<div class=\"evt-status fail\">未完成</div>";
                        }
                    }
                    else
                    {
                        task.Text.Headline = $"<div>{p.title}</div>";
                    }

                    task.Text.Text += $"<div class=\"evt-mark\">{p.remark}</div>";
                }
            }
            else
            {
                var task = new Slide();
                tl.Events.Add(task);
                task.Unique_id = "empty";
                task.Start_date = new SlideDate { Year = now.Year, Month = now.Month };
                task.Text = new SlideText { Headline = "节点进度(未填写)", Text = "节点进度内容(未填写)" };
            }

            return Json(tl, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}