using AutoMapper;
using Ch06.Aho.CityInfo.API.Entities;
using Ch06.Aho.CityInfo.API.Models;
using Ch06.Aho.CityInfo.API.Services;
using Ch06.Aho.CityInfo.API.Services.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
 
namespace Ch06.Aho.CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private const string MethodGetPointOfInterest = "GetPointOfInterest";
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly SimpleNotificationService _notificationServiceSimple;
        private readonly INotificationService _notificationServiceFancy;
        private readonly INotificationService _notificationServiceConfig;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            SimpleNotificationService notificationServiceSimple,
            [FromKeyedServices("notifFancy")] INotificationService notificationServiceFancy,
            [FromKeyedServices("notifConfig")] INotificationService notificationServiceConfig,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _notificationServiceSimple = notificationServiceSimple ?? throw new ArgumentNullException(nameof(notificationServiceSimple));
            _notificationServiceFancy = notificationServiceFancy ?? throw new ArgumentNullException(nameof(notificationServiceFancy));
            _notificationServiceConfig = notificationServiceConfig ?? throw new ArgumentNullException(nameof(notificationServiceConfig));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"The city with the id {cityId} was not found when trying to access the points of interest.");
                return NotFound();
            }

            var pointsOfInterestForCity = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{pointId}", Name = MethodGetPointOfInterest)]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterest>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreateDto pointOfInterestForCreation)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestForCreation);

            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, pointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            var pointOfInterestNewlyCreated = _mapper.Map<PointOfInterestDto>(pointOfInterest);

            _notificationServiceSimple.Notify("Point of interest created.", $"Point of interest '{pointOfInterest.Name}' with the id '{pointOfInterest.Id}' is created.");

            return CreatedAtRoute(MethodGetPointOfInterest,
                new
                {
                    cityId = pointOfInterest.CityId,
                    pointId = pointOfInterest.Id
                }, pointOfInterestNewlyCreated);
        }

        [HttpPut("{pointId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointId, PointOfInterestForUpdateDto updatePointOfInterest)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointId);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            //[AHO] Will map and update pointOfInterest object
            _mapper.Map(updatePointOfInterest, pointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            _notificationServiceFancy.Notify("Point of interest created.", $"Point of interest '{pointOfInterest.Name}' with the id '{pointOfInterest.Id}' was updated.");

            return NoContent();
        }

        [HttpPatch("{pointId}")]
        public async Task<ActionResult> PartialUpdatePointOfInterest(int cityId, int pointId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestPatch = _mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);
            
            patchDocument.ApplyTo(pointOfInterestPatch, ModelState);
            
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(pointOfInterestPatch);
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            
            _notificationServiceConfig.Notify("Point of interest patched.", $"Point of interest '{pointOfInterestEntity.Name}' with the id '{pointOfInterestEntity.Id}' was successfully patched.");

            return NoContent();
        }

        [HttpDelete("{pointId}")]
        public async Task<ActionResult> DeletePointOfInterest(int cityId, int pointId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            _notificationServiceConfig.Notify(
                "Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with the ID {pointOfInterestEntity.Id} was successfully deleted.");

            return NoContent();
        }
    }
}
