
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Order_Management_System.Helpers;
using OrderSys.Core.Service.Contract;
using OrderSys.Repository.Data;
using OrderSys.Repository.DataSeeding;
using OrderSys.Service.AuthService;
using OrderSys.Service.CustomerService;
using OrderSys.Service.InvoiceSerivce;
using OrderSys.Service.OrderService;
using OrderSys.Service.ProductService;
using System.Text;
using Talabat.Core;
using Talabat.Repository;

namespace Order_Management_System
{
    public class Program
    {
        public static async Task  Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services
            // Add services to the container.

            builder.Services.AddControllers()
              .AddNewtonsoftJson(Options =>
              {
                  Options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
              });

            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            builder.Services.AddScoped(typeof(IProductService), typeof(ProductService));

            builder.Services.AddScoped(typeof(ICustomerService), typeof(CustomerService));

            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));

            builder.Services.AddScoped(typeof(IInvoiceService), typeof(InvoiceSerive));


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<OrderManagementDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //add auth service
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerOptions =>
                {
                    JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["jwt:validIssuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["jwt:validAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:AuthKey"])),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            //add DI for auth service to add token
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); 
            #endregion


            var app = builder.Build();

            #region Asking Clr To Generate Object From OrderManagementDbContext fro migrations
            //1-Create scope (using keyword => dispose the scope after using it )
            using var scope = app.Services.CreateScope();

            //2-Create service
            var service = scope.ServiceProvider;

            //3-generate object from StoreContext and _IdentityDbContext
            var _DbContext = service.GetRequiredService<OrderManagementDbContext>();
            
            
            //4- log the ex using loggerFactory Class and generate object from loggerFactory
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                //4-add migration
                await _DbContext.Database.MigrateAsync();

                //dataSeeding
                await DataSeed.SeedAsync(_DbContext);
                
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been occured during apply the migration");
            }

            #endregion


            #region MiddleWares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseAuthorization();

            app.UseAuthentication();
            #endregion

            app.Run();
        }
    }
}
