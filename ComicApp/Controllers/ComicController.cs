using ComicApp.Services;
using ComicApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ComicApp.Controllers
{
    public class ComicController : Controller
    {
        private readonly MarvelService _marvelService;

        public ComicController(MarvelService marvelService)
        {
            _marvelService = marvelService;
        }

        public async Task<IActionResult> Detail(string comicId)
        {
            var comicDetail = await _marvelService.GetComicDetailAsync(comicId);
            if (comicDetail == null)
            {
                return NotFound();
            }

            ViewBag.LastSearchedCharacter = TempData["LastSearchedCharacter"];
            return View(comicDetail);
        }
    }
}
