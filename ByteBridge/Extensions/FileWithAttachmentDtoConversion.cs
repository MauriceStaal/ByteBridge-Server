using ByteBridge.Entities;
using ByteBridge.Models;

namespace ByteBridge.Extensions
{
    public static class FileWithAttachmentDtoConversion
    {
        public static FileWithAttachmentDto ToDto(this Files files, IFormFile fileAttachment)
        {
            return new FileWithAttachmentDto
            {
                Name = files.Name,
                FileAttachment = fileAttachment
            };
        }
    }
}
