using EmailSender.Database;
using EmailSender.Email;
using EmailSender.Models;
using EmailSender.OtpServices;
using Microsoft.EntityFrameworkCore;

namespace EmailSender
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder
          .Configuration.GetConnectionString("OtpConnection")));

            // Configure SMTP settings
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

            // Register EmailSender and OtpService services
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IOtpService, OtpService>();

            // Configure CORS to allow all origins, methods, and headers
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin() // Allows all origins
                          .AllowAnyMethod()  // Allows all methods (GET, POST, etc.)
                          .AllowAnyHeader(); // Allows all headers
                });
            });

            // Add services to the container.
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

            app.UseAuthorization();

            // Use the "AllowAll" CORS policy
            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
