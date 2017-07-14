using System;
using System.Linq;
using SpotifyAPI.Web.Models;
using SpotifyAPI.Web;

namespace web.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;

        public UserService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public int AddUser(PrivateProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.Id))
                return 0;
            try
            {
                Repositories.Context context = new Repositories.Context();

                var image = profile.Images.FirstOrDefault();

                int exist = context.DbContext.Sql("Select 1 from [User] Where UserId = '" + profile.Id + "'")
                    .QuerySingle<int>();

                if (exist > 0)
                {
                    return 0;
                }

                return context.DbContext.Insert("[User]")
                    .Column("Birthdate", profile.Birthdate)
                    .Column("Country", profile.Country)
                    .Column("DisplayName", profile.DisplayName)
                    .Column("Email", profile.Email)
                    .Column("Href", profile.Href)
                    .Column("UserId", profile.Id)
                    .Column("Images", (image != null) ? image.Url : null)
                    .Column("Product", profile.Product)
                    .Column("Type", profile.Type)
                    .Column("Uri", profile.Uri)
                    .ExecuteReturnLastId<int>("Id");

            }
            catch (Exception ex)
            {

            }

            return 0;
        }

        public PrivateProfile GetMe(Token token)
        {
            SpotifyWebAPI api = new SpotifyWebAPI() { AccessToken = token.AccessToken, TokenType = token.TokenType };

            return api.GetPrivateProfile();
        }
    }
}