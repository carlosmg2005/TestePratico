using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using TestePratico.Models;

public class RemovePessoaIdSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Pessoa))
        {
            schema.Properties.Remove("pessoaId");
        }
    }
}