using MealPlannerApi.Models;
using MealPlannerApi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MealPlannerApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }

    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Nutrition> Nutritions { get; set; }
    public DbSet<Day> Days { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<DayMeal> DayMeals { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Unit> Units { get; set; }
    public DbSet<FoodIngredient> FoodIngredients { get; set; }
    public DbSet<FoodCategory> FoodCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FoodIngredient>()
            .Property(fi => fi.Amount)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<Nutrition>(entity => {
            entity.Property(e => e.Kcal).HasPrecision(18, 2);
            entity.Property(e => e.Protein).HasPrecision(18, 2);
            entity.Property(e => e.Fat).HasPrecision(18, 2);
            entity.Property(e => e.Sacharid).HasPrecision(18, 2);
        });

        modelBuilder.Entity<FoodIngredient>(e => e.Property(p => p.Amount).HasPrecision(18, 2));

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
        });
        
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>();

        modelBuilder.Entity<Food>()
            .HasOne(f => f.Nutrition)
            .WithMany()
            .HasForeignKey(f => f.NutritionId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Food>()
            .HasOne(f => f.User)
            .WithMany(u => u.Foods)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Ingredient>()
            .HasOne(i => i.Nutrition)
            .WithMany()
            .HasForeignKey(i => i.NutritionId)
            .OnDelete(DeleteBehavior.SetNull);
        
        
        // SEED DATA
        var nutrVejce = new Nutrition { Id = 1, Kcal = 155, Protein = 13, Fat = 11, Sacharid = 1.1m };
        var nutrMaslo = new Nutrition { Id = 2, Kcal = 717, Protein = 0.8m, Fat = 81, Sacharid = 0.1m };
        var nutrChleb = new Nutrition { Id = 3, Kcal = 250, Protein = 9, Fat = 1, Sacharid = 48 };
        var nutrMleko = new Nutrition { Id = 4, Kcal = 42, Protein = 3.4m, Fat = 1.5m, Sacharid = 5 };
        var nutrSunka = new Nutrition { Id = 5, Kcal = 145, Protein = 21, Fat = 6, Sacharid = 1 };

        modelBuilder.Entity<Nutrition>().HasData(nutrVejce, nutrMaslo, nutrChleb, nutrMleko, nutrSunka);

        modelBuilder.Entity<Unit>().HasData(
            new Unit { Id = 1, Type = "g" },
            new Unit { Id = 2, Type = "ml" },
            new Unit { Id = 3, Type = "ks" },
            new Unit { Id = 4, Type = "kg" },
            new Unit { Id = 5, Type = "l" }
        );

        modelBuilder.Entity<Ingredient>().HasData(
            new Ingredient { Id = 1, Name = "Vejce", NutritionId = 1 },
            new Ingredient { Id = 2, Name = "Máslo", NutritionId = 2 },
            new Ingredient { Id = 3, Name = "Chléb", NutritionId = 3 },
            new Ingredient { Id = 4, Name = "Mléko", NutritionId = 4 },
            new Ingredient { Id = 5, Name = "Šunka", NutritionId = 5 }
        );
        
        decimal totalKcal = (200m / 100 * 155) + (10m / 100 * 717) + (50m / 100 * 42);
        decimal totalProtein = (200m / 100 * 13) + (10m / 100 * 0.8m) + (50m / 100 * 3.4m); 
        decimal totalFat = (200m / 100 * 11) + (10m / 100 * 81) + (50m / 100 * 1.5m);
        decimal totalCarbs = (200m / 100 * 1.1m) + (10m / 100 * 0.1m) + (50m / 100 * 5);


        var nutrVajickaJidlo = new Nutrition 
        { 
            Id = 6, 
            Kcal = totalKcal, 
            Protein = totalProtein, 
            Fat = totalFat, 
            Sacharid = totalCarbs 
        };
        modelBuilder.Entity<Nutrition>().HasData(nutrVajickaJidlo);

        modelBuilder.Entity<Food>().HasData(new Food
        {
            Id = 1,
            UserId = 1,
            Name = "Míchaná vajíčka na másle",
            Recipe = "Rozpusťte máslo, přidejte vejce rozšlehaná s mlékem a pomalu míchejte do ztuhnutí. Podávejte s chlebem. Recept pro 1 osobu.",
            Difficulty = 1,
            AverageRating = 5,
            NutritionId = 6,
            HasCustomNutrition = true,
            Creator = "Luky",
            Confirmed = true,
            IsPublic = true,
            PortionCount = 1
        });

        modelBuilder.Entity<FoodIngredient>().HasData(
            new FoodIngredient { Id = 1, FoodId = 1, IngredientId = 1, UnitId = 3, Amount = 4 }, // 4 ks vajec
            new FoodIngredient { Id = 2, FoodId = 1, IngredientId = 2, UnitId = 1, Amount = 10 }, // 10g másla
            new FoodIngredient { Id = 3, FoodId = 1, IngredientId = 4, UnitId = 2, Amount = 50 }  // 50ml mléka
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FName = "L",
                LName = "B",
                Username = "LB",
                Email = "LB@example.com",
                Role = Role.Admin,
                PasswordHash = "placeholder_hash", 
            }
        );
        
        modelBuilder.Entity<Day>().HasData(new Day
        {
            Id = 1,
            UserId = 1,
            Date = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc)
        });

        modelBuilder.Entity<Meal>().HasData(
            new Meal { Id = 1, Type = "Snídaně" },
            new Meal { Id = 2, Type = "Oběd" }
        );

        modelBuilder.Entity<DayMeal>().HasData(new DayMeal
        {
            Id = 1,
            DayId = 1,   
            MealId = 1,     
            FoodId = 1,   
            Time = new TimeSpan(8, 30, 0)
        });

        modelBuilder.Entity<Category>().HasData(
            new Category {
                Id = 1,
                Name = "Snídaně"
            },
            new Category
            {
                Id = 2,
                Name = "Oběd"
            },
            new Category
            {
                Id = 3,
                Name = "Večeře"
            },
            new Category
            {
                Id = 4,
                Name = "Svačina"
            },
            new Category
            {
                Id = 5,
                Name = "Slané"
            },
            new Category
            {
                Id = 6,
                Name = "Sladké"
            }
        );

        modelBuilder.Entity<FoodCategory>().HasData(
            new FoodCategory {Id = 1, FoodId = 1, CategoryId = 1},
            new FoodCategory {Id = 2, FoodId = 1, CategoryId = 5}    
        );

    } 
}