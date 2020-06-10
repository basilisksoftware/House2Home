using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public ItemsController(DataContext context)
        {
            _context = context;
        }

        // POST api/items/toggle-collection
        [HttpPost("toggle-collection")]
        public async Task<IActionResult> ToggleCollection(ItemForToggleDto itemDto)
        {
            Item item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemDto.Id);

            // Fail if item somehow doesn't exist
            if (item == null)
                return BadRequest();

            // Change the status of the item
            item.Collect = !item.Collect;
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}