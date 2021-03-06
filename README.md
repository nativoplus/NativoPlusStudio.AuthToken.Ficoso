# NativoPlusStudio.AuthToken.Ficoso

NativoPlusStudio.AuthToken.Ficoso is part of the NativoPlusStudio.AuthToken set of libraries that can be used to retrieve the auth token to be able to interface with the FICOSO APIs.

### Usage

To use this nuget package you can use the AddFicosoAuthTokenProvider extension method to register the FicosoAuthTokenProvider service in a Console app or api. Here is an example:

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Ficoso.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ExampleApp
{
    class Program
    {
        public static IServiceProvider serviceProvider;
        public static IAuthTokenGenerator authTokenGenerator;
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var services = new ServiceCollection();

            services.AddFicosoAuthTokenProvider((options, builder) =>
            {
                options.AddFicosoOptions(
                    protectedResource: configuration["FicosoOptions:ProtectedResourceName"],
                    accessTokenEndpoint: configuration["FicosoOptions:AccessTokenEndpoint"],
                    clientId: configuration["FicosoOptions:ClientId"],
                    clientSecret: configuration["FicosoOptions:ClientSecret"],
                    grantType: configuration["FicosoOptions:GrantType"],
                    userName: configuration["FicosoOptions:UserName"],
                    password: configuration["FicosoOptions:Password"],
                    scope: configuration["FicosoOptions:Scope"],
                    url: configuration["FicosoOptions:Url"],
                    includeEncryptedTokenInResponse: true
                );
            });

            serviceProvider = services.BuildServiceProvider();

            authTokenGenerator = serviceProvider.GetRequiredService<IAuthTokenGenerator>();

            var token = authTokenGenerator.GetTokenAsync(protectedResource: configuration["FicosoOptions:ProtectedResourceName"]).GetAwaiter().GetResult();

            Console.WriteLine(JsonConvert.SerializeObject(token));
        }
    }
}
```

This nuget package also includes extension methods to extends IHttpClientBuilder to add a Retry Authorization algorithm that fetches the token if the consequent requests to the FICOSO endpoints return a 401 Unauthorized.

Example of these extension methods:

```csharp
using FicosoLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.AuthToken.Ficoso.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ExampleApp
{
    class Program
    {
        public static IServiceProvider serviceProvider;
        public static FCSHttpClient client;
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppContext.BaseDirectory}/appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var services = new ServiceCollection();

            services.AddFicosoAuthTokenProvider((options, builder) =>
            {
                options.AddFicosoOptions(
                    protectedResource: configuration["FicosoOptions:ProtectedResourceName"],
                    accessTokenEndpoint: configuration["FicosoOptions:AccessTokenEndpoint"],
                    clientId: configuration["FicosoOptions:ClientId"],
                    clientSecret: configuration["FicosoOptions:ClientSecret"],
                    grantType: configuration["FicosoOptions:GrantType"],
                    userName: configuration["FicosoOptions:UserName"],
                    password: configuration["FicosoOptions:Password"],
                    scope: configuration["FicosoOptions:Scope"],
                    url: configuration["FicosoOptions:Url"],
                    includeEncryptedTokenInResponse: true
                );
            });

            services.AddHttpClient<FCSHttpClient>(async (provider, client) =>
            {
                client.BaseAddress = new Uri(configuration["FicosoOptions:Url"]);

                var token = await provider.GetService<IAuthTokenGenerator>().GetTokenAsync(protectedResource: configuration["FicosoOptions:ProtectedResourceName"]);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(scheme: "Bearer", parameter: token.Token);
            })
            .AddRefreshFicosoTokenPolicyWithJitteredBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithConstantBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithExponentialBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithLinearBackoff(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], initialDelayInSeconds: 1, retryCount: 2)
            //.AddRefreshFicosoTokenPolicyWithRetry(protectedResourceName: configuration["FicosoOptions:ProtectedResourceName"], retryCount: 2)
            ;

            serviceProvider = services.BuildServiceProvider();

            client = serviceProvider.GetRequiredService<FCSHttpClient>();

            var token = client.GetFillingAsync("replace this with your filing id").GetAwaiter().GetResult();

            Console.WriteLine(token.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}


```

The above code can be found in a console app in the project called ExampleApp.

Visit the following repositories for examples on how to use other auth token nuget packages

https://github.com/nativoplus/NativoPlusStudio.AuthToken.SymmetricEncryption
https://github.com/nativoplus/NativoPlusStudio.AuthToken.SqlServerCaching
https://github.com/nativoplus/NativoPlusStudio.AuthToken.Core
https://github.com/nativoplus/NativoPlusStudio.AuthToken.Fis