using Ch06.Aho.CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ch06.Aho.CityInfo.API.DbContexts
{
    public class AhoCityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public AhoCityInfoContext(DbContextOptions<AhoCityInfoContext> options) : base(options)
        {
        }
    }
}
