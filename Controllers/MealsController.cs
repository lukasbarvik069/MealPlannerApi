using MealPlannerApi.Data;
using MealPlannerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlannerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MealsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Meal>>> GetMeals()
    {
        return await _context.Meals.AsNoTracking().ToListAsync();
    }
}