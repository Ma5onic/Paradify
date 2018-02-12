using SpotifyAPI.Web.Models;

namespace web
{
    public static class Extensions
    {
        public static bool NullCheck<T>(this T obj)
        {
            
            return obj == null || (obj.GetType() == typeof(string) && string.IsNullOrEmpty(obj as string));
        }

        public static bool NullOrEmptyCheck(this PrivateProfile profile)
        {
            return (profile == null || string.IsNullOrEmpty(profile.Id));
        }

        public static bool IsNotAuthorized(this BasicModel model)
        {
            return (model != null && model.Error != null && model.Error.Status == 401);
        }
    }
}