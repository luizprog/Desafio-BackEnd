using System;
using System.Threading.Tasks;
using LocacaoDesafioBackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task CreateUserAsync(string userName, string userEmail, bool isAdmin)
    {
        // Verifica se o usuário já existe
        if (await _userManager.FindByNameAsync(userName) != null)
            return;

        // Garantir que os papéis necessários existam
        await EnsureRolesAsync();

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userEmail,
            EmailConfirmed = true,
            IsAdmin = isAdmin
        };

        var password = userName == "admin" ? "admin" : userName;
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var role = isAdmin ? "Admin" : "User";
            await _userManager.AddToRoleAsync(user, role);
        }
        else
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError($"Erro ao criar usuário: {error.Description}");
            }
        }
    }

    private async Task EnsureRolesAsync()
    {
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
