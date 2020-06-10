
using System.ComponentModel.DataAnnotations;
using API.Dtos;

class PetFreeYesNo : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var DonationDto = (ReactiveDonationDto)validationContext.ObjectInstance;

        if (DonationDto.PetFree != "Yes" && DonationDto.PetFree != "No")
            return new ValidationResult("Invalid input for Pet-free. Must be Yes or No");

        return ValidationResult.Success;

    }
}