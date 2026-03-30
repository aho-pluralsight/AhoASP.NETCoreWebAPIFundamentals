namespace Ch06.Aho.CityInfo.API.Models
{
    /// <summary>
    /// City DTO
    /// </summary>
    public class CityDto
    {
        /// <summary>
        /// City Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// City Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// City Description
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Number of points Of interest for the city
        /// </summary>
        public int NumberOfPointsOfInterest { get { return PointsOfInterest.Count; } }
        /// <summary>
        /// Points Of interest of the city
        /// </summary>
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}
