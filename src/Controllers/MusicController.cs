using System.Diagnostics;
using MediaWatcher.Models;
using MediaWatcher.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediaWatcher.Controllers;


[Route("[controller]")]
public class MusicController : Controller
{
    private readonly MusicIndexService _musicFileIndexService;
    private readonly ILogger<MusicController> _logger;

    public MusicController(ILogger<MusicController> logger, MusicIndexService musicFileIndexService)
    {
        _logger = logger;
        _musicFileIndexService = musicFileIndexService;
    }

    [Route("[controller]")]
    [Route("[action]")]
    public IActionResult Index()
    {
        Console.WriteLine();
        var contain = _musicFileIndexService.IndexFiles();
        return View(contain);
    }

    [Route("[action]")]
    public IActionResult Listen([FromQuery]string music)
    {
        
        var query = QueryString.Create("song", music);
        var audioSrc = new Uri($"http://{Request.Host.Value}/Media/musicstream{query}");
        List<string> subs = new(music.Split("/"));
        var title = subs[subs.Count - 1].Replace(".mp3", "");

        subs.RemoveAt(subs.Count - 1);

        var path = Path.Combine(subs.ToArray());

        var otherMusics = _musicFileIndexService.GetFiles(path);

        Dictionary<string, object> model = new()
        {
            { nameof(audioSrc), audioSrc },
            { nameof(title), title },
            { nameof(otherMusics), otherMusics}
        };
        return View(model: model);
    }

    [Route("[action]")]
    public IActionResult Browse([FromQuery]string position)
    {
        var contain = _musicFileIndexService.IndexFiles(position);
        return View(contain);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Route("[action]")]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
