using System;

namespace web.Models
{
    public class History
    {
        public string Query { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}