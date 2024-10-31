using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit;
using LocacaoDesafioBackEnd.Controllers;
using LocacaoDesafioBackEnd.Models;

namespace LocacaoDesafioBackEnd.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            // Configura o mock do UserManager
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            // Configura o mock da configuração
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkey12345");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("testissuer");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("testaudience");

            // Cria o controller
            _authController = new AuthController(_userManagerMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser" };
            var loginModel = new LoginModel { Username = "testuser", Password = "password123" };

            _userManagerMock.Setup(x => x.FindByNameAsync(loginModel.Username)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);

            // Act
            var result = await _authController.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<dynamic>(okResult.Value);
            Assert.NotNull(token.Token);

            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkey12345")),
                ValidateIssuer = true,
                ValidIssuer = "testissuer",
                ValidateAudience = true,
                ValidAudience = "testaudience"
            };

            // Corrigido: Declarar explicitamente o tipo de validatedToken
            var principal = jwtHandler.ValidateToken(token.Token, tokenValidationParameters, out SecurityToken validatedToken);
            Assert.NotNull(principal);
            Assert.Equal("testuser", principal.Identity.Name);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "testuser", Password = "wrongpassword" };

            _userManagerMock.Setup(x => x.FindByNameAsync(loginModel.Username)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _authController.Login(loginModel);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Credenciais inválidas.", ((dynamic)unauthorizedResult.Value).Message);
        }
    }
}
