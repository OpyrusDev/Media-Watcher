using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MediaWatcher.Models;
using MediaWatcher.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.InteropServices;

namespace MediaWatcher.Controllers;


[Route("/")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("/")]
    [Route("[action]")]
    public IActionResult Index()
    {
        return View();
    }
}
