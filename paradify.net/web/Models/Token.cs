namespace web.Models
{
    public class Token
    {
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; } 
        public int ExpireTime { get; set; } 
    }
}