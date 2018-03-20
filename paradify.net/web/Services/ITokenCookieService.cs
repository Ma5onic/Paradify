using web.Models;
using static web.Models.CustomToken;

namespace web.Services
{
    public interface ITokenCookieService
    {
        void SetToken(string accessToken, string refreshToken, int expiresIn, TokenCredentialType tokenCredentialType);
        void SetToken(CustomToken token);
        void DeleteToken();
        CustomToken Get();
        bool Signout();
        CustomToken RefreshToken(string refreshToken, string clientSecret);
    }
}