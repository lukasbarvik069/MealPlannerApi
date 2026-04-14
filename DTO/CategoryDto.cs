using MealPlannerApi.Models;

namespace MealPlannerApi.DTO;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;

    public static CategoryDto ToDto(Category c)
    {
        return new CategoryDto
        {
            Id = c.Id,
            Name = c.Name
        };
    }
}