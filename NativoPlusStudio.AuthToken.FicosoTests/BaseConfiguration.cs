﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativoPlusStudio.AuthToken.Ficoso.Extensions;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using FicosoLib;

namespace NativoPlusStudio.AuthToken.FicosoTests
{
    public abstract class BaseConfiguration
    {
        public static IServiceProvider serviceProvider;
        public static IConfiguration configuration;

        public BaseConfiguration()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var services = new ServiceCollection();

            //services.AddFicosoAuthTokenProvider((options, builder) =>
            //{
            //    options.AddFicosoOptions(
            //        protectedResource: configuration["FicosoOptions:ProtectedResourceName"],
            //        accessTokenEndpoint: configuration["FicosoOptions:AccessTokenEndpoint"],
            //        clientId: configuration["FicosoOptions:ClientId"],
            //        clientSecret: configuration["FicosoOptions:ClientSecret"],
            //        grantType: configuration["FicosoOptions:GrantType"],
            //        userName: configuration["FicosoOptions:UserName"],
            //        password: configuration["FicosoOptions:Password"],
            //        scope: configuration["FicosoOptions:Scope"],
            //        url: configuration["FicosoOptions:Url"],
            //        includeEncryptedTokenInResponse: true
            //    );
            //});

            //services.AddHttpClient<FCSHttpClient>(async (provider, client) =>
            //{
            //    client.BaseAddress = new Uri(configuration["FicosoOptions:Url"]);

            //    var token = await provider.GetService<IAuthTokenGenerator>().GetTokenAsync(protectedResource: configuration["FicosoOptions:ProtectedResourceName"]);
            //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "Bearer", parameter: token.Token);
            //})
            //.AddRefreshFicosoTokenPolicyWithJitteredBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithConstantBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithExponentialBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithLinearBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithRetry(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], retryCount: 2)
            ;

            serviceProvider = services.BuildServiceProvider();
        }
    }
}
