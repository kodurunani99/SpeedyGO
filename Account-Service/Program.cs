using Account_Service.Data;
using Account_Service.IRepository;
using Account_Service.Models;
using Account_Service.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using AuthenticationJWT;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Account_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create a new WebApplicationBuilder instance
            var builder = WebApplication.CreateBuilder(args);

            // Add controllers to the service collection
            builder.Services.AddControllers();

            // Configure Entity Framework to use SQL Server with a connection string from configuration
            builder.Services.AddDbContext<AccountDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection")));

            // Add Identity services for ApplicationUser and IdentityRole
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AccountDBContext>() // Use Entity Framework to store identity data
                .AddDefaultTokenProviders(); // Add default token providers for password reset, etc.

            // Add JWT Authentication extension method
            builder.Services.AddJwtAuthentication();

            // Register the AccountRepository to the IAccountRepository interface for dependency injection
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            // Add services for API endpoint exploration
            builder.Services.AddEndpointsApiExplorer();
            
            // Add Swagger for API documentation
            builder.Services.AddSwaggerGen();

            // Add authorization services
            builder.Services.AddAuthorization();

            // Add CORS policy with a policy named "AllowAll" that allows any origin, method, and header
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Build the application
            var app = builder.Build();

            // Configure HTTP request pipeline for development environment
            if (!app.Environment.IsDevelopment())
            {
            }
            else
            {
                //    // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                //    // Enable middleware to serve swagger-ui specifying the Swagger JSON endpoint
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                options.RoutePrefix = string.Empty; // Swagger at root
            });
            // Enforce HTTPS redirection
            app.UseHttpsRedirection();
            
            // Add routing middleware
            app.UseRouting();

            // Apply CORS policy named "AllowAll" after routing
            app.UseCors("AllowAll");

            // Add authentication middleware to the application pipeline
            app.UseAuthentication();

            // Add authorization middleware to the application pipeline
            app.UseAuthorization();

            // Map controller routes for attribute routing
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}

