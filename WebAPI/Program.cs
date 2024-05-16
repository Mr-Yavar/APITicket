

using Contracts.IDapperRepository;
using Contracts.IService;
using Dapper;
using Dapper.DapperReporsitory;
using Entities;
using Entities.Mapper;
using LoggerService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using Repositories;
using Services;
using Services.Services;
using WebAPI;
using WebAPI.ActionFilters;
using WebAPI.Configuration;
using WebAPI.Extensions;
using AuthenticationService = Services.Services.AuthenticationService;
using IAuthenticationService = Contracts.IService.IAuthenticationService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


        var connectionString = builder.Configuration.GetConnectionString("sqlConnection");
        builder.Services.AddDbContext<DBContextProvider>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("WebAPI")));

        //builder.Services.AddControllers();
        builder.Services.ConfigureVersioning();
        builder.Services.AddControllers(config =>
        {
            config.RespectBrowserAcceptHeader = true;
            config.ReturnHttpNotAcceptable = true;
        }).AddXmlDataContractSerializerFormatters();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.ConfigureSwagger();
       builder.Services.AddEndpointsApiExplorer();
       builder.Services.AddSwaggerGen();
        builder.Services.ConfigureResponseCaching();
        builder.Services.ConfigureOutputCaching();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination");
            });
        });

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

        builder.Services.ConfigureCors();
        builder.Services.ConfigureIdentity();



        builder.Services.DapperInstallation();

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        //=====================Dependency Injection================
        builder.Services.AddScoped<ILoggerManager, LoggerManager>();

        builder.Services.RegisterServices();


        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

        //ActionFilter on level of controller and actions
        builder.Services.AddScoped<ValidationFilterAttribute>();
        //=========================================================


  

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseRouting();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapControllerRoute("default", "{controller=home}/{action=Index}/{id?}");

        //});

        app.UseCors("CorsPolicy");
        app.UseResponseCaching();
        app.UseOutputCache();
        app.UseRateLimiter();
        app.Use(async (context, next) =>
        {

            next.Invoke();
        });
        app.MapControllers();
        app.ConfigureExceptionHandler(new LoggerManager());
        //Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        app.Run();


    }
}
