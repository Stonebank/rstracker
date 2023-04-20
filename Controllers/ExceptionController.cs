using Microsoft.AspNetCore.Mvc;
using rstracker.Core;
using rstracker.Models;
using rstracker.Utility;
using System.Diagnostics;

namespace rstracker.Controllers
{
    public class ExceptionController : Controller
    {

        [Route("/Error/{statusCode}")]
        public IActionResult StatusCodeError(int statusCode)
        {
            HttpContext.Response.StatusCode = statusCode;
            var model = new ErrorModel(
                Utils.GetErrorMessage(statusCode),
                statusCode, 
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            return View("Error", model);
        }

        public IActionResult LogError([FromBody] ErrorModel model)
        {
            try
            {
                ErrorLogHandler.SaveErrorLog(model);
                return Ok("Thanks for submitting the report. Incase we need more information, we will contact you!");
            } catch (Exception e)
            {
                return BadRequest($"An error has occured while logging the error. The message has been logged automatically: {e.Message}");
            }
        }

    }
}
