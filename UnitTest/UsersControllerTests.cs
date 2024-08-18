using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using OrderSys.Core.Service.Contract;
using Order_Management_System.Controllers;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Specifications.UserSpecifications;
using Talabat.Core;
using FakeItEasy;
using AutoMapper;
using Talabat.Repository;
using OrderSys.Core.Repository.Contract;

namespace UnitTest
{
    public class UsersControllerTests
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UsersController _usersController;
        private readonly IGenericRepository<User > _genericRepository;

        public UsersControllerTests()
        {
            _authService = A.Fake<IAuthService>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _genericRepository = A.Fake<IGenericRepository<User>>();
            _usersController = new UsersController(_unitOfWork, _authService);
        }
        [Fact]
        public async Task UsersController_LogIn_ReturnOk()
        {
            //Arrange
            var logInDto = A.Fake<LogInDto>();
            var spec = A.Fake<UserSpecifications>();
            var user = A.Fake<User>();
            A.CallTo(() => _unitOfWork.Repository<User>()).Returns(_genericRepository);
            A.CallTo(() => _genericRepository.GetUserByUserName(spec))
                .Returns(Task.FromResult(user));
            //Act
            var result = await _usersController.LogIn(logInDto);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>();      
        }
        [Fact]
        public async Task UsersController_LogIn_ReturnBadRequest_UserIsNull()
        {

            //Arrange
            var  logInDto = new LogInDto() { Username="user",Password="pass"};
            var spec = A.Fake<UserSpecifications>();
            A.CallTo(() => _unitOfWork.Repository<User>()).Returns(_genericRepository);
            A.CallTo(() => _genericRepository.GetUserByUserName(spec))
                .Returns(Task.FromResult<User>(null));
            //Act
            var result = await _usersController.LogIn(logInDto);
            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(400, "invalid login"));
        }
        [Fact]
        public async Task UsersController_LogIn_ReturnBadRequest_WrongPassword()
        {

            //Arrange
            var logInDto = new LogInDto() { Username = "user", Password = "WrongPass" };
            var user = new User() { Username = "user", PasswordHash = "correctPass" };
            var spec = A.Fake<UserSpecifications>();
            A.CallTo(() => _unitOfWork.Repository<User>()).Returns(_genericRepository);
            A.CallTo(() => _genericRepository.GetUserByUserName(spec))
                .Returns(Task.FromResult(user));
            //Act
            var result = await _usersController.LogIn(logInDto);
            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new ApisResponse(400, "invalid login"));
        }
        [Fact]
        public async Task UsersController_Register_ReturnOk()
        {
            //Arrange
            RegisterDto registerDto = new RegisterDto() { Password = "Shehab123", Username = "userfdgfd", Role = UserRole.Admin.ToString() };
            User user = new User()
            {
                PasswordHash = registerDto.Password,
                Username = registerDto.Username,
                Role = UserRole.Admin,
            };
            A.CallTo(() => _unitOfWork.Repository<User>()).Returns(_genericRepository);
            A.CallTo(() => _genericRepository.Add(user));
            A.CallTo(() =>_unitOfWork.CompleteAsync()).Returns(Task.FromResult(1));
            //Act
            var result = await _usersController.Register(registerDto);
            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new UserDto()
                {
                    Username = user.Username,
                    Role = registerDto.Role.ToString(),
                    Token = "LogIn For Token"
                });

        }
        [Fact]
        public async Task UsersController_Register_ReturnBadRequest_RoleNotExist()
        {
            //Arrange
            RegisterDto registerDto = new RegisterDto() { Password = "Shehab123", Username = "userfdgfd", Role = "fakeRole" };
            //Act
            var result = await _usersController.Register(registerDto);
            //Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
         
        }
    }  
}
