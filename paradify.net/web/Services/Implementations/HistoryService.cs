using System;
using web.Enums;
using web.Repositories;

namespace web.Services.Implementations
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public int AddSearchHistory(string quer, string track, string userId, AppSource appsource)
        {
            try
            {
                return _historyRepository.AddSearchHistory(quer, track, userId, appsource);
            }
            catch (Exception)
            {
                

            }

            return 0;
            
        }
    }
}