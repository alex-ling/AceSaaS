using System.Collections.Generic;
using System.Linq;

using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Acesoft.Web.Mvc
{
	public class DataSourceFilter : IOperationFilter
	{
		public void Apply(Operation operation, OperationFilterContext context)
		{
			if (operation.Parameters == null)
			{
				operation.Parameters = new List<IParameter>();
			}

			if (context.MethodInfo.CustomAttributes.OfType<DataSourceAttribute>().Any())
			{
				operation.Parameters.Add(new NonBodyParameter
				{
					Name = "app",
					In = "query",
					Description = "AppName",
					Required = true,
					Type = "string"
				});
				operation.Parameters.Add(new NonBodyParameter
				{
					Name = "ds",
					In = "query",
					Description = "DataSource",
					Required = true,
					Type = "string"
				});
			}
		}
	}
}
