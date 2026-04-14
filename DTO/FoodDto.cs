using MealPlannerApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MealPlannerApi.DTO;

public class FoodDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Recipe { get; set; }
    public string? ImageUrl { get; set; }
    public string? Creator { get; set; }
    public decimal Difficulty { get; set; }
    public decimal? AverageRating { get; set; }
    
    public bool? HasCustomNutrition { get; set; }
    public int PortionCount { get; set; }
    public NutritionDto? Nutrition { get; set; }
    
    public ICollection<string> Ingredients { get; set; }

    public static FoodDto ToDto(Food food)
    {
        return new FoodDto
        {
            Id = food.Id,
            Name = food.Name,
            Recipe = food.Recipe,
            ImageUrl = food.ImageUrl,
            Creator = food.Creator,
            Difficulty = food.Difficulty,
            AverageRating = food.AverageRating,
            HasCustomNutrition = food.HasCustomNutrition,
            Nutrition = NutritionDto.ToDto(food.Nutrition),
            PortionCount = food.PortionCount,
            Ingredients = food.FoodIngredients?.Select(fi => 
                $"{fi.Amount.ToString("G29")}{fi.Unit.Type} {fi.Ingredient.Name}").ToList() ?? new List<string>()
        };
    }

    public static Food FromDto(FoodDto dto)
    {
        return new Food
        {
            Id = 0,
            Name = dto.Name,
            Recipe = dto.Recipe,
            ImageUrl = dto.ImageUrl,
            Creator = dto.Creator,
            Difficulty = dto.Difficulty,
            AverageRating = dto.AverageRating,
            HasCustomNutrition = dto.HasCustomNutrition,
            Confirmed = false,
            PortionCount = dto.PortionCount
        };
    }
}