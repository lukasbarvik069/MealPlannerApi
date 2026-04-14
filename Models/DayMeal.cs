using System.Text.Json.Serialization;

namespace MealPlannerApi.Models;

public class DayMeal
{
    public int Id { get; set; }
    
    public int DayId { get; set; }
    public int MealId { get; set; }
    public int? FoodId { get; set; }
    
    public TimeSpan Time { get; set; }
    
    [JsonIgnore]
    public Day? Day { get; set; }
    public Meal? Meal { get; set; }
    public Food? Food { get; set; }
}