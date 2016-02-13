using System.Configuration;
using FluentData;

namespace web.Repositories
{
    public class Context
    {
        public IDbContext DbContext;
        public Context()
        {
            DbContext = new DbContext().ConnectionString(
                ConfigurationManager.ConnectionStrings["default"].ToString(), new SqlServerProvider());
        }
    }
}