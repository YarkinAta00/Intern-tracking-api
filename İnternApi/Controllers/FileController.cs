using System;
using İnternApi.Data;
using Microsoft.AspNetCore.Mvc;
using İnternApi.Model;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace İnternApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public FileController(ApplicationDBContext applicationDBContext)
        {
            this._applicationDBContext = applicationDBContext;
        }

        [HttpPost("UploadFile")]
       async public Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("No file uploaded.");
            }

          
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                var fileData = memoryStream.ToArray();

              
                var fileEntity = new Files
                {
                    FileName = file.FileName,
                    FileData = fileData
                };

               
                _applicationDBContext.files.Add(fileEntity);
                await _applicationDBContext.SaveChangesAsync();


                return Ok(fileEntity.Id);
            }
        }

        [HttpGet("DownloadFile/{id}")]
        public IActionResult DownloadFile(int id)
        {
           
            var fileEntity =  _applicationDBContext.files.Where(x => x.Id == id).FirstOrDefault();
            if (fileEntity == null)
            {
                return NotFound();
            }

     
            var memoryStream = new MemoryStream(fileEntity.FileData);

           
            var contentType = "application/octet-stream";
            var fileName = fileEntity.FileName;

         
            return new FileContentResult(memoryStream.ToArray(), contentType)
            {
                FileDownloadName = fileName
            };
        }
        [HttpDelete("DeleteFile")]
        public IActionResult DeleteFile(int id)
        {
            var item = _applicationDBContext.files.Where(x => x.Id == id).FirstOrDefault();
            _applicationDBContext.files.Remove(item);
            _applicationDBContext.SaveChanges();
            return Ok("File deleted");
        }

        [HttpGet("DisplayFile/{id}")]
        public IActionResult DisplayFile(int id)
        {
          
            var fileEntity = _applicationDBContext.files.Where(x => x.Id == id).FirstOrDefault();
            if (fileEntity == null)
            {
                return NotFound();
            }


            var memoryStream = new MemoryStream(fileEntity.FileData, 0, fileEntity.FileData.Length);



            var contentType = GetContentType(fileEntity.FileName);

        
            return new FileStreamResult(memoryStream, contentType);
        }

        private string GetContentType(string fileName)
        {
           
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".pdf":
                    return "application/pdf";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }


    }

}


