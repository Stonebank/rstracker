using Microsoft.AspNetCore.Mvc;
using rstracker.Core;

namespace rstracker.Controllers
{
    public class CookieController : Controller
    {

        [HttpPost]
        public IActionResult SetCookieConsent()
        {
            CookieHandler.SetCookie(Response, Enums.CookieKeys.COOKIE_CONSENT, "true");
            return Ok();
        }

    }
}
