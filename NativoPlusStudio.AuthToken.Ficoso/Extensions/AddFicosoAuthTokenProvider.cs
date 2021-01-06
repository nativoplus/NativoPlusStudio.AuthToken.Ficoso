using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core;
using NativoPlusStudio.AuthToken.Core.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Ficoso.Configurations;
using Polly;
using System;

namespace NativoPlusStudio.AuthToken.Ficoso.Extensions
{
    public static class FicosoServicesExtension
    {
        public static IServiceCollection AddFicosoAuthTokenProvider(this IServiceCollection services,
            Action<FicosoAuthTokenOptions, AuthTokenServicesBuilder> actions
            )
        {
            var ficosoOptions = new FicosoAuthTokenOptions();
            var servicesBuilder = new AuthTokenServicesBuilder() { Services = services };

            actions.Invoke(ficosoOptions, servicesBuilder);

            services.AddTokenProviderHelper(ficosoOptions.ProtectedResource, () => 
            {
                services.Configure<FicosoAuthTokenOptions>(f =>
                {
                    f.Scope = ficosoOptions.Scope;
                    f.ClientSecret = ficosoOptions.ClientSecret;
                    f.AccessTokenEndpoint = ficosoOptions.AccessTokenEndpoint;
                    f.ClientId = ficosoOptions.ClientId;
                    f.GrantType = ficosoOptions.GrantType;
                    f.UserName = ficosoOptions.UserName;
                    f.Password = ficosoOptions.Password;
                    f.ProtectedResource = ficosoOptions.ProtectedResource;
                    f.IncludeEncryptedTokenInResponse = ficosoOptions.IncludeEncryptedTokenInResponse;
                });

                services
                .AddHttpClient<IAuthTokenProvider, FicosoAuthTokenProvider>(client =>
                {
                    client.BaseAddress = new Uri(ficosoOptions.Url);
                })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }))
                .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                ));

                services.AddTransient(implementationFactory => servicesBuilder.EncryptionService);
                services.AddTransient(implementationFactory => servicesBuilder.TokenCacheService);
            });

            return services;
        }
    }
}
