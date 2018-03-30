using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;
using web.Models;

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
        private static List<Country> _countryCodes;
        public static List<Country> CountryCodes { get { return GetCountryCodes(); } }
        public static string DefaultCountryCode { get { return "US"; } }
        private static List<Country> GetCountryCodes()
        {
            if (_countryCodes == null)
            {
                var approotpath = HostingEnvironment.ApplicationPhysicalPath;
                string codes = File.ReadAllText(Path.Combine(approotpath, @"src\static\countries.json"));
                _countryCodes = JsonConvert.DeserializeObject<List<Country>>(codes);
            }

            return _countryCodes;
        }

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