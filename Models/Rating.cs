using System.ComponentModel.DataAnnotations;

namespace MealPlannerApi.Models;

public class Rating
{
    public int Id { get; set; }
    public int FoodId { get; set; }
    public int? UserId { get; set; }
    
    [Range(0,5)]
    public decimal Value { get; set; }
    
    [Required] 
    public Food Food { get; set; }
    public User? User { get; set; }
    
}