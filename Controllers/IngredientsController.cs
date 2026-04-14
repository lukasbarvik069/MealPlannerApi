using MealPlannerApi.Data;
using MealPlannerApi.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlannerApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public IngredientsController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Admin,NormalUser,PremiumUser")]
    [HttpGet("search/{name}")]
    public async Task<ActionResult<IEnumerable<string>>> Search(string name)
    {
        if (string.IsNullOrEmpty(name)) return Ok(new string[] {});
        
        var ingredients = await _context.Ingredients
            .Where(i => EF.Functions.Unaccent(i.Name).Contains(EF.Functions.Unaccent(name)))
            .Take(10)
            .ToListAsync();
        
        return Ok(ingredients.Select(i => i.Name).ToArray());
    }

}