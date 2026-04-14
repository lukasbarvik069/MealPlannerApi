namespace MealPlannerApi.DTO;

public class IngredientDto
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public string Unit { get; set; }
    public int Order { get; set; }
}