using Ch06.Aho.CityInfo.API.Models;
using Ch06.Aho.CityInfo.API.Services.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Ch06.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ILogger<CitiesController> logger, ICityInfoRepository cityInfoRepository)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            var results = new List<CityWithoutPointsOfInterestDto>();
            foreach (var city in cityEntities)
            {
                results.Add(new CityWithoutPointsOfInterestDto()
                {
                    Description = city.Description,
                    Name = city.Name,
                    Id = city.Id
                });
            }
            return Ok(results);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            //try
            //{
            //    if (id == 0)
            //    {
            //        throw new ArgumentException("This is nasty yo!");
            //    }

            //    var city = _citiesDataStore.Cities.FirstOrDefault(city => city.Id == id);

            //    if (city == null)
            //    {
            //        return NotFound();
            //    }

            //    return Ok(city);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogCritical($"Bad exception occured while handling a call to get the city with the id {id}!", ex);
            //    return StatusCode(500, $"Something nasty occurred on the server side!");
            //}
            return Ok();
        }
    }
}
