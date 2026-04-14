using System.ComponentModel.DataAnnotations;

namespace MealPlannerApi.Models;

public class Ingredient
{
    public int Id { get; set; }
    public int? NutritionId { get; set; }

    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    public string Name { get; set; } = String.Empty;

    public bool Confirmed { get; set; } = false;

    public ICollection<FoodIngredient> FoodIngredients { get; set; } = new List<FoodIngredient>();
    public Nutrition? Nutrition { get; set; }
}