using System.Collections.Generic;
using web.Enums;
using web.Models;

namespace web.Services
{
    public interface IHistoryService
    {
        int AddSearchHistory(string quer, string track, string userId, AppSource appsource);
        List<History> GetHistories(int max);
    }
}