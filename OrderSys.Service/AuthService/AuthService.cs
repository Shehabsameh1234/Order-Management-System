using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderSys.Core.Entities;
using OrderSys.Core.Service.Contract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace OrderSys.Service.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            #region Claims
            var authClaims = new List<Claim>()
           {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role.ToString(),user.Role.ToString()),
           };
            #endregion

            #region AuthKey
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:AuthKey"]));
            #endregion

            #region Generate Token
            var token = new JwtSecurityToken(
                  audience: _configuration["jwt:validAudience"],
                  issuer: _configuration["jwt:validIssuer"],
                  expires: DateTime.Now.AddDays(double.Parse(_configuration["jwt:DurationInDays"])),
                  claims: authClaims,
                  signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                  );
            #endregion

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
