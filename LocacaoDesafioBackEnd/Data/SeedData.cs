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

                var roles = new[] { "Admin", "Guest" };
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

                var users = new List<ApplicationUser>
                {
                    new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true,
                        IsAdmin = true
                    },
                    new ApplicationUser
                    {
                        UserName = "guest",
                        Email = "guest@example.com",
                        EmailConfirmed = true
                    }
                };

                foreach (var user in users)
                {
                    var password = user.UserName == "admin" ? "admin" : "guest";
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        var role = user.UserName == "admin" ? "Admin" : "Guest";
                        if (!await userManager.IsInRoleAsync(user, role))
                        {
                            await userManager.AddToRoleAsync(user, role);
                        }
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"Erro ao criar usuário: {error.Description}");
                        }
                    }
                }

                // Chama a função para popular veículos e entregadores
                await SeedVehiclesAndDeliverersAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public static async Task SeedVehiclesAndDeliverersAsync(ApplicationDbContext context)
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
            foreach (var moto in motos)
            {
                await context.Motos.AddAsync(moto); 
            }

            await context.SaveChangesAsync();
        }

        var entregadoresPath = Path.Combine(AppContext.BaseDirectory, "DadosParaInsert", "Entregadores.json");
        var entregadoresJson = await File.ReadAllTextAsync(entregadoresPath);

        var entregadores = JsonSerializer.Deserialize<List<Entregador>>(entregadoresJson, options);
        if (entregadores != null)
        {
            await context.Entregadores.AddRangeAsync(entregadores);
            await context.SaveChangesAsync();
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