using System.Security.Claims;
using MealPlannerApi.Data;
using MealPlannerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MealPlannerApi.DTO;
using Microsoft.IdentityModel.Tokens;


namespace MealPlannerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto login)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == login.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Neplatné přihlašovací údaje" });
        }
        
        var token = CreateToken(user);

        return Ok(new
        {
            id = user.Id,
            username = user.Username, 
            email = user.Email,
            role = user.Role.ToString(),
            token = token
        });
    }

    [HttpPost("signup")]
    public async Task<ActionResult> SignUp([FromBody] SignUpDto dto)
    {
        try
        {


            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username || u.Email == dto.Email);

            if (user != null)
            {
                return BadRequest(new { message = "Uživatel s daným jménem nebo emailem už existuje" });
            }

            dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = SignUpDto.FromDto(dto);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            Console.WriteLine($"Uživatel {newUser.Username} byl úspěšně uložen do DB s ID {newUser.Id}");

            var token = CreateToken(newUser);

            return Ok(new
            {
                token = token,
                username = newUser.Username,
                message = "Registrace proběhla úspěšně"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("CHYBA PŘI REGISTRACI: " + ex.Message);
            if (ex.InnerException != null) Console.WriteLine("INNER: " + ex.InnerException.Message);
        
            return StatusCode(500, new { message = "Chyba při registraci: " + ex.Message });
        }
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var jwtKey = _config.GetSection("Jwt:Key").Value;

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new Exception("JWT key nebyl nalezen");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}