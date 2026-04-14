using System.ComponentModel.DataAnnotations.Schema;

namespace MealPlannerApi.Models;

public class Day
{
    public int Id { get; set; }
    public int UserId { get; set; }
    
    [Column(TypeName = "date")]
    public DateTime Date { get; set; }

    public ICollection<DayMeal> DayMeals { get; set; } = new List<DayMeal>();
    
    public User? User { get; set; }
}