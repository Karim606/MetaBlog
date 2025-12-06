using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Schema;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MetaBlog.Api.OpenApi
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true)]
    public class ReturnsCookieAttribute : Attribute
    {
        public Type BodyType    { get; }
        public string CookieName { get; }
        public int StatusCode { get; }
        public string Description { get; }
        public string Example { get; }

        public ReturnsCookieAttribute( string cookieName,int statusCode,string example = null, string description = null,Type bodyType=null)
        {
            
            CookieName = cookieName;
            Example = example;
            StatusCode = statusCode;
            BodyType = bodyType;
            Description = description;
        }
    }

    public class CookieOperationTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            
            var attributes = context.Description.ActionDescriptor.EndpointMetadata
                .OfType<ReturnsCookieAttribute>();

            
            foreach (var attr in attributes)
            {
                string code = attr.StatusCode.ToString();

                if (operation.Responses.ContainsKey(code))
                {
                    operation.Responses[code].Headers ??= new Dictionary<string, OpenApiHeader>();

                    operation.Responses[code].Headers[attr.CookieName] = new OpenApiHeader
                    {
                        Description = attr.Description,
                        Schema = new OpenApiSchema
                        {
                            Type = "string", 
                            Example = new Microsoft.OpenApi.Any.OpenApiString(
                                attr.Example ?? $"{attr.CookieName}=example; HttpOnly; Secure; SameSite=Strict")
                        }
                    };
                }

                
                if (attr.BodyType != null && operation.Responses.ContainsKey(code))
                {
                    operation.Responses[code].Content ??= new Dictionary<string, OpenApiMediaType>();
                    operation.Responses[code].Content["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = attr.BodyType.Name.ToLower()
                        }
                    };
                }
            }

            return Task.CompletedTask;
        }
    }
}
