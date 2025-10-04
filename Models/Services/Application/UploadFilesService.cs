using System;
using System.IO;
using System.Threading.Tasks;
using API_UploadFiles.Models.InputModels;
using API_UploadFiles.Models.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace API_UploadFiles.Models.Services.Application;

public class UploadFilesService() : IUploadFilesService
{
    public async Task UploadFileAsync([FromForm] InputUploadFile model, [FromServices] IWebHostEnvironment env)
    {
        var documenti = model.documenti;

        if (documenti == null || documenti.Count == 0)
        {
            return;
        }

        var now = DateTime.Now;
        var fileFolder = Path.Combine(env.ContentRootPath, "upload", now.ToString("yyyy"), now.ToString("MM"));

        Directory.CreateDirectory(fileFolder); // Safe to call even if exists

        foreach (var docs in documenti)
        {
            if (docs == null || docs.Length == 0)
            {
                continue;
            }

            var pathSaveDoc = Path.Combine(fileFolder, docs.FileName);

            // Use FileStream constructor for async support and better performance
            await using var fileStream = new FileStream(pathSaveDoc, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true);
            await docs.CopyToAsync(fileStream).ConfigureAwait(false);
        }
    }
}