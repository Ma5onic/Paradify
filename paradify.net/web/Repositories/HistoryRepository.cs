using System.Collections.Generic;
using web.Enums;
using web.Models;

namespace web.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        public int AddSearchHistory(string query, string track, string userId, AppSource appsource)
        {
            Context context = new Context();

            return context.DbContext.Insert("History")
                .Column("Query", query)
                .Column("TrackId", track)
                .Column("UserId", userId)
                .Column("source", appsource)
                .ExecuteReturnLastId<int>("Id");
        }

        public List<History> GetHistories(int max)
        {
            Context context = new Context();

            return context.DbContext.Sql("select top " + max + " Query, LEFT(UserId, 1) as UserId, CreatedDate from history order by CreatedDate desc")
                .QueryMany<History>();
        }
    }
}