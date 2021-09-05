using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Blob.Manager.Logics;
using Test.Blob.Manager.Model;

namespace Test.Blob.Manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IFileManagerLogic _fileManagerLogic;

        public ImageController(IFileManagerLogic fileManagerLogic)
        {
            _fileManagerLogic = fileManagerLogic;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<GetImageOutputDto> Upload([FromForm] FileModel model)
        {
            if (model.MyFile != null)
            {
                var imgBytes = await _fileManagerLogic.Upload(model);
                return imgBytes;

                // return File(imgBytes, "image/webp");
            }
            return new GetImageOutputDto();
        }

        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> Get(string fileName)
        {
            var imgBytes = await _fileManagerLogic.Get(fileName);

            return File(imgBytes, "image/webp");
        }

        [Route("download")]
        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var imagBytes = await _fileManagerLogic.Get(fileName);
            return new FileContentResult(imagBytes, "application/octet-stream")
            {
                FileDownloadName = Guid.NewGuid().ToString() + ".webp",
            };
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _fileManagerLogic.Delete(fileName);
            return Ok();
        }


        [Route("getUrl")]
        [HttpGet]
        public string GetUrl(string fileName)
        {
            return _fileManagerLogic.GetUrl(fileName);
        }

    }
}