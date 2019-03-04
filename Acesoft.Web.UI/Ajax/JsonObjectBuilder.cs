namespace Acesoft.Web.UI.Ajax
{
	public abstract class JsonObjectBuilder<J, B> where J : JsonObject where B : JsonObjectBuilder<J, B>
	{
		public J JsonObject
		{
			get;
			private set;
		}

		public JsonObjectBuilder(J jsonObj)
		{
			JsonObject = jsonObj;
		}
	}
}
