namespace MealPlannerApi.Models;

public class Nutrition
{
    public int Id { get; set; }
    
    public decimal Kcal { get; set; }
    public decimal Protein { get; set; }
    public decimal Fat { get; set; }
    public decimal Sacharid { get; set; }
}