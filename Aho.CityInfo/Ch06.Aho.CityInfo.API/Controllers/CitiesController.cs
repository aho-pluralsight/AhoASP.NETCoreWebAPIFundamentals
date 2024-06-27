using Asp.Versioning;
using AutoMapper;
using Ch06.Aho.CityInfo.API.Models;
using Ch06.Aho.CityInfo.API.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
//using Newtonsoft.Json;

namespace Ch06.Aho.CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    [Route("api/v{version:apiVersion}/cities")]
    [Authorize]
    [ApiController]
    [ApiVersion(2.7)]
    [ApiVersion(3.0)]
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

        /// <summary>
        /// Returns a list of cities available in the API
        /// </summary>
        /// <param name="filter">City name to filter with</param>
        /// <param name="search">String to search through names and descriptions on cities</param>
        /// <param name="pageNumber">Number of the page to get</param>
        /// <param name="pageSize">The size of the page</param>
        /// <returns>List of cities</returns>
        /// <response code="200">Returns a list of cities</response>
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

        /// <summary>
        /// Get a city by its Id
        /// </summary>
        /// <param name="id">The city Id</param>
        /// <param name="includePointsOfInterest">Wether to include the city's points of interests or not</param>
        /// <returns>A city or nothing</returns>
        /// <response code="200">Returns the requested city</response>
        /// <response code="403">You don't have permission man</response>
        /// <response code="404">Pas aujourd'hui !</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CityDto>> GetCity(int id, bool includePointsOfInterest)
        {
            //var sub = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var givenname = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;

            if (givenname != "Mad")
            {
                return Forbid();
            }

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
