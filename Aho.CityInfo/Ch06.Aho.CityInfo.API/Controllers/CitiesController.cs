using AutoMapper;
using Ch06.Aho.CityInfo.API.Models;
using Ch06.Aho.CityInfo.API.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
//using Newtonsoft.Json;

namespace Ch06.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    [Authorize]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> _logger;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        private readonly int maxPageSize = 20;

        public CitiesController(ILogger<CitiesController> logger, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
            [FromQuery(Name = "name")] string? filter,
            string? search,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var cityPage = await _cityInfoRepository.GetCitiesAsync(filter, search, pageNumber, pageSize);
            //[AHO] Mapping the Entity to the DTO using Auto Mapper
            var cities = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityPage.CitiesList);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(cityPage.Metadata));
            return Ok(cities);
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
