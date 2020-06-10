using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using API.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static API.Helpers.DateHelper;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class StatisticsController : ControllerBase
    {
        private readonly DataContext _context;

        public StatisticsController(DataContext context)
        {
            _context = context;
        }

        // GET api/statistics
        [HttpGet("")]
        public async Task<ActionResult> Get()
        {

            var donationStats = new DonationsStatisticsDto();
            var itemStats = new ItemsStatisticsDto();

            // Initialise instance of Gregorian Calendar to get week number of year
            var greg = new System.Globalization.GregorianCalendar();

            // Get donations
            List<Donation> donations = await _context.Donations.Include(d => d.Items).ToListAsync();

            // Get Items
            List<Item> items = await _context.Items.ToListAsync();

            // Donation stats ----------------------------------------------------

            donationStats.AwaitingResponse = donations.Count(d => d.Status == null);
            donationStats.AwaitingCollection = donations.Count(d => d.Status == "Accepted");
            donationStats.CollectionArranged = donations.Count(d => d.Status == "Awaiting Collection");

            // Week
            donationStats.ReceivedWeek = donations.Count(
                d => WeekNumber(d.SubmissionDate) == WeekNumber(DateTime.Now));
            donationStats.RejectedWeek = donations.Count(
                d => d.Status == "Rejected"
                && WeekNumber(d.SubmissionDate) == WeekNumber(DateTime.Now));

            // Month
            donationStats.ReceivedMonth = donations.Count(d => d.SubmissionDate.Month == DateTime.Now.Month);
            donationStats.RejectedMonth = donations.Count(d => d.Status == "Rejected" && d.SubmissionDate.Month == DateTime.Now.Month);
            // Year
            donationStats.ReceivedYear = donations.Count(d => d.SubmissionDate.Year == DateTime.Now.Year);
            donationStats.RejectedYear = donations.Count(d => d.SubmissionDate.Year == DateTime.Now.Year && d.Status == "Rejected");

            // All time
            donationStats.ReceivedAll = donations.Count;
            donationStats.RejectedAll = donations.Count(d => d.Status == "Rejected");

            // Donation stats ----------------------------------------------------


            // Item stats --------------------------------------------------------
            foreach (var don in donations)
            {
                // Week
                if (WeekNumber(don.SubmissionDate) == WeekNumber(DateTime.Now))
                {
                    itemStats.ReceivedWeek += don.Items.Count;
                    itemStats.RejectedWeek += don.Items.Count(i => i.Collect == false);
                }

                // Month
                if (don.SubmissionDate.Month == DateTime.Now.Month)
                {
                    itemStats.ReceivedMonth += don.Items.Count;
                    itemStats.RejectedMonth += don.Items.Count(i => i.Collect == false);
                }

                // Year
                if (don.SubmissionDate.Year == DateTime.Now.Year)
                {
                    itemStats.ReceivedYear += don.Items.Count;
                    itemStats.RejectedYear += don.Items.Count(i => i.Collect == false);
                }
            }
            
            // All time
            itemStats.ReceivedAll = items.Count;
            itemStats.RejectedAll = items.Count(i => i.Collect == false);

            // Item stats --------------------------------------------------------

            // Create dto to send
            StatisticsDto stats = new StatisticsDto()
            {
                Donations = donationStats,
                Items = itemStats
            };

            return Ok(stats);
        }


    }
}