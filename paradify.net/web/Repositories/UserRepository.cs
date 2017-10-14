using System.Linq;
using SpotifyAPI.Web.Models;

namespace web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private Context _context;
        public UserRepository()
        {
            _context = new Context();
        }

        public int AddUser(PrivateProfile profile)
        {
            var image = profile.Images.FirstOrDefault();

            return _context.DbContext.Insert("[User]")
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

        public bool IsUserExist(string profileId)
        {
            return _context.DbContext.Sql(
                string.Format("Select 1 from [User] Where UserId = '{0}'", profileId))
                   .QuerySingle<int>() > 0;
        }
    }
}