using NativoPlusStudio.AuthToken.Core.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Core.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace NativoPlusStudio.AuthToken.Ficoso.Extensions
{
    public static class FicosoHelperExtensions
    {
        public static IHttpClientBuilder AddRefreshFicosoTokenPolicyWithJitteredBackoff(this IHttpClientBuilder builder, string protectedResourceName, int initialDelayInSeconds, int retryCount)
        {
             builder.AddPolicyHandler((provider, request) => provider
                .GetRequiredService<IAuthTokenGenerator>()
                .CreateTokenRefreshPolicy(
                    request,
                    protectedResourceName,
                    async (generator, message, protectedResource) =>
                    {
                        var token = await generator.GetTokenAsync(protectedResource);
                        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                    },
                    BackoffAlgorithmTypeEnums.Jitter,
                    initialDelayInSeconds,
                    retryCount
                ));

            return builder;
        }public static IHttpClientBuilder AddRefreshFicosoTokenPolicyWithLinearBackoff(this IHttpClientBuilder builder, string protectedResourceName, int initialDelayInSeconds, int retryCount)
        {
            builder.AddPolicyHandler((provider, request) => provider
              .GetRequiredService<IAuthTokenGenerator>()
              .CreateTokenRefreshPolicy(
                    request,
                    protectedResourceName,
                    async (generator, message, protectedResource) =>
                    {
                        var token = await generator.GetTokenAsync(protectedResource);
                        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                    },
                    BackoffAlgorithmTypeEnums.Linear,
                    initialDelayInSeconds,
                    retryCount
                ));

            return builder;
        }public static IHttpClientBuilder AddRefreshFicosoTokenPolicyWithConstantBackoff(this IHttpClientBuilder builder, string protectedResourceName, int initialDelayInSeconds, int retryCount)
        {
            builder.AddPolicyHandler((provider, request) => provider
              .GetRequiredService<IAuthTokenGenerator>()
              .CreateTokenRefreshPolicy(
                    request,
                    protectedResourceName,
                    async (generator, message, protectedResource) =>
                    {
                        var token = await generator.GetTokenAsync(protectedResource);
                        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                    },
                    BackoffAlgorithmTypeEnums.Constant,
                    initialDelayInSeconds,
                    retryCount
                ));

            return builder;
        }public static IHttpClientBuilder AddRefreshFicosoTokenPolicyWithExponentialBackoff(this IHttpClientBuilder builder, string protectedResourceName, int initialDelayInSeconds, int retryCount)
        {
            builder.AddPolicyHandler((provider, request) => provider
              .GetRequiredService<IAuthTokenGenerator>()
              .CreateTokenRefreshPolicy(
                    request,
                    protectedResourceName,
                    async (generator, message, protectedResource) =>
                    {
                        var token = await generator.GetTokenAsync(protectedResource);
                        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                    },
                    BackoffAlgorithmTypeEnums.Exponential,
                    initialDelayInSeconds,
                    retryCount
                ));

            return builder;
        }public static IHttpClientBuilder AddRefreshFicosoTokenPolicyWithRetry(this IHttpClientBuilder builder, string protectedResourceName, int retryCount)
        {
            builder.AddPolicyHandler((provider, request) => provider
              .GetRequiredService<IAuthTokenGenerator>()
              .CreateTokenRefreshPolicy(
                    request,
                    protectedResourceName,
                    async (generator, message, protectedResource) =>
                    {
                        var token = await generator.GetTokenAsync(protectedResource);
                        message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
                    },
                    retryCount
                ));

            return builder;
        }
    }
}
