using ByteBridge.Extensions;
using ByteBridge.Models;
using ByteBridge.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Reflection.Metadata.Ecma335;


namespace ByteBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        public FileController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileDto>>> Get()
        {
            try
            {
                var files = await _fileRepository.GetFiles();
                if (files.IsNullOrEmpty())
                {
                    return NoContent();
                }
                return Ok(files.Select(file => file.ToDto()));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
 
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FileDto>> Get(int id)
        {
            try
            {
                var file = await _fileRepository.GetFile(id);
                if (file == null)
                {
                    return NotFound();
                }
                return Ok(file.ToDto());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FileDto>> Post(FileDto file)
        {
            try
            {
                var result = await _fileRepository.CreateFile(file.FromDto());
                return result.ToDto();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FileDto>> Update(FileDto file)
        {
            try
            {
                var result = await _fileRepository.UpdateFile(file.FromDto());
                return result.ToDto();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
                try
                {
                    await _fileRepository.DeleteFile(id);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
        }
    }
}