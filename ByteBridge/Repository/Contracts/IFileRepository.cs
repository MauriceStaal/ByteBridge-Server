using ByteBridge.Entities;

namespace ByteBridge.Repository.Contracts
{
    public interface IFileRepository
    {
        Task<IEnumerable<Files>> GetFiles();
        Task<Files> GetFile(int FileId);
        Task<Files> CreateFile(Files file);
        Task<Files> UpdateFile(Files file);
        Task DeleteFile(int FileId);
    }
}
