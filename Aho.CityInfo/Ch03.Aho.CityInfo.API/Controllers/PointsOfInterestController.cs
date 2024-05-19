using Ch03.Aho.CityInfo.API.Models;
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
    }
}
