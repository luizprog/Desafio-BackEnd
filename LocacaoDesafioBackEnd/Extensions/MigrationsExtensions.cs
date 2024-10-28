using Microsoft.EntityFrameworkCore;
using LocacaoDesafioBackEnd.Data;

namespace LocacaoDesafioBackEnd.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                // Opcional: Excluir o banco de dados existente e criar um novo
                //dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Aqui você pode registrar o erro ou tomar alguma ação apropriada
                Console.WriteLine($"Error applying migrations: {ex.Message}");
            }
        }
    }
}
