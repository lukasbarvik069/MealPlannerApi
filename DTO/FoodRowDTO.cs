using MealPlannerApi.Models;

namespace MealPlannerApi.DTO;

public class FoodRowDto
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Creator { get; set; }
    public decimal Difficulty { get; set; }
    public decimal? AverageRating { get; set; }
    public decimal? Kcal { get; set; }

    
    public static FoodRowDto ToDto(Food food)
    {
        return new FoodRowDto
        {
            Id = food.Id,
            ImageUrl = food.ImageUrl,
            Name = food.Name,
            Creator = food.Creator,
            Difficulty = food.Difficulty,
            AverageRating = food.AverageRating,
            Kcal = food.Nutrition?.Kcal
        };
    }
}