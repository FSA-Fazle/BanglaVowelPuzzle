using Microsoft.AspNetCore.Mvc;

namespace BanglaVowelPuzzle.Controllers
{
    public class TtsController : Controller
    {
        private static readonly HttpClient _http = new();

        static TtsController()
        {
            _http.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        }

        [HttpGet]
        public async Task<IActionResult> Index(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return BadRequest();
            var url = $"https://translate.google.com/translate_tts?ie=UTF-8&q={Uri.EscapeDataString(text)}&tl=bn&client=tw-ob&ttsspeed=0.9";
            try
            {
                var bytes = await _http.GetByteArrayAsync(url);
                return File(bytes, "audio/mpeg");
            }
            catch
            {
                return NoContent();
            }
        }
    }
}
