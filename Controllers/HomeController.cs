using Microsoft.AspNetCore.Mvc;
using rstracker.Core;

namespace rstracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SetCookieConsent()
    {
        CookieHandler.SetCookie(Response, Enums.CookieKeys.COOKIE_CONSENT, "true");
        return Ok();
    }

}
