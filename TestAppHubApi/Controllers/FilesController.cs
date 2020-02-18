using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.Services;
using TestAppApi.Models;

namespace TestAppApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [AllowAnonymous]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileDetailsVm))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get(long id)
        {
            FileDetailsVm vm = await _fileService.Get(id);
            if (vm == null)
            {
                return NotFound();
            }

            return Ok(vm);
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            try
            {
                var vm = GetFileFromRequest(file);
                var id = await _fileService.Upload(vm);
                return Ok();
            }
            catch (Exception ex)
            {
                return SendError("Could not upload file", ex);
            }
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download()
        {
            byte[] fileData = new byte[0];
            return File(fileData, "application/text", fileDownloadName: "test.txt");
        }

        private FileVm GetFileFromRequest(IFormFile file)
        {
            if (Request.Form.Files.Count == 0)
            {
                throw new System.ArgumentException("Files not found");
            }

            file = Request.Form.Files[0];

            string fileName = file.FileName;
            string contentType = file.ContentType;
            byte[] content;

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                content = stream.ToArray();
            }

            FileVm fileModel = new FileVm
            {
                Content = content,
                ContentType = contentType,
                FileName = fileName,
            };

            return fileModel;
        }

        private ObjectResult SendError(string message, Exception ex = null, int status = 500)
        {
            ErrorModelInfo errorDetails = new ErrorModelInfo()
            {
                Message = message,
                ErrorObject = ex?.Message,
                StatusCode = status,

            };

            return StatusCode(status, errorDetails);
        }
    }
}