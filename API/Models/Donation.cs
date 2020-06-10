using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Donation
    {
        public int Id { get; set; }
        public DateTime SubmissionDate { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? CollectionDate { get; set; }
        public string SmokeFree { get; set; }
        public string PetFree { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
        public List<Item> Items { get; set; }
    }
}