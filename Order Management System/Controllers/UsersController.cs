using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Order_Management_System.Dtos;
using Order_Management_System.Errors;
using OrderSys.Core.Entities;
using OrderSys.Core.Entities.Enums;
using OrderSys.Core.Service.Contract;
using OrderSys.Core.Specifications.UserSpecifications;
using Talabat.Core;
using Talabat.Repository;

namespace Order_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;

        public UsersController(IUnitOfWork unitOfWork,IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExist(model.Username).Result.Value)
                return NotFound(new ApisResponse(400, "userName in use try another one"));

            if (model.Role !=UserRole.Admin.ToString() && model.Role !=UserRole.Customer.ToString())
                return NotFound(new ApisResponse(400, "this role does not exist try another (Admin,Customer)"));

            var user = new User()
            { 
                Username=model.Username,
                PasswordHash=model.PasswordHash,
                Role= (UserRole)Enum.Parse(typeof(UserRole), model.Role),
            };
            _unitOfWork.Repository<User>().Add(user);

            await _unitOfWork.CompleteAsync();

            return Ok(new UserDto() 
            {  
                Username=user.Username,
                PasswordHash=user.PasswordHash,
                Role=model.Role.ToString(),
            });
        }
        [HttpGet("emailexist")]
        public async  Task<ActionResult<bool>> CheckEmailExist(string userName)
        {
            var spec = new UserSpecifications(userName);
            return await _unitOfWork.Repository<User>().CheckUserNameExsist(spec);
        }


    }
    
}
