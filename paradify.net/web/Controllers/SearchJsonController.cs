using System.Web.Mvc;
using SpotifyAPI.Web.Models;
using web.Enums;
using web.Repositories;
using web.Services;
using web.Services.Implementations;

namespace web.Controllers
{
    public class SearchJsonController : Controller
    {
        private readonly IHistoryRepository _historyRepository;

        public SearchJsonController(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public ActionResult Index(string q)
        {
            IParadifyService paradifyService = new ParadifyService();

            SearchItem result = paradifyService.SearchResult(q, 50);

            HistoryService historyService = new HistoryService(_historyRepository);

            historyService.AddSearchHistory(q, null, null, AppSource.Extension);

            return View(result.Tracks);
        }
    }
}