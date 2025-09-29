﻿using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace MetaBlog.Api.OpenApi.Transformers
{
    public class VersionInfoTransformer : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var version = context.DocumentName;
            document.Info.Version = version;
            document.Info.Title = $"MetaBlog API {version}";
            return Task.CompletedTask;
        }
    }
}
