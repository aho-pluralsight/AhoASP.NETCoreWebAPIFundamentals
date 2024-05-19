using Ch03.Aho.CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ch03.Aho.CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private const string MethodGetPointOfInterest = "GetPointOfInterest";

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{pointId}", Name = MethodGetPointOfInterest)]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(point => point.Id == pointId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreateDto pointOfInterestForCreation)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault<CityDto>(ci => ci.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfIntId = CitiesDataStore.Instance.Cities.SelectMany(ci => ci.PointsOfInterest).Max(pi => pi.Id);
            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfIntId,
                Name = pointOfInterestForCreation.Name,
                Description = pointOfInterestForCreation.Description
            };
            city.PointsOfInterest.Add(newPointOfInterest);

            return CreatedAtRoute(MethodGetPointOfInterest, new { cityId = city.Id, pointId = newPointOfInterest.Id }, newPointOfInterest);
        }
    }
}
