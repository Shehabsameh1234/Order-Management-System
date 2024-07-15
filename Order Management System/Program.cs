
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderSys.Repository.Data;
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

            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            builder.Services.AddDbContext<OrderManagementDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddControllers();
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

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
