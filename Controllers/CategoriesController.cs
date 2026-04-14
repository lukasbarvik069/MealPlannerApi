using MealPlannerApi.Data;
using MealPlannerApi.DTO;
using MealPlannerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlannerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var categories = await _context
            .Categories
            .OrderBy(c => c.Name)
            .ToListAsync();

        var dto = categories.Select(c => CategoryDto.ToDto(c));
        return Ok(categories);
    }
}