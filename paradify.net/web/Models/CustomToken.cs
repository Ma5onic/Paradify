using SpotifyAPI.Web.Models;

namespace web.Models
{
    public class CustomToken : Token
    {
        public TokenCredentialType tokenCredentialType { get; set; }

        public enum TokenCredentialType
        {
            Auth = 1,
            Client = 2
        }
    }
}