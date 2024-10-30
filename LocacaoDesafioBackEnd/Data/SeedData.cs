using System.Text.Json;
using System.Security.Claims;
using LocacaoDesafioBackEnd.Data;
using LocacaoDesafioBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            try
            {
                var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Guest", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Verifica se já existem dados na tabela de usuários
                if (await userManager.Users.AnyAsync())
                {
                    return; // DB has been seeded
                }

                // Criação de usuários principais
                await CreateUserAsync(userManager, "admin", "admin@example.com", "admin", "Admin", isAdmin: true);
                await CreateUserAsync(userManager, "guest", "guest@example.com", "guest", "Guest");

                // Chama a função para popular veículos, entregadores e usuários para cada entregador
                await SeedVehiclesAndDeliverersAsync(context, userManager);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static async Task CreateUserAsync(
        UserManager<ApplicationUser> userManager,
        string userName,
        string email,
        string password,
        string role,
        bool isAdmin = false)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            IsAdmin = isAdmin
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
        else
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Erro ao criar usuário '{userName}': {error.Description}");
            }
        }
    }

    public static async Task SeedVehiclesAndDeliverersAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        var motosPath = Path.Combine(AppContext.BaseDirectory, "DadosParaInsert", "Motos.json");
        var motosJson = await File.ReadAllTextAsync(motosPath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var motos = JsonSerializer.Deserialize<List<Moto>>(motosJson, options);
        if (motos != null)
        {
            await context.Motos.AddRangeAsync(motos);
            await context.SaveChangesAsync();
        }

        var entregadoresPath = Path.Combine(AppContext.BaseDirectory, "DadosParaInsert", "Entregadores.json");
        var entregadoresJson = await File.ReadAllTextAsync(entregadoresPath);

        var entregadores = JsonSerializer.Deserialize<List<Entregador>>(entregadoresJson, options);
        if (entregadores != null)
        {
            await context.Entregadores.AddRangeAsync(entregadores);
            await context.SaveChangesAsync();

            // Criação de usuário para cada entregador
            foreach (var entregador in entregadores)
            {
                var userName = entregador.Identificador;
                var email = $"{userName.ToLower()}@example.com";
                var password = $"{userName.ToLower()}"; // Pode ser uma senha padrão ou gerada

                await CreateUserAsync(userManager, userName, email, password, "User");
            }
        }

        var locacoesPath = Path.Combine(AppContext.BaseDirectory, "DadosParaInsert", "Locacoes.json");
        var locacoesJson = await File.ReadAllTextAsync(locacoesPath);

        var locacoes = JsonSerializer.Deserialize<List<Locacao>>(locacoesJson, options);
        if (locacoes != null)
        {
            await context.Locacoes.AddRangeAsync(locacoes);
            await context.SaveChangesAsync();
        }
    }
}
