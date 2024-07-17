using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Order_Management_System.Errors;
using Order_Management_System.Helpers;
using OrderSys.Core.Service.Contract;
using OrderSys.Repository.Data;
using OrderSys.Service.AuthService;
using OrderSys.Service.CustomerService;
using OrderSys.Service.InvoiceSerivce;
using OrderSys.Service.OrderService;
using OrderSys.Service.ProductService;
using System.Text;
using Talabat.Core;
using Talabat.Repository;

namespace Order_Management_System.Extentions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services)
        {

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            services.AddScoped(typeof(IProductService), typeof(ProductService));

            services.AddScoped(typeof(ICustomerService), typeof(CustomerService));

            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            services.AddScoped(typeof(IInvoiceService), typeof(InvoiceSerive));

            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                            .SelectMany(p => p.Value.Errors)
                                            .Select(p => p.ErrorMessage)
                                            .ToList();
                    var response = new ApisValidationErrors()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

        public static IServiceCollection AddAuthServicees(this IServiceCollection services, IConfiguration configuration)
        {
            //add auth service
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerOptions =>
                {
                    JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["jwt:validIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["jwt:validAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:AuthKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            //add DI for auth service to add token
            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            return services;
        }
    }
}
