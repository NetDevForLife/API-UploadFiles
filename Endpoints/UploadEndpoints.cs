using System;
using System.Threading.Tasks;
using API_UploadFiles.Extensions;
using API_UploadFiles.Models.InputModels;
using API_UploadFiles.Models.Services.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace API_UploadFiles.Endpoints;

public class UploadEndpoints : IEndpointRouteHandler
{
    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var emailApiGroup = endpoints
            .MapGroup("/api/upload")
            .WithOpenApi();

        emailApiGroup.MapGet("/welcome", GetWelcomeMessage)
            .WithDescription("Endpoint for welcome message");

        emailApiGroup.MapPost("/UploadFiles", UploadFiles)
            .DisableAntiforgery()
            .WithDescription("Endpoint for uploading files");
    }

    private static string GetWelcomeMessage(ILogger<UploadEndpoints> logger)
    {
        logger.LogInformation("GET /api/Upload/Welcome called at {Time}", DateTime.UtcNow);
        var message = $"Ciao sono le ore: {DateTime.Now.ToLongTimeString()}";
        logger.LogInformation("Responding with message: {Message}", message);
        return message;
    }

    private static async Task<Results<Ok, InternalServerError>> UploadFiles(
        ILogger<UploadEndpoints> logger,
        IUploadFilesService filesService,
        IWebHostEnvironment env,
        InputUploadFile model)
    {
        logger.LogInformation("POST /api/Upload/UploadFiles called at {Time}", DateTime.UtcNow);
        
        try
        {
            await filesService.UploadFileAsync(model, env);
            logger.LogInformation("File uploaded successfully at {Time}", DateTime.UtcNow);
            
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error uploading file at {Time}", DateTime.UtcNow);
            return TypedResults.InternalServerError();
        }
    }
}