using System.ComponentModel.DataAnnotations;

namespace MealPlannerApi.Models;

public class Unit
{
    public int Id { get; set; }

    [Required]
    [MaxLength(10)]
    [MinLength(1)]
    public string Type { get; set; } = String.Empty;

    public ICollection<FoodIngredient> FoodIngredients { get; set; } = new List<FoodIngredient>();

}