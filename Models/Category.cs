using System.ComponentModel.DataAnnotations;

namespace MealPlannerApi.Models;

public class Category
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = String.Empty;
    
    private ICollection<FoodCategory> FoodCategories { get; set; } = new List<FoodCategory>();
}