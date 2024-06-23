namespace Ch06.Aho.CityInfo.API.Models
{
    public class CityWithoutPointsOfInterestDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
