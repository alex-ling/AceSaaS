using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Routing;

namespace Acesoft.Web.UI.Ajax
{
	public abstract class JsonObject
	{
		public string Url { get; set; }
		public HttpMethod? Method { get; set; }
        public RouteValueDictionary RouteValues { get; private set; }
        public IDictionary<string, object> Events { get; private set; }
        public IWidget Widget { get; private set; }
        public bool Dynamic { get; set; }

        protected JsonObject(IWidget widget)
		{
			Widget = widget;
			RouteValues = new RouteValueDictionary();
			Events = new Dictionary<string, object>();
		}

		protected abstract void Serialize(IDictionary<string, object> json);

		public virtual void GenerateUrl()
		{
			if (!RouteValues.ContainsKey("app"))
			{
				RouteValues["app"] = Widget.Ace.AppName;
			}
			Url = GetApi(RouteValues);
			Dynamic = true;
		}

		public IDictionary<string, object> ToJson()
		{
			var dictionary = new Dictionary<string, object>();
			Serialize(dictionary);
			if (Events.Any())
			{
				dictionary.Merge(Events, true);
			}
			return dictionary;
		}

        private string GetApi(RouteValueDictionary routeValues)
        {
            var api = App.GetWebPath("api/{controller}/{action}/{id}");

            foreach (var route in routeValues)
            {
                if (route.Key == "controller" || route.Key == "action" || route.Key == "id")
                {
                    api = api.Replace("{" + route.Key + "}", route.Value.ToString());
                }
                else
                {
                    api += (api.IndexOf("?") > 0) ? "&" : "?";
                    api += $"{route.Key}={WebUtility.UrlEncode(route.Value.ToString())}";
                }
            }

            return api.Replace("/{action}", "").Replace("/{id}", "");
        }
	}
}
