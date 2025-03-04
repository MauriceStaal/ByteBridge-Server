using ByteBridge.Entities;
using ByteBridge.Repository.Contracts;

namespace ByteBridge.Repository
{
    public class FileAttachmentRepository : IFileAttachmentRepository
    {
        private readonly string _storagePath;

        public FileAttachmentRepository(IConfiguration config)
        {
            // Initialize the storage path from configuration or use the default path
            _storagePath = config["FileStorage:Path"] ?? "C:\\FileUploads";
        }

        public Task<string> DeleteFile(string filePath)
        {
            try
            {
                // Check if the file exists before attempting to delete
                if (File.Exists(filePath))
                {
                    File.Delete(filePath); // Delete the file
                    return Task.FromResult("File deleted successfully.");
                }
                else
                {
                    // Return a message if the file is not found
                    return Task.FromResult("File not found.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during file deletion
                throw new IOException("Error while deleting the file.", ex);
            }
        }

        public async Task<Stream> DownloadFile(string filePath)
        {
            // Check if the file exists before attempting to open
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File attachment not found.", filePath);

            try
            {
                // Open the file for reading and return the stream
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return await Task.FromResult(fileStream);
            }
            catch (Exception ex)
            {
                // Handle any errors encountered while opening the file
                throw new IOException("Error while opening the file.", ex);
            }
        }

        public async Task<string> UploadFile(string fileName, Stream fileAttachment)
        {
            try
            {
                // Generate a unique file name and determine the upload path
                var uploadPath = Path.Combine(_storagePath, $"{Guid.NewGuid()}_{fileName}");

                // Ensure the storage directory exists, create if necessary
                Directory.CreateDirectory(_storagePath);

                // Save the uploaded file to the specified path
                await using var output = new FileStream(uploadPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await fileAttachment.CopyToAsync(output);

                // Return the path where the file is saved
                return uploadPath;
            }
            catch (Exception ex)
            {
                // Handle any errors during the file upload process
                throw new IOException("Failed to save file attachment.", ex);
            }
        }
    }
}
