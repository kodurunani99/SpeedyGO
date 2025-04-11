

using Microsoft.EntityFrameworkCore;
using Quotation_Service.Data;
using Quotation_Service.IRepository;
using Quotation_Service.Repository;
using System.Text.Json.Serialization;
using TransportQuotation_Service.IRepository;
using TransportQuotation_Service.Repository;
using AuthenticationJWT;

namespace Quotation_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<QuotationDBContext>(options => options.UseSqlServer(builder
                .Configuration.GetConnectionString("QuotationDB")));
            builder.Services.AddScoped<IQuoteRepository, QuotationRepository>();
            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            //adding jwt extension method
            builder.Services.AddJwtAuthentication();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                 });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAllOrigins");
            app.UseAuthentication();  // Ensure this is before UseAuthorization
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
