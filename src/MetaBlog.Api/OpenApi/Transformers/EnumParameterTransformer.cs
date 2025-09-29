//using Microsoft.AspNetCore.OpenApi;
//using Microsoft.OpenApi.Any;
//using Microsoft.OpenApi.Models;

//namespace MetaBlog.Api.OpenApi.Transformers
//{
//    public class EnumParameterTransformer : IOpenApiOperationTransformer
//    {
//        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
//        {
//            foreach (var parameter in operation.Parameters)
//            {
//                // See if this parameter has an enum CLR type
//                var clrType = context.GetType()?
//                    .GetParameters()
//                    .FirstOrDefault(p => string.Equals(p.Name, parameter.Name, StringComparison.OrdinalIgnoreCase))
//                    ?.ParameterType;

//                if (clrType != null && clrType.IsEnum)
//                {
//                    parameter.Schema.Enum.Clear();
//                    foreach (var name in Enum.GetNames(clrType))
//                    {
//                        parameter.Schema.Enum.Add(new OpenApiString(name));
//                    }
//                    parameter.Schema.Type = "string"; // represent enums as strings
//                }
//            }

//            return Task.CompletedTask;
//        }
//    }
//}
