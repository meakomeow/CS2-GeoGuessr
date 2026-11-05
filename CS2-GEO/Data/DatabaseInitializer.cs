using Microsoft.EntityFrameworkCore;

namespace CS2_GEO.Data;

public static class DatabaseInitializer
{
    /// <summary>Applies pending migrations, then seeds the database if it is still empty.</summary>
    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();

        await db.Database.MigrateAsync();

        var seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "seed.json");
        await DatabaseSeeder.SeedAsync(db, seedPath);
    }
}
