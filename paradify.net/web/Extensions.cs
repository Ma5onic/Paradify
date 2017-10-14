using SpotifyAPI.Web.Models;

namespace web
{
    public static class Extensions
    {
        public static bool NullCheck<T>(this T obj)
        {
            return obj == null;
        }

        public static bool NullOrEmptyCheck(this PrivateProfile profile)
        {
            return (profile == null || string.IsNullOrEmpty(profile.Id));
        }
    }
}