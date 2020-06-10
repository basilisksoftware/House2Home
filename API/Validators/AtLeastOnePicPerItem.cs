
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using API.Dtos;
using API.Models;
using Newtonsoft.Json;

class AtLeastOnePicPerItem : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var DonationDto = (ReactiveDonationDto)validationContext.ObjectInstance;

        // Get all the items
        List<Item> items = JsonConvert.DeserializeObject<List<Item>>(DonationDto.Items);

        // Initialse a list of strings
        List<string> fileNames = new List<string>();

        // Get all the file names and append to above list
        for (int i = 0; i < DonationDto.Pictures.Length; i++)
            fileNames.Add(DonationDto.Pictures[i].FileName);

        // Loop each item and check at least one of the filenames in the 
        // fileNames list matches the item's description
        foreach (Item item in items)
            if (!fileNames.Contains(item.Description))
                return new ValidationResult("Each item must have at least one picture");

        // Success
        return ValidationResult.Success;
    }
}