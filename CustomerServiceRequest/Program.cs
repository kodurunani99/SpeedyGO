using System.Text.Json.Serialization;
using CustomerServiceRequest.DBContext;
using CustomerServiceRequest.ServiceRequestRepository;
using CustomerServiceRequest.Services;
using Microsoft.EntityFrameworkCore;
using AuthenticationJWT;

namespace CustomerServiceRequest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAuthorization();

            // Add DBContext with SQL Server connection
            builder.Services.AddDbContext<ServiceDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RequestServiceDB"))
            );

            // Add Services and Repositories
            builder.Services.AddScoped<IServiceRequest, ServiceRequestService>();
            builder.Services.AddScoped<IServiceRequestRepo, ServiceRequestRepo>();
            builder.Services.AddJwtAuthentication();

            // Configure CORS to allow all origins, methods, and headers
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Allows all origins
                          .AllowAnyMethod()  // Allows all HTTP methods
                          .AllowAnyHeader(); // Allows all headers
                });
            });

            // Add services to the container and configure JSON options for enum as string
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Apply the "AllowAll" CORS policy
            app.UseCors("AllowAll");
            app.UseAuthentication();  // Ensure this is before UseAuthorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
