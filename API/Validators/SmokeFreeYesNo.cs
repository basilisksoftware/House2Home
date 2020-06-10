
using System.ComponentModel.DataAnnotations;
using API.Dtos;

class SmokeFreeYesNo : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var DonationDto = (ReactiveDonationDto)validationContext.ObjectInstance;

        if (DonationDto.SmokeFree != "Yes" && DonationDto.SmokeFree != "No")
            return new ValidationResult("Invalid input for Smoke-free. Must be Yes or No");

        return ValidationResult.Success;

    }
}