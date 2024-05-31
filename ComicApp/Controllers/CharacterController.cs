using ComicApp.Services;
using ComicApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicApp.Controllers
{
    public class CharacterController : Controller
    {
        private readonly MarvelService _marvelService;

        public CharacterController(MarvelService marvelService)
        {
            _marvelService = marvelService;
        }

        public IActionResult Index()
        {
            var characterName = TempData["LastSearchedCharacter"] as string;
            TempData.Keep("LastSearchedCharacter");
            if (characterName != null)
            {
                ViewBag.LastSearchedCharacter = characterName;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string characterName)
        {
            TempData["LastSearchedCharacter"] = characterName;
            var character = await _marvelService.GetCharacterAsync(characterName);
            if (character == null)
            {
                ViewBag.ErrorMessage = "Character not found. Please try another name.";
                return View("Index");
            }

            return View("Index", character);
        }

        [HttpGet]
        public async Task<IActionResult> GetCharacterNames(string query)
        {
            var characterNames = await _marvelService.GetCharacterNamesAsync(query);
            return Json(characterNames);
        }
    }
}
