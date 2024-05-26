using Ch03.Aho.CityInfo.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Ch03.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(ILogger<CitiesController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Instance.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            try
            {
                if (id == 0)
                {
                    throw new ArgumentException("This is nasty yo!");
                }

                var city = CitiesDataStore.Instance.Cities.FirstOrDefault(city => city.Id == id);

                if (city == null)
                {
                    return NotFound();
                }

                return Ok(city);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Bad exception occured while handling a call to get the city with the id {id}!", ex);
                return StatusCode(500, $"Something nasty occurred on the server side!");
            }
        }
    }
}
