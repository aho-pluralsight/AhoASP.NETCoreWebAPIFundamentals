using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Ch03.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>
            {
                new { Id = 1, Name = "New York City" },
                new { Id = 2, Name = "Lyon" }
            });
        }
    }
}
