using System.Configuration;

namespace web
{
    public class Constants
    {
        public static string ClientSecret { get; set; }
        public static string ClientId { get; set; }
        public static string RedirectUri { get; set; }
        public static string StateKey { get; set; }
        public static string Scope { get; set; }
        public static string Domain { get; set; }
        public static string FullTitle = "Paradify - Spotify Discovery - Songs to Spotify";

        public static string SingleTitle = "Paradify";


        static Constants()
        {
            Domain = ConfigurationManager.AppSettings["domain"];
            ClientSecret = ConfigurationManager.AppSettings["clientSecret"];
            ClientId = ConfigurationManager.AppSettings["clientId"];
            RedirectUri = ConfigurationManager.AppSettings["redirectUri"];
            StateKey = ConfigurationManager.AppSettings["stateKey"];
            Scope = ConfigurationManager.AppSettings["scope"];
        }
    }
}