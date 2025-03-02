using ByteBridge.Data;
using ByteBridge.Entities;
using ByteBridge.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ByteBridge.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDBContext dbContext;
        public FileRepository(AppDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Files> CreateFile(Files file)
        {
            var result = await dbContext.Files.AddAsync(file);
            await dbContext.SaveChangesAsync();

            return result == null ? throw new Exception("Failed to save file") : result.Entity;
        }

        public async Task DeleteFile(int FileId)
        {
            var file = dbContext.Files.FirstOrDefault(file => file.Id == FileId);
            if (file != null)
            {
                var result = dbContext.Files.Remove(file);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<Files> GetFile(int FileId)
        {
            var result = await dbContext.Files.FirstOrDefaultAsync(file => file.Id == FileId);
            return result ?? throw new Exception($"File with Id {FileId} not found");
        }

        public async Task<IEnumerable<Files>> GetFiles()
        {
            return await dbContext.Files.ToListAsync();
        }

        public async Task<Files> UpdateFile(Files file)
        {
            var existingFile = await dbContext.Files.FirstOrDefaultAsync(f => f.Id == file.Id);
            if (existingFile == null)
            {
                throw new Exception("File not found");
            }
            file.CreatedOn = existingFile.CreatedOn;
            file.UpdatedOn = DateTime.Now;

            dbContext.Update(file);
            await dbContext.SaveChangesAsync();
            return file;
        }
    }
}
