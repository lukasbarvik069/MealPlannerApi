using MealPlannerApi.Data;
using MealPlannerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MealPlannerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DaysController : ControllerBase
{
    private readonly AppDbContext _context;

    public DaysController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("week")]
    public async Task<ActionResult<IEnumerable<Day>>> GetWeek(DateTime start, DateTime end)
    {
        var days = await _context.Days
            .Include(d => d.DayMeals)
            .ThenInclude(dm => dm.Meal)
            .Include(d => d.DayMeals)
            .ThenInclude(dm => dm.Food)
            .Where(d => d.Date >= start.Date && d.Date <= end.Date)
            .OrderBy(d => d.Date)
            .AsNoTracking()
            .ToListAsync<Day>();

        return days;
    }

}