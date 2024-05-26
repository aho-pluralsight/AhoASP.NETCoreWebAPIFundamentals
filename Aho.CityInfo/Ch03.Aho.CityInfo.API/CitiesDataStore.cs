using Ch03.Aho.CityInfo.API.Models;

namespace Ch03.Aho.CityInfo.API
{
    public class CitiesDataStore
    {
        //public static CitiesDataStore Instance { get; set; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "Big Apple!",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name="Central Park",
                            Description="The most visited urban park in the United States!"
                        },
                        new PointOfInterestDto()
                        {
                            Id=2,
                            Name="Empire State Building",
                            Description="An iconic 102-story skyscraper in Manhattan!"
                        }
                    }
                },new CityDto()
                {
                    Id = 2,
                    Name = "Lyon",
                    Description = "The capital of French gastronomy!",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=3,
                            Name="Basilica Notre-Dame de Fourvière",
                            Description="A beautiful cathedral (late 19th century) that overlooks the city of Lyon from the Fourvière Hill."
                        },
                        new PointOfInterestDto()
                        {
                            Id=4,
                            Name="Confluences Museum",
                            Description="A museum of natural history, anthropology, societies, and civilizations, housed in a modern-designed building."
                        }
                    }
                },new CityDto()
                {
                    Id = 3,
                    Name = "Jijel",
                    Description = "The place of birth of AHO!",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=5,
                            Name="Taza National Park",
                            Description="Known for its diverse landscapes, including forests, mountains, and beaches."
                        },
                        new PointOfInterestDto()
                        {
                            Id=6,
                            Name="Phare de Ras Afia",
                            Description="An iconic landmark along the shores of Jijel."
                        }
                    }
                }
            };
        }
    }
}
