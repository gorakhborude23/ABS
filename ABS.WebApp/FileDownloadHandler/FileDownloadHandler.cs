using ABS.FileGeneration;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ABS.WebApp.FileDownloadHandler
{
    public class FileDownloadHandler : HttpTaskAsyncHandler
    {
        private readonly FileGenerationService _fileGenerationService;

        public FileDownloadHandler()
        {
            _fileGenerationService = new FileGenerationService();
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=employees.xlsx");

            try
            {
                // Generate the file and get the stream
                using (var fileStream = await _fileGenerationService.GenerateFile())
                {
                    // Check if the file stream is valid
                    if (fileStream == null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.Write("Failed to generate the file.");
                        return;
                    }

                    // Copy the file stream to the response output stream
                    await fileStream.CopyToAsync(context.Response.OutputStream);
                    context.Response.Flush();
                }
                // unmanaged resources memory released gracefully and created file deleted after read to save the disk space.
            }
            catch (UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.Write("Error: Access to the file is denied. Please check the file permissions.");
            }
            catch (DirectoryNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Write("Error: The specified directory was not found.");
            }
            catch (PathTooLongException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Write("Error: The specified path is too long.");
            }
            catch (IOException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Write("Error: A general file I/O error occurred. Please check disk space and ensure the file is not locked.");
            }
            catch (NotSupportedException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Write("Error: The file path provided is invalid.");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.Write($"Error: {ex.Message}");
            }
        }
        public override bool IsReusable => false;
    }

}