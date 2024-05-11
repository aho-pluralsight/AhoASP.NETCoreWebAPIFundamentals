using Ch03.Aho.CityInfo.API.Models;

namespace Ch03.Aho.CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Instance { get; set; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        private CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "Big Apple!"
                },new CityDto()
                {
                    Id = 2,
                    Name = "Lyon",
                    Description = "The capital of French gastronomy!"
                },new CityDto()
                {
                    Id = 3,
                    Name = "Jijel",
                    Description = "The place of birth of AHO!"
                }
            };
        }
    }
}
