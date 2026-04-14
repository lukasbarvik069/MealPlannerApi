using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using MealPlannerApi.Models.Enums;

namespace MealPlannerApi.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FName { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string LName { get; set; } = String.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = String.Empty;
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = String.Empty;
    
    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = String.Empty;
    
    public Role Role { get; set; }

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Day> Days { get; set; } = new List<Day>();
    public ICollection<Food> Foods { get; set; } = new List<Food>();
}