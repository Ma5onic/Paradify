using web.Enums;

namespace web.Repositories
{
    public interface IHistoryRepository
    {
        int AddSearchHistory(string query, string track, string userId, AppSource appsource);
    }
}