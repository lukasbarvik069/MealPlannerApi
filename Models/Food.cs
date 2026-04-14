using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MealPlannerApi.Models;

public class Food
{
    public int Id { get; set; }
    public int? NutritionId { get; set; }
    public int? UserId { get; set; }

    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public string Name { get; set; } = String.Empty;
    
    [Column(TypeName = "text")]
    [MaxLength(10000)]
    public string? Recipe { get; set; }
    [MaxLength(2048)]
    public string? ImageUrl { get; set; }

    [MaxLength(50)]
    [MinLength(1)]
    public string? Creator { get; set; }
    
    [Required]
    public decimal Difficulty { get; set; }
    [Range(0, 5)]
    public decimal? AverageRating { get; set; }
    
    public bool? HasCustomNutrition { get; set; }

    [Required] 
    public bool Confirmed { get; set; } = false;

    [Required] public bool IsPublic { get; set; } = false;

    [Required] public int PortionCount { get; set; } = 2;
    
    [JsonIgnore]
    public ICollection<DayMeal> DayMeals { get; set; } = new List<DayMeal>();
    public ICollection<FoodIngredient> FoodIngredients { get; set; } = new List<FoodIngredient>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<FoodCategory> FoodCategories { get; set; } = new List<FoodCategory>();
    
    public Nutrition? Nutrition { get; set; }
    public User? User { get; set; }
}