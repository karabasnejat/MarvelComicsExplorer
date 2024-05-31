using ComicApp.Models;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using ComicApp.Models;

namespace ComicApp.Services
{
    public class MarvelService
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly HttpClient _httpClient;

        public MarvelService(IConfiguration configuration)
        {
            _publicKey = configuration["Marvel:PublicKey"];
            _privateKey = configuration["Marvel:PrivateKey"];
            _httpClient = new HttpClient { BaseAddress = new Uri("https://gateway.marvel.com/") };
        }

        public async Task<Character> GetCharacterAsync(string characterName)
        {
            var ts = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var hash = CreateHash(ts);
            var response = await _httpClient.GetAsync($"v1/public/characters?name={characterName}&ts={ts}&apikey={_publicKey}&hash={hash}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var characterDataArray = jsonDocument.RootElement.GetProperty("data").GetProperty("results");

            if (characterDataArray.GetArrayLength() == 0)
            {
                return null;
            }

            var characterData = characterDataArray[0];
            var comics = new List<Comic>();

            foreach (var comic in characterData.GetProperty("comics").GetProperty("items").EnumerateArray())
            {
                var comicUrl = comic.GetProperty("resourceURI").GetString();
                var comicId = comicUrl.Split('/').Last(); // Get comic ID from resourceURI

                // Fetch comic details to get the thumbnail
                var comicResponse = await _httpClient.GetAsync($"v1/public/comics/{comicId}?ts={ts}&apikey={_publicKey}&hash={hash}");
                if (comicResponse.IsSuccessStatusCode)
                {
                    var comicContent = await comicResponse.Content.ReadAsStringAsync();
                    var comicJson = JsonDocument.Parse(comicContent);
                    var comicData = comicJson.RootElement.GetProperty("data").GetProperty("results")[0];

                    comics.Add(new Comic
                    {
                        ComicId = comicId,
                        Title = comicData.GetProperty("title").GetString(),
                        Thumbnail = $"{comicData.GetProperty("thumbnail").GetProperty("path").GetString()}.{comicData.GetProperty("thumbnail").GetProperty("extension").GetString()}"
                    });
                }
            }

            var character = new Character
            {
                Id = characterData.GetProperty("id").GetInt32(),
                Name = characterData.GetProperty("name").GetString(),
                Description = characterData.GetProperty("description").GetString(),
                Thumbnail = $"{characterData.GetProperty("thumbnail").GetProperty("path").GetString()}.{characterData.GetProperty("thumbnail").GetProperty("extension").GetString()}",
                Comics = comics
            };

            return character;
        }

        public async Task<ComicDetail> GetComicDetailAsync(string comicId)
        {
            var ts = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var hash = CreateHash(ts);
            var response = await _httpClient.GetAsync($"v1/public/comics/{comicId}?ts={ts}&apikey={_publicKey}&hash={hash}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var comicData = jsonDocument.RootElement.GetProperty("data").GetProperty("results")[0];

            var creators = comicData.GetProperty("creators").GetProperty("items").EnumerateArray()
                                   .Select(c => c.GetProperty("name").GetString())
                                   .ToList();

            var comicDetail = new ComicDetail
            {
                Title = comicData.GetProperty("title").GetString(),
                Description = comicData.GetProperty("description").GetString(),
                Thumbnail = $"{comicData.GetProperty("thumbnail").GetProperty("path").GetString()}.{comicData.GetProperty("thumbnail").GetProperty("extension").GetString()}",
                Creators = string.Join(", ", creators),
                PublishedDate = comicData.GetProperty("dates").EnumerateArray()
                                         .First(d => d.GetProperty("type").GetString() == "onsaleDate")
                                         .GetProperty("date").GetString()
            };

            return comicDetail;
        }

        public async Task<IEnumerable<string>> GetCharacterNamesAsync(string query)
        {
            var ts = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var hash = CreateHash(ts);
            var response = await _httpClient.GetAsync($"v1/public/characters?nameStartsWith={query}&ts={ts}&apikey={_publicKey}&hash={hash}");

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<string>();

            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            var characterDataArray = jsonDocument.RootElement.GetProperty("data").GetProperty("results");

            var characterNames = characterDataArray.EnumerateArray()
                                                   .Select(c => c.GetProperty("name").GetString())
                                                   .ToList();

            return characterNames;
        }

        private string CreateHash(long timestamp)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes($"{timestamp}{_privateKey}{_publicKey}");
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
