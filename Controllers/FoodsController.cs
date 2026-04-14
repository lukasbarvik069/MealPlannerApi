using MealPlannerApi.Data;
using MealPlannerApi.DTO;
using MealPlannerApi.DTO.CreateMealDtos;
using MealPlannerApi.Models;
using MealPlannerApi.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;

namespace MealPlannerApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class FoodsController : ControllerBase
{

    private readonly AppDbContext _context;

    public FoodsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodRowDto>>> GetFoodRows()
    {
        var foods = await _context.Foods
            .Include(f => f.Nutrition)
            .Where(f => f.Confirmed && f.IsPublic)
            .AsNoTracking()
            .ToListAsync();
        var rows = foods.Select(f => FoodRowDto.ToDto(f)).ToList();

        return Ok(rows);
    }

    [HttpGet("{filter}")]
    public async Task<ActionResult<IEnumerable<FoodRowDto>>> GetFoodsByFilter(string filter)
    {
        var foods = await _context
            .Foods
            .AsNoTracking()
            .Where(f => f.Confirmed && f.IsPublic && f.Name == filter)
            .ToListAsync();

        var rows = foods.Select(f => FoodRowDto.ToDto(f)).ToList();

        return Ok(rows);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<FoodDto>>> GetPendingFoods()
    {
        try 
        {
            var foods = await _context.Foods
                .Include(f => f.Nutrition)
                .Where(f => f.Confirmed == false)
                .AsNoTracking()
                .ToListAsync();

            var dtos = foods.Select(f => FoodDto.ToDto(f)).ToList();
            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FoodDto>> GetById(int id)
    {
        var food = await _context.Foods
            .AsNoTracking()
            .Include(f => f.Nutrition)
            .Include(f => f.FoodIngredients)
            .ThenInclude(fi => fi.Ingredient)
            .Include(f => f.FoodIngredients)
            .ThenInclude(fi => fi.Unit)
            .Where(f => f.Confirmed && f.IsPublic)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if (food == null)
        {
            return NotFound(new { message = $"Jídlo s id {id} nenalezeno!" });
        }

        var dto = FoodDto.ToDto(food);
        return Ok(dto);
    }
    
    [HttpGet("myfood/{id}")]
    public async Task<ActionResult<FoodDto>> GetByUserFoodById(int id, [FromQuery]int userId)
    {
        var food = await _context.Foods
            .AsNoTracking()
            .Include(f => f.Nutrition)
            .Include(f => f.FoodIngredients)
            .ThenInclude(fi => fi.Ingredient)
            .Include(f => f.FoodIngredients)
            .ThenInclude(fi => fi.Unit)
            .Where(f => f.UserId == userId)
            .FirstOrDefaultAsync(f => f.Id == id);
        
        if (food == null)
        {
            return NotFound(new { message = $"Jídlo s id {id} nenalezeno!" });
        }

        var dto = FoodDto.ToDto(food);
        return Ok(dto);
    }

    [HttpPost("create")]
    [Authorize(Roles = "NormalUser,Admin")]
    public async Task<ActionResult> CreateMeal([FromBody] CreateMealDto dto)
    {

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var food = CreateMealDto.ToFood(dto);

            var user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
            {
                await transaction.RollbackAsync();
                return BadRequest("User not found");
            }

            food.Creator = user?.Username;
            

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            var categoryIds = dto.CategoryDtos?.Select(c => c.Id).ToList();
            if (categoryIds != null)
            {
                var categories = await _context
                    .Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .ToListAsync();

                foreach (var c in categories)
                {
                    var foodCategory = new FoodCategory
                    {
                        Id = 0,
                        FoodId = food.Id,
                        CategoryId = c.Id
                    };
                    _context.FoodCategories.Add(foodCategory);
                }
            }


            foreach (var i in dto.IngredientDtos)
            {
                var ingredient = await _context
                    .Ingredients
                    .FirstOrDefaultAsync(ing => ing.Name == i.Name);
                if (ingredient == null)
                {
                    ingredient = CreateMealDto.ToIngredient(i);
                    _context.Ingredients.Add(ingredient);
                    await _context.SaveChangesAsync();
                }

                var unit = await _context
                    .Units
                    .FirstOrDefaultAsync(u => u.Type == i.Unit);

                if (unit == null)
                {
                    await transaction.RollbackAsync();
                    return BadRequest("Unit not found");
                }

                var foodIngredient = new FoodIngredient
                {
                    Id = 0,
                    IngredientId = ingredient.Id,
                    FoodId = food.Id,
                    UnitId = unit.Id,
                    Amount = i.Amount
                };

                _context.FoodIngredients.Add(foodIngredient);
            }


            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return Ok(food.Id);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, "Error creating meal: " + ex.Message);
        }
        
    }
    
}