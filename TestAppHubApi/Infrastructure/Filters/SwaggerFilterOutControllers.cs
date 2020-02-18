using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TestAppApi.Infrastructure.Filters
{
    public class SwaggerFilterOutControllers : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (!apiDescription.RelativePath.Contains("api/"))
                {
                    swaggerDoc.Paths.Remove("/" + apiDescription.RelativePath);
                }
            }
        }
    }
}
