using MediaWatcher.Models;
using MediaWatcher.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace MediaWatcher.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {

        [HttpGet("stream")]
        public ActionResult StreamVideo(string video)
        {
            var videoPath = Path.Combine(FileIndexService.LibraryPath, video);

            if (!System.IO.File.Exists(videoPath))
            {
                return NotFound();
            }

            var info = new FileInfo(videoPath);


            var stream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var response = new FileStreamResult(stream, MediaTypeHeaderValue.Parse($"video/*")){
                EnableRangeProcessing = true,
            };

            return response;
        }
    }
}
