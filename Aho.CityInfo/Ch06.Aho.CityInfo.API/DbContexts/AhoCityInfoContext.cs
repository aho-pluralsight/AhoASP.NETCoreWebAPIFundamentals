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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "Big Apple!"
                },
                new City("Lyon")
                {
                    Id = 2,
                    Description = "The capital of French gastronomy!"
                },
                new City("Jijel")
                {
                    Id = 3,
                    Description = "The place of birth of AHO!"
                });

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "The most visited urban park in the United States!"
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "An iconic 102-story skyscraper in Manhattan!"
                },
                new PointOfInterest("Basilica Notre-Dame de Fourvière")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "A beautiful cathedral (late 19th century) that overlooks the city of Lyon from the Fourvière Hill."
                },
                new PointOfInterest("Confluences Museum")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "A museum of natural history, anthropology, societies, and civilizations, housed in a modern-designed building."
                },
                new PointOfInterest("Taza National Park")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "Known for its diverse landscapes, including forests, mountains, and beaches."
                },
                new PointOfInterest("Phare de Ras Afia")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "An iconic landmark along the shores of Jijel."
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
