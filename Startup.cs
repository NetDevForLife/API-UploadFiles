using System.Globalization;
using API_UploadFiles.Models.Services.Application;
using API_UploadFiles.Models.Services.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API_UploadFiles;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API_UploadFiles", Version = "v1" });
        });

        services.AddTransient<IUploadFilesService, UploadFilesService>();

        // Enable CORS for all origins, methods, and headers
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        // Always enable Swagger and DeveloperExceptionPage for Codespaces/local dev
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API_UploadFiles v1"));

        // Enable CORS before routing
        app.UseCors("AllowAll");

        CultureInfo appCulture = new("it-IT");

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(appCulture),
            SupportedCultures = new[] { appCulture }
        });

        // Only use HTTPS redirection if both HTTP and HTTPS are enabled
        // Commented out to avoid errors when running only on HTTP
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}