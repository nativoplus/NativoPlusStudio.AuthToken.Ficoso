using NativoPlusStudio.AuthToken.Core.DTOs;

namespace NativoPlusStudio.AuthToken.Ficoso.Configurations
{
    public class FicosoAuthTokenOptions : BaseOptions
    {
        public string ProtectedResource { get; set; }
        public string AccessTokenEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Scope { get; set; }
        public string Url { get; set; }

        public void AddFicosoOptions(string accessTokenEndpoint,
         string clientId,
         string clientSecret,
         string grantType,
         string userName,
         string password,
         string scope,
         string protectedResource,
         string url,
         bool includeEncryptedTokenInResponse = false)
        {
            ProtectedResource = protectedResource;
            AccessTokenEndpoint = accessTokenEndpoint;
            ClientId = clientId;
            ClientSecret = clientSecret;
            GrantType = grantType;
            UserName = userName;
            Password = password;
            Scope = scope;
            Url = url;
            IncludeEncryptedTokenInResponse = includeEncryptedTokenInResponse;
        }

    }
}
