using ByteBridge.Entities;
using ByteBridge.Models;

namespace ByteBridge.Extensions
{
    public static class FileDtoConversion
    {
        public static FileDto ToDto(this Files files)
        {
            return new FileDto
            {
                Id = files.Id,
                Name = files.Name,
                Path = files.Path,
                Hash = files.Hash,
                UpdatedOn = files.UpdatedOn,
                CreatedOn = files.CreatedOn,
            };
        }
        public static Files FromDto(this FileDto fileDto)
        {
            return new Files
            {
                Id = fileDto.Id,
                Name = fileDto.Name,
                Path = fileDto.Path,
                Hash = fileDto.Hash,
                UpdatedOn = fileDto.UpdatedOn,
                CreatedOn = fileDto.CreatedOn,
            };
        }
    }
}
