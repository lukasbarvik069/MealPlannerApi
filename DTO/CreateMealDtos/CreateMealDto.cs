using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using MealPlannerApi.Models;

namespace MealPlannerApi.DTO.CreateMealDtos;

public class CreateMealDto
{
    public int UserId { get; set; }
    
    [Required]
    public string MealName { get; set; } = string.Empty;
    public ICollection<IngredientDto> IngredientDtos { get; set; } = new List<IngredientDto>();
    public string? Recipe { get; set; }
    
    public int PortionCount { get; set; }
    public ICollection<CategoryDto>? CategoryDtos { get; set; }
    
    [Required]
    public bool IsPublic { get; set; }
    

    public static Food ToFood(CreateMealDto dto)
    {
        var recipe = dto.Recipe ?? "";
        var cleanedRecipe = string.Join("\n", recipe
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrEmpty(line))
        );
        return new Food
        {
            UserId = Convert.ToInt32(dto.UserId),
            Name = dto.MealName,
            Recipe = cleanedRecipe,
            IsPublic = dto.IsPublic,
            PortionCount = dto.PortionCount,
            Confirmed = false
        };
    }

    public static Ingredient ToIngredient(IngredientDto dto)
    {
        return new Ingredient
        {
            Id = 0,
            Name = dto.Name,
            Confirmed = false
        };
    }
}