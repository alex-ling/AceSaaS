using System;
using System.Collections.Generic;

namespace Acesoft.Web.UI.Ajax
{
	public class AjaxObjectBuilder<A, B> : JsonObjectBuilder<A, B> where A : AjaxObject where B : AjaxObjectBuilder<A, B>
	{
		public AjaxObjectBuilder(A ajaxObject)
			: base(ajaxObject)
		{
		}

		public virtual B Controller(string controller)
		{
			base.JsonObject.RouteValues["controller"] = controller;
			return this as B;
		}

		public virtual B Action(string action)
		{
			base.JsonObject.RouteValues["action"] = action;
			return this as B;
		}

		public virtual B RouteValues(object routeValues)
		{
			base.JsonObject.RouteValues.Merge(routeValues, true);
			return this as B;
		}

		public virtual B RouteValue(string name, object value)
		{
			base.JsonObject.RouteValues[name] = value;
			return this as B;
		}

		public virtual B RouteQuery(params string[] names)
		{
            names.Each(name =>
			{
				base.JsonObject.RouteValues[name] = App.GetQuery<string>(name, "");
			});
			return this as B;
		}

		public virtual B HttpGet(string controller = null, string action = null, object routeValues = null)
		{
			return Route(HttpMethod.get, controller, action, routeValues);
		}

		public virtual B HttpPost(string controller = null, string action = null, object routeValues = null)
		{
			return Route(HttpMethod.post, controller, action, routeValues);
		}

		public virtual B HttpPut(string controller = null, string action = null, object routeValues = null)
		{
			return Route(HttpMethod.put, controller, action, routeValues);
		}

		public virtual B HttpDelete(string controller = null, string action = null, object routeValues = null)
		{
			return Route(HttpMethod.delete, controller, action, routeValues);
		}

		public virtual B Route(HttpMethod type, string controller = null, string action = null, object routeValues = null)
		{
			base.JsonObject.Method = type;
			if (controller != null)
			{
				base.JsonObject.RouteValues["controller"] = controller;
			}
			if (action != null)
			{
				base.JsonObject.RouteValues["action"] = action;
			}
			if (routeValues != null)
			{
				base.JsonObject.RouteValues.Merge(routeValues, true);
			}
			base.JsonObject.GenerateUrl();
			return this as B;
		}

		public virtual B Accepts(string accepts)
		{
			base.JsonObject.Accepts = accepts;
			return this as B;
		}

		public virtual B Async(bool async = true)
		{
			base.JsonObject.Async = async;
			return this as B;
		}

		public virtual B Cache(bool cache = true)
		{
			base.JsonObject.Cache = cache;
			return this as B;
		}

		public virtual B Contents(string contents)
		{
			base.JsonObject.Contents = contents;
			return this as B;
		}

		public virtual B ContentType(string contentType)
		{
			base.JsonObject.ContentType = contentType;
			return this as B;
		}

		public virtual B Context(string context)
		{
			base.JsonObject.Context = context;
			return this as B;
		}

		public virtual B Converters(string converters)
		{
			base.JsonObject.Converters = converters;
			return this as B;
		}

		public virtual B CrossDomain(bool crossDomain = true)
		{
			base.JsonObject.CrossDomain = crossDomain;
			return this as B;
		}

		public virtual B Data(string data)
		{
			base.JsonObject.Data = data;
			return this as B;
		}

		public virtual B DataType(DataType dataType)
		{
			base.JsonObject.DataType = dataType;
			return this as B;
		}

		public virtual B Global(bool global = true)
		{
			base.JsonObject.Global = global;
			return this as B;
		}

		public virtual B Headers(string headers)
		{
			base.JsonObject.Headers = headers;
			return this as B;
		}

		public virtual B IfModified(bool ifModified = true)
		{
			base.JsonObject.IfModified = ifModified;
			return this as B;
		}

		public virtual B IsLocal(bool isLocal = true)
		{
			base.JsonObject.IsLocal = isLocal;
			return this as B;
		}

		public virtual B Jsonp(string jsonp)
		{
			base.JsonObject.Jsonp = jsonp;
			return this as B;
		}

		public virtual B JsonpCallback(string jsonpCallback)
		{
			base.JsonObject.JsonpCallback = jsonpCallback;
			return this as B;
		}

		public virtual B MimeType(string mimeType)
		{
			base.JsonObject.MimeType = mimeType;
			return this as B;
		}

		public virtual B Password(string password)
		{
			base.JsonObject.Password = password;
			return this as B;
		}

		public virtual B ProcessData(bool processData = true)
		{
			base.JsonObject.ProcessData = processData;
			return this as B;
		}

		public virtual B ScriptCharset(string scriptCharset)
		{
			base.JsonObject.ScriptCharset = scriptCharset;
			return this as B;
		}

		public virtual B StatusCode(string statusCode)
		{
			base.JsonObject.StatusCode = statusCode;
			return this as B;
		}

		public virtual B Traditional(bool traditional = true)
		{
			base.JsonObject.Traditional = traditional;
			return this as B;
		}

		public virtual B Timeout(int timeout)
		{
			base.JsonObject.Timeout = timeout;
			return this as B;
		}

		public virtual B UserName(string userName)
		{
			base.JsonObject.UserName = userName;
			return this as B;
		}

		public virtual B Url(string url)
		{
			base.JsonObject.Url = url;
			return this as B;
		}

		public virtual B Type(HttpMethod type)
		{
			base.JsonObject.Method = type;
			return this as B;
		}
	}
}
