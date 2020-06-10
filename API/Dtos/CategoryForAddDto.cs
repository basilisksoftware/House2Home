using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CategoryForAddDto
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public bool VerifyFireLabels { get; set; }
    }
}