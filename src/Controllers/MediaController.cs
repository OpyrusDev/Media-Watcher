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

        [HttpGet("videostream")]
        public ActionResult StreamVideo(string video)
        {
            var videoPath = Path.Combine(VideoIndexService.VideoLibraryPath, video);

            if (!System.IO.File.Exists(videoPath))
            {
                return NotFound();
            }

            var info = new FileInfo(videoPath);


            var stream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var response = new FileStreamResult(stream, MediaTypeHeaderValue.Parse($"video/mp4")){
                EnableRangeProcessing = true,
            };
            

            return response;
        }

        [HttpGet("musicstream")]
        public ActionResult StreamMusic(string song)
        {
            var videoPath = Path.Combine(MusicIndexService.MusicLibraryPath, song);

            if (!System.IO.File.Exists(videoPath))
            {
                return NotFound();
            }

            var info = new FileInfo(videoPath);


            var stream = new FileStream(videoPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var response = new FileStreamResult(stream, MediaTypeHeaderValue.Parse($"music/mp3")){
                EnableRangeProcessing = true,
            };
            

            return response;
        }
    }
}
