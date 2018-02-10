namespace web
{
    public class Helper
    {
        public static string SetSearchReturnUrl(string controllerName, string searhQuery)
        {
            return "~/" + controllerName + "?q=" + searhQuery;
        }
    }
}