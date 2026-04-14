using MealPlannerApi.Models;
using MealPlannerApi.Models.Enums;

namespace MealPlannerApi.DTO;

public class SignUpDto
{
    public string FName { get; set; } = String.Empty;
    public string LName { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;

    public string Username { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;

    public static User FromDto(SignUpDto dto)
    {
        return new User
        {
            Id = 0,
            FName = dto.FName,
            LName = dto.LName,
            Email = dto.Email,
            Username = dto.Username,
            PasswordHash = dto.Password,
            Role = Role.NormalUser
        };
    }
}   