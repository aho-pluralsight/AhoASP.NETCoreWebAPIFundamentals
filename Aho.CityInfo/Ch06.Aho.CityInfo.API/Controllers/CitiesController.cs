using AutoMapper;
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
        private readonly IMapper _mapper;

        public CitiesController(ILogger<CitiesController> logger, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities()
        {
            var cityEntities = await _cityInfoRepository.GetCitiesAsync();
            //var results = new List<CityWithoutPointsOfInterestDto>();
            //foreach (var city in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto()
            //    {
            //        Description = city.Description,
            //        Name = city.Name,
            //        Id = city.Id
            //    });
            //}
            //[AHO] Mapping the Entity to the DTO using Auto Mapper
            var results = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCity(int id, bool includePointsOfInterest)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }

            return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
        }
    }
}
