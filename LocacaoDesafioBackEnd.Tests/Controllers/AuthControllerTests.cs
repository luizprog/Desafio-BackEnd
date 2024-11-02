using System.Collections.Generic;
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
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

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

            // Configura o mock da configuração com uma chave de pelo menos 256 bits
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("thisisaverylongsecretkeyforjwttoken12345"); // Chave de 256 bits
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
            _userManagerMock.Setup(x => x.IsInRoleAsync(user, "Admin")).ReturnsAsync(false); // Mock para o papel do usuário

            // Act
            var result = await _authController.Login(loginModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tokenResponse = Assert.IsType<Dictionary<string, string>>(okResult.Value); // Esperando um Dictionary
            Assert.NotNull(tokenResponse["Token"]);
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
            var value = Assert.IsType<ErrorResponse>(unauthorizedResult.Value); // Mudança para dynamic

            Assert.NotNull(value); // Verifica se o objeto não é nulo
            Assert.Equal("Credenciais inválidas.", value.Message); // Acessa a propriedade Message do objeto anônimo
        }
    }
}
