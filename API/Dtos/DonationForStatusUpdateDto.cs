using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class DonationForStatusUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string StatusToSet { get; set; }

        [Required]
        public DateTime CollectionDate { get; set; }
    }
}