using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Extension;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerGenSetup(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name;
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            opt.IncludeXmlComments($"{xmlPath}.xml");

            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT授权，Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            opt.OperationFilter<AuthorizeCheckOperationFilter>();
        });
        return services;
    }
}

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any()
            || context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Any();

        if (!hasAuthorize) return;

        operation.Security ??= new List<OpenApiSecurityRequirement>();

        operation.Security.Add(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        }
                    },
                    Array.Empty<string>()
                }
            }
        );
    }
}