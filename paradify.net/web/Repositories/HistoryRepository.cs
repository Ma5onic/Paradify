using web.Enums;

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
    }
}