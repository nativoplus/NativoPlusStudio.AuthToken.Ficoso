using NativoPlusStudio.AuthToken.Ficoso.Interfaces;
using Newtonsoft.Json;

namespace NativoPlusStudio.AuthToken.Ficoso.DTOs
{
    public class FicosoTokenResponse : IFicosoTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

    }
}
