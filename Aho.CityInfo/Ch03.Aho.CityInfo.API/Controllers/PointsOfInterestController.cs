using Ch03.Aho.CityInfo.API.Models;
using Ch03.Aho.CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Ch03.Aho.CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private const string MethodGetPointOfInterest = "GetPointOfInterest";
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly SimpleNotificationService _notificationService;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, SimpleNotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService ?? throw new ArgumentNullException();
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            if (cityId == 0)
            {
                throw new Exception("Simulated-nasty-not-handled exception!");
            }

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
                _logger.LogDebug($"The city with the id = {cityId} was not found when trying to get the point of interest with the id = {pointId}.");
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

            _notificationService.Notify("Point of interest created.", $"Point of interest '{newPointOfInterest.Name}' with the id '{newPointOfInterest.Id}' is created.");

            return CreatedAtRoute(MethodGetPointOfInterest, new { cityId = city.Id, pointId = newPointOfInterest.Id }, newPointOfInterest);
        }

        [HttpPut("{pointId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointId, PointOfInterestForUpdateDto updatePointOfInterest)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(ci => ci.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var oldPointOfInterest = city.PointsOfInterest.FirstOrDefault(pi => pi.Id == pointId);
            if (oldPointOfInterest == null)
            {
                return NotFound();
            }

            oldPointOfInterest.Name = updatePointOfInterest.Name;
            oldPointOfInterest.Description = updatePointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{pointId}")]
        public ActionResult PartialUpdatePointOfInterest(int cityId, int pointId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var storedPointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);
            if (storedPointOfInterest == null)
            {
                return NotFound();
            }

            var patchPointOfInterest = new PointOfInterestForUpdateDto()
            {
                Name = storedPointOfInterest.Name,
                Description = storedPointOfInterest.Description
            };
            patchDocument.ApplyTo(patchPointOfInterest, ModelState);
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(patchPointOfInterest);
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            storedPointOfInterest.Name = patchPointOfInterest.Name;
            storedPointOfInterest.Description = patchPointOfInterest.Description;

            return NoContent();
        }

        [HttpDelete("{pointId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Instance.Cities.FirstOrDefault(ci => ci.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var point = city.PointsOfInterest.FirstOrDefault(pi => pi.Id == pointId);
            if (point == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(point);

            return NoContent();
        }
    }
}
