using Microsoft.Extensions.Options;
using NativoPlusStudio.AuthToken.Core;
using NativoPlusStudio.AuthToken.Ficoso.DTOs;
using NativoPlusStudio.AuthToken.Core.Interfaces;
using NativoPlusStudio.Encryption.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NativoPlusStudio.AuthToken.Core.Extensions;
using NativoPlusStudio.AuthToken.Ficoso.Configurations;
using NativoPlusStudio.AuthToken.DTOs;

namespace NativoPlusStudio.AuthToken.Ficoso
{
    public class FicosoAuthTokenProvider : BaseTokenProvider<FicosoAuthTokenOptions>, IAuthTokenProvider
    {
        private readonly HttpClient _client;
        public FicosoAuthTokenProvider(
            HttpClient client,
            IOptions<FicosoAuthTokenOptions> ficosoOptions,
            IEncryption symmetricEncryption = null,
            IAuthTokenCacheService tokenCacheService = null,
            ILogger logger = null
        )
            : base(symmetricEncryption, tokenCacheService, logger, ficosoOptions)
        {
            _client = client;
        }

        public async Task<ITokenResponse> GetTokenAsync()
        {
            _logger.Information("FicosoAuthTokenProvider GetTokenAsync start");
            try
            {
                if (_tokenCacheService != null)
                {
                    var cachedToken = _tokenCacheService.GetCachedAuthToken(_options.ProtectedResource);

                    if (!cachedToken?.IsExpired ?? false)
                    {
                        return GetTokenFromCache(cachedToken);
                    }
                }

                var (Response, Status, Code, Message) = await 
                    (await _client.PostFormAsync(BuildFormUrlEncodedContent(), _options.AccessTokenEndpoint))
                    .TransformHttpResponseToType<FicosoTokenResponse>()
                    ;

                var tokenResponse = new TokenResponse
                {
                    Token = Response.AccessToken,
                    TokenType = Response.TokenType,
                    EncryptedToken = _options.IncludeEncryptedTokenInResponse && _symmetricEncryption != null 
                        ? _symmetricEncryption.Encrypt(Response.AccessToken) 
                        : null,
                    ExpiryDateUtc = Response.AccessToken
                        .BuildJwtSecurityToken()
                        .GetClaimFromJwtToken("exp")
                        .GetExpirationDateInUtcFromJwsTokenClaim()
                };

                if (!Status)
                {
                    _logger.Error("#GetToken HTTP POST to FCS Authentication unsuccessful.");
                }
                else
                {
                    if (_tokenCacheService != null)
                    {
                        TokenCacheUpsert(_options.ProtectedResource, tokenResponse);
                    }

                }
                _logger.Information($"Encrypted Token: {tokenResponse.EncryptedToken}. Included EncryptedToken InResponse: {_options.IncludeEncryptedTokenInResponse}");
                return tokenResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "#GetToken: Caught exception {Exception}");
                return new TokenResponse()
                {
                };
            }
        }

        private List<KeyValuePair<string, string>> BuildFormUrlEncodedContent()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("grant_type", _options.GrantType),
                new KeyValuePair<string, string>("username", _options.UserName),
                new KeyValuePair<string, string>("password", _options.Password),
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                new KeyValuePair<string, string>("scope", _options.Scope)
            };
        }

        private void TokenCacheUpsert(string protectedResource, ITokenResponse tokenResponse)
        {
            _logger.Information("FicosoAuthTokenProvider TokenCacheUpsert start");

            string tokenTobeStored;
            if (tokenResponse.EncryptedToken != null)
            {
                tokenTobeStored = tokenResponse.EncryptedToken;
            }
            else
            {
                tokenTobeStored = _symmetricEncryption != null 
                    ? _symmetricEncryption.Encrypt(tokenResponse.Token) 
                    : tokenResponse.Token;
            }

            var (upsertResult, errorMessage) = _tokenCacheService.UpsertAuthTokenCache(
                    protectedResource.ToString(),
                    tokenTobeStored,
                    tokenResponse.TokenType,
                    tokenResponse.ExpiryDateUtc
                );

            if (!string.IsNullOrEmpty(errorMessage))
            {
                _logger.Error($"#GetToken {errorMessage}");
            }
        }

        private ITokenResponse GetTokenFromCache(IAuthTokenDetails cachedToken)
        {
            _logger.Information("FicosoAuthTokenProvider GetTokenFromCache start");

            var decryptedToken = _symmetricEncryption != null ? _symmetricEncryption.Decrypt(cachedToken.Token) : cachedToken.Token;
            return new TokenResponse()
            {
                Token = decryptedToken,
                TokenType = cachedToken.TokenType,
                EncryptedToken = decryptedToken != cachedToken.Token ? cachedToken.Token : null,
                ExpiryDateUtc = cachedToken.ExpirationDate
                
            };
        }
    }
}
