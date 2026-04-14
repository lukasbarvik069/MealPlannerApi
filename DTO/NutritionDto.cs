using MealPlannerApi.Models;

namespace MealPlannerApi.DTO;

public class NutritionDto
{
    public decimal? Kcal { get; set; }
    public decimal? Protein { get; set; }
    public decimal? Fat { get; set; }
    public decimal? Sacharid { get; set; }

    public static NutritionDto? ToDto(Nutrition? nutrition)
    {
        if (nutrition == null) return null;
        
        return new NutritionDto
        {
            Kcal = nutrition.Kcal,
            Protein = nutrition.Protein,
            Fat = nutrition.Fat,
            Sacharid = nutrition.Sacharid
        };
    }
}