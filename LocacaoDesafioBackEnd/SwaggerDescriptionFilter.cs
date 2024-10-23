using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class SwaggerDescriptionFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath.Contains("motos"))
        {
            operation.Summary = "Operação para gerenciar motos";
            operation.Description = "Endpoint para realizar operações CRUD em motos.";
        }
    }
}
