
namespace NativoPlusStudio.AuthToken.Ficoso.Interfaces
{
    public interface IFicosoTokenResponse
    {
        string AccessToken { get; set; }
        string ExpiresIn { get; set; }
        string TokenType { get; set; }
    }
}
