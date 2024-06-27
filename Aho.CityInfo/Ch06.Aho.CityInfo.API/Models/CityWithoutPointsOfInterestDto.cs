namespace Ch06.Aho.CityInfo.API.Models
{
    /// <summary>
    /// City without points of interest DTO
    /// </summary>
    public class CityWithoutPointsOfInterestDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }
    }
}
