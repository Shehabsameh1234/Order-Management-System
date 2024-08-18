using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Service.Contract;
using OrderSys.Core.Specifications.UserSpecifications;
using System.Text.Json.Serialization;
using Talabat.Core;


namespace Order_Management_System.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public UsersController(IUnitOfWork unitOfWork,IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("logIn")]
        public async Task<ActionResult<UserDto>> LogIn(LogInDto model)
        {
            var spec = new UserSpecifications(model.Username);
            var user = await _unitOfWork.Repository<User>().GetUserByUserName(spec);
            if(user  == null || user.PasswordHash != model.Password) return BadRequest(new ApisResponse(400,"invalid login"));
            return Ok(new UserDto() 
            { 
                Username = user.Username,
                Role=user.Role.ToString(),
                Token=await _authService.CreateTokenAsync(user)
            });
        }
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApisResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckUserNameExist(model.Username).Result)
                return BadRequest(new ApisResponse(400, "userName in use try another one"));

            if (model.Role !=UserRole.Admin.ToString() && model.Role !=UserRole.Customer.ToString())
                return BadRequest(new ApisResponse(400, "this role does not exist try another (Admin,Customer)"));

            var user = new User()
            { 
                Username=model.Username,
                PasswordHash=model.Password,
                Role= (UserRole)Enum.Parse(typeof(UserRole), model.Role),
            };
            _unitOfWork.Repository<User>().Add(user);

            await _unitOfWork.CompleteAsync();

            return Ok(new UserDto() 
            {  
                Username=user.Username,
                Role=model.Role.ToString(),
                Token="LogIn For Token"
            });
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async  Task<bool> CheckUserNameExist(string userName)
        {
            var spec = new UserSpecifications(userName);
            var boolean = await _unitOfWork.Repository<User>().CheckUserNameExsist(spec);
            return boolean ;
        }
    }
    
}
