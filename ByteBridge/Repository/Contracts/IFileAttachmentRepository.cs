using ByteBridge.Entities;
using System.Net;

namespace ByteBridge.Repository.Contracts
{
    public interface IFileAttachmentRepository
    {
        Task<string> UploadFile(string fileName, Stream fileAttachment);
        Task<Stream> DownloadFile(string filePath);
        Task<string> DeleteFile(string filePath);
    }
}