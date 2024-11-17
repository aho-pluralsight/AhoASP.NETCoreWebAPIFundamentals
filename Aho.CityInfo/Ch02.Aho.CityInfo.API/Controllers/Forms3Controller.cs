using Microsoft.AspNetCore.Mvc;

namespace Ch02.Aho.CityInfo.API.Controllers
{

    [Route("/[controller]")]
    public class Forms3Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("submit-first")]
        //public async Task<IActionResult> SubmitFirstFormAsync(object obj)
        public IActionResult SubmitFirst(IFormCollection form)
        {
            //var test = await string.Empty;
            return View();
        }
    }
}
