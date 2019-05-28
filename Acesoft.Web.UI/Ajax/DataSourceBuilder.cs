using System;

using Acesoft.Data;
using Acesoft.Rbac;
using Microsoft.Extensions.DependencyInjection;

namespace Acesoft.Web.UI.Ajax
{
	public class DataSourceBuilder : AjaxObjectBuilder<DataSource, DataSourceBuilder>
	{
		public DataSourceBuilder(DataSource ajaxObject)
			: base(ajaxObject)
		{
		}

		public virtual DataSourceBuilder DataSource(string dataSource)
		{
			JsonObject.RouteValues["ds"] = dataSource;
			return this;
		}

		public virtual DataSourceBuilder Dict(string dict)
		{
			JsonObject.RouteValues["ds"] = "sys.dictitem_list";
			return RouteValue("dict", dict);
		}

		public virtual DataSourceBuilder NullSelect(bool selected = true)
		{
			return RouteValue("nullselect", selected);
		}

		public virtual DataSourceBuilder QueryParams(object param)
		{
			JsonObject.QueryParams = param;
			return this;
		}

		public virtual DataSourceBuilder PageSize(int pageSize)
		{
			JsonObject.RouteValues["pagesize"] = pageSize;
			return this;
		}

		public virtual DataSourceBuilder Form(Func<object> func)
		{
			JsonObject.FormData = func();
			return this;
		}

        public virtual DataSourceBuilder Load(string sqlFullId, string requestId = "id")
        {
            return Load<long>(sqlFullId, requestId);
        }

        public virtual DataSourceBuilder Load<T>(string sqlFullId, string requestId = "id", T defaultId = default(T))
        {
            return Load(sqlFullId, new { id = App.GetQuery(requestId, defaultId) });
        }

        public virtual DataSourceBuilder Load(string sqlFullId, object param)
        {
            JsonObject.RouteValues["ds"] = sqlFullId;

            var ctx = new RequestContext(sqlFullId)
                .SetCmdType(CmdType.select)
                .SetParam(param)
                .SetExtraParam(JsonObject.Widget.Ace.AC.Params);
            JsonObject.FormData = JsonObject.Widget.Ace.Session.QueryFirst(ctx);
            JsonObject.IsEdit = true;
            return this;
        }

        public virtual DataSourceBuilder Form(object data)
		{
			string query = App.GetQuery("id", "");
			if (query.HasValue())
			{
				var ds = JsonObject.RouteValues.GetValue("ds", "");
				if (ds == null)
				{
					throw new AceException("未设置ds数据源参数，格式：SqlScope.SqlId");
				}

                var ctx = new RequestContext(ds)
                    .SetCmdType(CmdType.select)
                    .SetNewObj(data)
                    .SetParam(new
                    {
                        id = query
                    });
				JsonObject.FormData = JsonObject.Widget.Ace.Session.QueryFirst(ctx);
				JsonObject.IsEdit = true;
			}
			if (JsonObject.FormData == null)
			{
				JsonObject.FormData = data;
			}
			return this;
		}

		public virtual DataSourceBuilder Form<T>(Func<T, object> action, object data) where T : EntityBase
		{
			var service = JsonObject.Widget.Context.RequestServices.GetService<IService<T>>();
			var id = App.GetQuery<long>("id", 0);
			if (id > 0)
			{
				JsonObject.FormData = action(service.Get(id));
				JsonObject.IsEdit = true;
			}
			else
			{
				JsonObject.FormData = data;
			}
			return this;
		}

		public virtual DataSourceBuilder HttpSave(string controller = null, string action = null, object routeValues = null)
		{
			HttpMethod httpMethod = JsonObject.IsEdit ? HttpMethod.put : HttpMethod.post;
			return Route(httpMethod, controller, $"{httpMethod}{action}", routeValues);
		}

		public DataSourceBuilder Events(Action<DataSourceEventBuilder> clientEventsAction)
		{
			clientEventsAction(new DataSourceEventBuilder(JsonObject.Events));
			return this;
		}
	}
}
