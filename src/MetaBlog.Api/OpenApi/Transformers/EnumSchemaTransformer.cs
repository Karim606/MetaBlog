using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MetaBlog.Api.OpenApi.Transformers
{
    public class EnumSchemaTransformer : IOpenApiSchemaTransformer
    {
        public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
        {
            if (!context.GetType().IsEnum)
                return Task.CompletedTask;

            schema.Enum.Clear();

            foreach (var name in Enum.GetNames(context.GetType()))
            {
                schema.Enum.Add(new OpenApiString(name));
            }

            return Task.CompletedTask;
        }

        
    }
}
