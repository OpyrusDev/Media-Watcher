using System.Diagnostics;
using MediaWatcher.Models;
using MediaWatcher.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaWatcher.Controllers;
[Route("[controller]")]
public class VideoController : Controller
{

    private readonly VideoIndexService _videoFileIndexService;
    private readonly ILogger<VideoController> _logger;

    public VideoController(ILogger<VideoController> logger, VideoIndexService videoFileIndexService)
    {
        _logger = logger;
        _videoFileIndexService = videoFileIndexService;
    }

    [Route("[controller]")]
    [Route("[action]")]
    public IActionResult Index()
    {
        Console.WriteLine();
        var categories = _videoFileIndexService.GetCategories();
        return View(categories);
    }

    [Route("[action]")]
    public IActionResult Watch([FromQuery]string video)
    {
        
        var query = QueryString.Create("video", video);
        var videoSrc = new Uri($"http://{Request.Host.Value}/Media/videostream{query}");
        List<string> subs = new(video.Split("/"));
        var title = subs[subs.Count - 1].Replace(".mp4", "");

        subs.RemoveAt(subs.Count - 1);

        var path = Path.Combine(subs.ToArray());

        var otherVideos = _videoFileIndexService.GetFiles(path);

        Dictionary<string, object> model = new()
        {
            { nameof(videoSrc), videoSrc },
            { nameof(title), title },
            { nameof(otherVideos), otherVideos}
        };
        return View(model: model);
    }

    [Route("[action]")]
    public IActionResult Browse([FromQuery]string position)
    {
        var contain = _videoFileIndexService.IndexFiles(position);
        return View(contain);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("[action]")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
