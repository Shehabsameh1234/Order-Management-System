using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using OrderSys.Core.Service.Contract;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Specifications.UserSpecifications;
using Talabat.Core;

namespace UnitTest
{
    public class UsersControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _authServiceMock = new Mock<IAuthService>();
            _controller = new UsersController(_unitOfWorkMock.Object, _authServiceMock.Object);
        }

        [Fact]
        public async Task LogIn_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var model = new LogInDto { Username = "testuser", Password = "password123" };
            var user = new User { Username = "testuser", PasswordHash = "password123", Role = UserRole.Customer };

            _unitOfWorkMock.Setup(u => u.Repository<User>().GetUserByUserName(It.IsAny<UserSpecifications>())).ReturnsAsync(user);
            _authServiceMock.Setup(a => a.CreateTokenAsync(user)).ReturnsAsync("fakeToken");

            // Act
            var result = await _controller.LogIn(model);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeOfType<UserDto>();
            var userDto = okResult.Value.As<UserDto>();
            userDto.Username.Should().Be("testuser");
            userDto.Role.Should().Be(UserRole.Customer.ToString());
            userDto.Token.Should().Be("fakeToken");
        }

        [Fact]
        public async Task LogIn_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var model = new LogInDto { Username = "testuser", Password = "wrongpassword" };
            var user = new User { Username = "testuser", PasswordHash = "password123", Role = UserRole.Customer };

            _unitOfWorkMock.Setup(u => u.Repository<User>().GetUserByUserName(It.IsAny<UserSpecifications>())).ReturnsAsync(user);

            // Act
            var result = await _controller.LogIn(model);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeOfType<ApisResponse>();
            var apiResponse = badRequestResult.Value.As<ApisResponse>();
            apiResponse.StatusCode.Should().Be(400);
            apiResponse.Messege.Should().Be("invalid login");
        }

        [Fact]
        public async Task Register_ValidModel_ReturnsOkWithToken()
        {
            // Arrange
            var model = new RegisterDto { Username = "newuser", Password = "newpassword", Role = UserRole.Admin.ToString() };

            _unitOfWorkMock.Setup(u => u.Repository<User>().CheckUserNameExsist(It.IsAny<UserSpecifications>())).ReturnsAsync(false);

            // Act
            var result = await _controller.Register(model);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeOfType<UserDto>();
            var userDto = okResult.Value.As<UserDto>();
            userDto.Username.Should().Be("newuser");
            userDto.Role.Should().Be(UserRole.Admin.ToString());
            userDto.Token.Should().Be("LogIn For Token"); // Adjust accordingly based on actual implementation
        }

        [Fact]
        public async Task Register_UserNameExists_ReturnsBadRequest()
        {
            // Arrange
            var model = new RegisterDto { Username = "existinguser", Password = "newpassword", Role = UserRole.Customer.ToString() };

            _unitOfWorkMock.Setup(u => u.Repository<User>().CheckUserNameExsist(It.IsAny<UserSpecifications>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Register(model);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeOfType<ApisResponse>();
            var apiResponse = badRequestResult.Value.As<ApisResponse>();
            apiResponse.StatusCode.Should().Be(400);
            apiResponse.Messege.Should().Be("userName in use try another one");
        }

        [Fact]
        public async Task Register_InvalidRole_ReturnsBadRequest()
        {
            // Arrange
            var model = new RegisterDto { Username = "newuser", Password = "newpassword", Role = "InvalidRole" };

            // Act
            var result = await _controller.Register(model);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().BeOfType<ApisResponse>();
            var apiResponse = badRequestResult.Value.As<ApisResponse>();
            apiResponse.StatusCode.Should().Be(400);
            apiResponse.Messege.Should().Be("this role does not exist try another (Admin,Customer)");
        }

        [Fact]
        public async Task CheckUserNameExist_ExistingUserName_ReturnsTrue()
        {
            // Arrange
            var userName = "existinguser";
            _unitOfWorkMock.Setup(u => u.Repository<User>().CheckUserNameExsist(It.IsAny<UserSpecifications>())).ReturnsAsync(true);

            // Act
            var result = await _controller.CheckUserNameExist(userName);

            // Assert
            result.Should().BeOfType<ActionResult<bool>>();
            var actionResult = result.Should().BeOfType<ActionResult<bool>>().Subject;
            actionResult.Value.Should().BeTrue();
        }

        [Fact]
        public async Task CheckUserNameExist_NonExistingUserName_ReturnsFalse()
        {
            // Arrange
            var userName = "nonexistinguser";
            _unitOfWorkMock.Setup(u => u.Repository<User>().CheckUserNameExsist(It.IsAny<UserSpecifications>())).ReturnsAsync(false);

            // Act
            var result = await _controller.CheckUserNameExist(userName);

            // Assert
            result.Should().BeOfType<ActionResult<bool>>();
            var actionResult = result.Should().BeOfType<ActionResult<bool>>().Subject;
            actionResult.Value.Should().BeFalse();
        }
    }
}
