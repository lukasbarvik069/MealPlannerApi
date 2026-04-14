namespace MealPlannerApi.Models;

public class FoodIngredient
{
    public int Id { get; set; }
    
    public int IngredientId { get; set; }
    public int FoodId { get; set; }
    public int UnitId { get; set; }
    
    public decimal Amount { get; set; }

    public Ingredient? Ingredient { get; set; }
    public Food? Food { get; set; }
    public Unit? Unit { get; set; }
}