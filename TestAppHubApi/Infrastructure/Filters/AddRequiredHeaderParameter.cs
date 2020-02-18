using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TestAppApi.Infrastructure.Filters
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var param = new OpenApiParameter();            
            param.Name = "authorization";            
            param.Description = "Basic Auth";
            param.Required = true;            
            param.In = ParameterLocation.Header;            
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            operation.Parameters.Add(param);
        }
    }
}
