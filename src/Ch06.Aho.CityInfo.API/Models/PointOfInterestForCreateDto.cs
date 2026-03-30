using System.ComponentModel.DataAnnotations;

namespace Ch06.Aho.CityInfo.API.Models
{
    /// <summary>
    /// Point of interest for creation DTO
    /// </summary>
    public class PointOfInterestForCreateDto
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required(ErrorMessage = "The property 'Name' is required!")]
        [MaxLength(50, ErrorMessage = "The 'Name' field must not exceed 50 characters!")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Description
        /// </summary>
        [MaxLength(200, ErrorMessage = "The 'Name' field must not exceed 200 characters!")]
        public string? Description { get; set; }
    }
}
