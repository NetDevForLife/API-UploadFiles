using System;
using System.Threading.Tasks;
using API_UploadFiles.Models.InputModels;
using API_UploadFiles.Models.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_UploadFiles.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly ILogger<UploadController> logger;
        private readonly IUploadFilesService filesService;
        private readonly IWebHostEnvironment env;

        public UploadController(ILogger<UploadController> logger, IUploadFilesService filesService, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.filesService = filesService;
            this.env = env;
        }
        
        [HttpGet("Welcome")]
        public IActionResult Welcome()
        {
            logger.LogInformation("GET /api/Upload/Welcome called at {Time}", DateTime.UtcNow);
            var message = $"Ciao sono le ore: {DateTime.Now.ToLongTimeString()}";
            logger.LogInformation("Responding with message: {Message}", message);
            return Ok(new { message });
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles([FromForm] InputUploadFile model)
        {
            logger.LogInformation("POST /api/Upload/UploadFiles called at {Time}", DateTime.UtcNow);
            try
            {
                await filesService.UploadFileAsync(model, env);
                logger.LogInformation("File uploaded successfully at {Time}", DateTime.UtcNow);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading file at {Time}", DateTime.UtcNow);
                return StatusCode(500);
            }
        }
    }
}