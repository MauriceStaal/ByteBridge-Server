namespace ByteBridge.Models
{
    public class FileDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Path { get; set; }
        public required string Hash { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
