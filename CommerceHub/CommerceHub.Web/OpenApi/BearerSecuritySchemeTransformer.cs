using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CommerceHub.Web.OpenApi
{
    public class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(
            OpenApiDocument document, 
            OpenApiDocumentTransformerContext context, 
            CancellationToken cancellationToken)
        {
            var schemes = await authenticationSchemeProvider.GetAllSchemesAsync();
            if (!schemes.Any(scheme=>scheme.Name == JwtBearerDefaults.AuthenticationScheme))
            {
                return;
            }

            var bearerScheme = new OpenApiSecurityScheme
            {

                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT token'ı başında 'Bearer' ifadesi yazmadan sadece token'i içerecek biçimde yazın"

            };

          
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme] = bearerScheme;


            var schemeReference = new OpenApiSecuritySchemeReference(JwtBearerDefaults.AuthenticationScheme);

            foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations.Values))
            {
                operation.Security ??= new List<OpenApiSecurityRequirement>();
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [schemeReference] = new List<string>()
                });
            }


        }
    }
}

