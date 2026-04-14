using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MealPlannerApi.Models;

public class Meal
{
    public int Id { get; set; }

    [Required] 
    [MaxLength(50)]
    [MinLength(1)]
    public string Type { get; set; } = String.Empty;

    [JsonIgnore]
    public ICollection<DayMeal> DayMeals { get; set; } = new List<DayMeal>();

}