using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediaWatcher.Models;
using MediaWatcher.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MediaWatcher.Controllers;


[Route("/")]
public class HomeController : Controller
{
    private readonly FileIndexService _fileIndexService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, FileIndexService fileIndexService)
    {
        _logger = logger;
         _fileIndexService = fileIndexService;
    }

    [Route("/")]
    [Route("[action]")]
    public IActionResult Index()
    {
        var categories = _fileIndexService.GetCategories();
        return View(categories);
    }

    [Route("[action]")]
    public IActionResult Watch([FromQuery]string video)
    { 
        var query = QueryString.Create("video", video);
        var videoSrc = new Uri($"{Data.address}/Media/stream?video={video}");
        List<string> subs = new(video.Split("/"));
        var title = subs[subs.Count - 1].Replace(".mp4", "");

        subs.RemoveAt(subs.Count - 1);

        var path = Path.Combine(subs.ToArray());

        var otherVideos = _fileIndexService.GetFiles(path);

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
        var contain = _fileIndexService.IndexFiles(position);
        return View(contain);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("[action]")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
