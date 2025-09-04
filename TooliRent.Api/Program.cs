
using TooliRent.Infrastructure.Data;
using TooliRent.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using TooliRent.Domain.Interfaces.Repositories;
using AutoMapper;
using TooliRent.Application.Interfaces.Services;
using TooliRent.Application.Services;
using TooliRent.Application.Mappings;
using FluentValidation;
using FluentValidation.AspNetCore;
using TooliRent.Application.Validators;


namespace TooliRent.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TooliRentDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IToolRepository, ToolRepository>();

            builder.Services.AddScoped<IToolService, ToolService>();

            builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfile));

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateToolDtoValidator>();
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


            app.MapControllers();

            app.Run();
        }
    }
}
