using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test.Blob.Manager.Model;

namespace Test.Blob.Manager.Logics
{
    public interface IFileManagerLogic
    {
        Task<GetImageOutputDto> Upload(FileModel model);
        Task<byte[]> Get(string imageName);
        Task Delete(string imageName);

        string GetUrl(string fileName);
    }
}