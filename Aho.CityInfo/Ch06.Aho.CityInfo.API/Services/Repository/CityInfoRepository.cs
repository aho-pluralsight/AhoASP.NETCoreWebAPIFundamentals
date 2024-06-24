using Ch06.Aho.CityInfo.API.DbContexts;
using Ch06.Aho.CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ch06.Aho.CityInfo.API.Services.Repository
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly AhoCityInfoContext _cityInfoDbContext;

        public CityInfoRepository(AhoCityInfoContext dbContext)
        {
            _cityInfoDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _cityInfoDbContext.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? filter, string? searchQuery, int pageNumber, int pageSize)
        {
            //if (filter == null && searchQuery == null)
            //{
            //    return await GetCitiesAsync();
            //}

            var citiesCollection = _cityInfoDbContext.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                filter = filter.Trim();
                citiesCollection = citiesCollection.Where(c => c.Name.ToLower() == filter.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                citiesCollection = citiesCollection.
                    Where(c =>
                    c.Name.ToLower().Contains(searchQuery.ToLower()) ||
                    (c.Description != null && c.Description.ToLower().Contains(searchQuery.ToLower()))
                    );
            }

            return await citiesCollection
                .OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return await _cityInfoDbContext.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }
            return await _cityInfoDbContext.Cities.Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _cityInfoDbContext.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _cityInfoDbContext.PointsOfInterest.Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await _cityInfoDbContext.PointsOfInterest.Where(p => p.CityId == cityId && p.Id == pointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _cityInfoDbContext.PointsOfInterest.Remove(pointOfInterest);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _cityInfoDbContext.SaveChangesAsync() >= 0);
        }
    }
}
