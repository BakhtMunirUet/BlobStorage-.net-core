using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Test.Blob.Manager.Model;

namespace Test.Blob.Manager.Logics
{
    public class FileManagerLogics : IFileManagerLogic
    {

        private readonly BlobServiceClient _blobServiceClient;

        public FileManagerLogics(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<GetImageOutputDto> Upload(FileModel model)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("name-of-container");

            string fileName = Guid.NewGuid().ToString();

            var blobClient = blobContainer.GetBlobClient(fileName);

            var header = new BlobHttpHeaders();
            header.ContentType = "image/png";


            await blobClient.UploadAsync(model.MyFile.OpenReadStream(), header);

            var blobClientSecond = blobContainer.GetBlobClient(fileName);

            var url = blobClientSecond.Uri.AbsoluteUri;


            return new GetImageOutputDto
            {
                ImageUrl = url
            };
        }


        public async Task<byte[]> Get(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("name-of-container");

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public async Task Delete(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("name-of-container");

            var blobClient = blobContainer.GetBlobClient(imageName);

            await blobClient.DeleteAsync();
        }

        public string GetUrl(string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("name-of-container");
            var blobClientSecond = blobContainer.GetBlobClient(fileName);

            return blobClientSecond.Uri.AbsoluteUri;

        }
    }
}