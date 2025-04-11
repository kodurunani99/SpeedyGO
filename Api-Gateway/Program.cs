using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Api_Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()  // Allow any origin
                           .AllowAnyMethod()  // Allow any HTTP method
                           .AllowAnyHeader(); // Allow any header
                });
            });

            builder.Services.AddAuthorization();
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
               .AddJsonFile("API-Gateway-Config.json", false, reloadOnChange: true);
            builder.Services.AddOcelot();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Apply CORS policy globally before any other middleware
            app.UseCors("AllowAll");

            // Ensure Authorization is configured before Ocelot
            app.UseAuthorization();

            // Add Ocelot middleware
            app.UseOcelot().Wait(); // Await the Ocelot middleware to ensure it's properly initialized

            app.MapControllers();

            app.Run();
        }
    }
}
