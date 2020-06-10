using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Helpers;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using static API.Helpers.EmailHelper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class DonationController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        public EmailHelper _emailHelper = new EmailHelper();

        public DonationController(DataContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetDonations()
        {
            var donations = await _context.Donations.Include(d => d.Items).ToListAsync();
            return Ok(donations);
        }

        [HttpGet("archive")]
        public async Task<IActionResult> GetArchive()
        {
            var donations = await _context.Donations.Include(d => d.Items).Where(dn => dn.Status == "Archived").ToListAsync();
            return Ok(donations);
        }

        [HttpGet("rejects")]
        public async Task<IActionResult> GetRejects()
        {
            var donations = await _context.Donations.Include(d => d.Items).Where(dn => dn.Status == "Rejected").ToListAsync();
            return Ok(donations);
        }

        [HttpGet("schedule")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSchedule()
        {
            var donations = await _context.Donations.Include(d => d.Items).Where(dn => dn.Status == "Awaiting Collection").ToListAsync();
            return Ok(donations);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var donation = await _context.Donations.Include(d => d.Items).FirstOrDefaultAsync(i => i.Id == id);
            return Ok(donation);
        }


        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] ReactiveDonationDto donation)
        {
            // Validate Dto
            if (!ModelState.IsValid)
            {
                System.Console.WriteLine("ModelState invalid!");
                return BadRequest();
            }

            // Log
            System.Console.WriteLine("New donation incoming from: " + donation.Name);

            // Initialse an instance of the item helper class
            var itemHelper = new ItemHelper(_config);

            // Deserialise the items from JSON to a list of Item entities
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(donation.Items);

            // Upload the image for each Item to Cloudinary, and associate the url with the Item
            foreach (Item item in items)
            {
                for (int i = 0; i < donation.Pictures.Length; i++)
                {
                    IFormFile picture = donation.Pictures[i];

                    // Verify that this image is for this Item, if so upload
                    if (picture.FileName == item.Description)
                        item.PictureUrls += await itemHelper.UploadAndGetUri(item, picture) + ';';
                    System.Console.WriteLine($"Uploaded {i + 1} of {donation.Pictures.Length} images");
                }

            }

            // Create a new Donation entity populate with data
            Donation donationToSave = new Donation()
            {
                SubmissionDate = DateTime.Now,
                Name = donation.Name,
                Address = donation.Address,
                Email = donation.Email,
                Phone = donation.Phone,
                SmokeFree = donation.SmokeFree,
                PetFree = donation.PetFree,
                Items = items
            };

            // Save
            await _context.Donations.AddAsync(donationToSave);
            await _context.SaveChangesAsync();


            // Send acknowledgement email
            var templateAcknowledge = new TemplateHelper(donorName: donation.Name.Split(" ")[0]);
            await _emailHelper.SendEmail(MessageType.Acknowledge, templateAcknowledge.MessageAcknowledge, donation.Email);

            // Send email to admin 
            var adminEmail = Startup.Configuration.GetSection("Email:Username").Value;
            await _emailHelper.SendEmail(MessageType.New, $"New submission received from: {donation.Name}", adminEmail);

            return Ok();

        }

        [HttpPost("archive-submission")]
        public async Task<IActionResult> ArchiveSubmission(DonationForArchiveDto donationDto)
        {
            var donation = await _context.Donations.SingleOrDefaultAsync(d => d.Id == donationDto.Id);

            // Kill all personal details
            donation.Name = "[REDACTED]";
            donation.Email = "[REDACTED]";
            donation.Phone = "[REDACTED]";
            donation.Address = "[REDACTED]";
            donation.Status = "Archived";

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus(DonationForStatusUpdateDto donationDto)
        {
            var donation = await _context.Donations.Include(i => i.Items).SingleOrDefaultAsync(d => d.Id == donationDto.Id);

            // Additional checks before switching to 'Awaiting Collection'
            if (donation.Status == "Accepted")
            {
                // Check at least 1 item
                bool passed = false;
                foreach (var item in donation.Items)
                    if (item.Collect)
                        passed = true;

                if (!passed)
                    return BadRequest("Donations must have at least 1 item");

                // Set collection date
                donation.CollectionDate = donationDto.CollectionDate;
            }

            // Emails
            switch (donationDto.StatusToSet)
            {
                case "Awaiting Collection":
                    string itemsToAccept = "";
                    donation.Items.ForEach(i => { if (i.Collect) itemsToAccept += $"<li>{i.Description}</li>"; });

                    var templateAccept = new TemplateHelper(
                        donorName: donation.Name.Split(" ")[0],
                        itemList: itemsToAccept,
                        collectionDate: donationDto.CollectionDate.ToString("dd/MM/yyyy"),
                        donorPhone: donation.Phone
                        );
                    await _emailHelper.SendEmail(MessageType.Accept, templateAccept.MessageAccept, donation.Email);
                    break;
                case "Rejected":
                    string allItems = "";
                    donation.Items.ForEach(i => { allItems += $"<li>{i.Description}</li>"; });
                    var templateReject = new TemplateHelper(donorName: donation.Name.Split(" ")[0], itemList: allItems);
                    await _emailHelper.SendEmail(MessageType.Reject, templateReject.MessageReject, donation.Email);
                    break;
            }


            // Set the status
            donation.Status = donationDto.StatusToSet;

            // Save the status
            await _context.SaveChangesAsync();

            return Ok(donation);
        }
    }
}