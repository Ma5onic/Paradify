using System.Collections.Generic;
using web.Enums;
using web.Models;

namespace web.Repositories
{
    public interface IHistoryRepository
    {
        int AddSearchHistory(string query, string track, string userId, AppSource appsource);

        List<History> GetHistories(int max);
    }
}