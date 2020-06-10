using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace API.Dtos
{
    public class ReactiveDonationDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PetFree { get; set; }

        [Required] [SmokeFreeYesNo]
        public string SmokeFree { get; set; }

        [Required] [PetFreeYesNo]
        public string Items { get; set; }

        [Required] [AtLeastOnePicPerItem]
        public IFormFile[] Pictures { get; set; }

    }
}