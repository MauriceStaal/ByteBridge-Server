namespace ByteBridge.Models
{
    public class FileWithAttachmentDto
    {
        public required string Name { get; set; }
        public required IFormFile FileAttachment { get; set; }
    }
}
