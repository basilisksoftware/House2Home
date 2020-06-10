using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{

    public class ItemHelper
    {
        private readonly IConfiguration _config;
        private Cloudinary _cloudinary;

        public ItemHelper(IConfiguration config)
        {

            var acc = new Account(
                config.GetSection("Cloudinary:CloudName").Value,
                config.GetSection("Cloudinary:ApiKey").Value,
                config.GetSection("Cloudinary:ApiSecret").Value
            );

            _cloudinary = new Cloudinary(acc);
            _config = config;
        }

        public async Task<string> UploadAndGetUri(Item item, IFormFile itemFile)
        {

            if (item.Description == null)
                return "null";

            ImageUploadResult result = await _cloudinary.UploadAsync(new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new FileDescription("x", itemFile.OpenReadStream())
            });

            return result.Uri.ToString();
        }
    }
}