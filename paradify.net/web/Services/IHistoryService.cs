using web.Enums;

namespace web.Services
{
    public interface IHistoryService
    {
        int AddSearchHistory(string quer, string track, string userId, AppSource appsource);
    }
}