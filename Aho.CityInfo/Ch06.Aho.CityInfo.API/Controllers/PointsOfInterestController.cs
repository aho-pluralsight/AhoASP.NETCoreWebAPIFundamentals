﻿using AutoMapper;
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

        /*
        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreateDto pointOfInterestForCreation)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault<CityDto>(ci => ci.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfIntId = _citiesDataStore.Cities.SelectMany(ci => ci.PointsOfInterest).Max(pi => pi.Id);
            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfIntId,
                Name = pointOfInterestForCreation.Name,
                Description = pointOfInterestForCreation.Description
            };
            city.PointsOfInterest.Add(newPointOfInterest);

            _notificationServiceSimple.Notify("Point of interest created.", $"Point of interest '{newPointOfInterest.Name}' with the id '{newPointOfInterest.Id}' is created.");

            return CreatedAtRoute(MethodGetPointOfInterest, new { cityId = city.Id, pointId = newPointOfInterest.Id }, newPointOfInterest);
        }

        [HttpPut("{pointId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointId, PointOfInterestForUpdateDto updatePointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(ci => ci.Id == cityId);
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

            _notificationServiceFancy.Notify("Point of interest created.", $"Point of interest '{oldPointOfInterest.Name}' with the id '{oldPointOfInterest.Id}' was updated.");

            return NoContent();
        }

        [HttpPatch("{pointId}")]
        public ActionResult PartialUpdatePointOfInterest(int cityId, int pointId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
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

            _notificationServiceConfig.Notify("Point of interest patched.", $"Point of interest '{storedPointOfInterest.Name}' with the id '{storedPointOfInterest.Id}' was successfully patched.");

            return NoContent();
        }

        [HttpDelete("{pointId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(ci => ci.Id == cityId);
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
        */
    }
}
