namespace Ch06.Aho.CityInfo.API.Models
{
    /// <summary>
    /// Point Of Interest Dto
    /// </summary>
    public class PointOfInterestDto
    {
        /// <summary>
        /// POI Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// POI Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// POI Description
        /// </summary>
        public string? Description { get; set; }
    }
}
