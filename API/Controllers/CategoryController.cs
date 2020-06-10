using System.Threading.Tasks;
using API.Dtos;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        // GET api/category
        [HttpGet("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // POST api/category
        [HttpPost("")]
        public async Task<IActionResult> AddCategory(CategoryForAddDto categoryDto)
        {
            var category = new Category()
            {
                CategoryName = categoryDto.CategoryName,
                VerifyFireLabels = categoryDto.VerifyFireLabels
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return Ok(category);

        }


        // DELETE api/category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}