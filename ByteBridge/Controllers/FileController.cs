﻿using ByteBridge.Extensions;
using ByteBridge.Models;
using ByteBridge.Repository.Contracts;
using ByteBridge.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ByteBridge.Entities;
using Microsoft.OpenApi;

namespace ByteBridge.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFileAttachmentRepository _fileAttachmentRepository;
        private readonly int apiVersion;
        public FileController(IFileRepository fileRepository, IFileAttachmentRepository fileAttachmentRepository, IConfiguration config)
        {
            _fileRepository = fileRepository;
            _fileAttachmentRepository = fileAttachmentRepository;
            apiVersion = int.Parse(config["APIVersionManagement:Version"]!);
        }

        [HttpGet, MapToApiVersion("1.0")]
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

        [HttpGet("{id}"), MapToApiVersion("1.0")]
        public async Task<ActionResult> Get(int id)
        {
            try
            {
                var file = await _fileRepository.GetFile(id);
                if (file == null)
                {
                    return NotFound();
                }
                if (!System.IO.File.Exists(file.Path))
                {
                    return NotFound("File not found on server");
                }

                var fileStream = new FileStream(file.Path, FileMode.Open, FileAccess.Read);
                return File(fileStream, "application/octet-stream", file.Name);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost, MapToApiVersion("1.0")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<FileDto>> Post([FromForm] FileWithAttachmentDto file)
        {
            try
            {
                if (file.FileAttachment == null || file.FileAttachment.Length == 0)
                {
                    return BadRequest("File attachment is required!");
                }

                var filePath = await _fileAttachmentRepository.UploadFile(file.Name, file.FileAttachment.OpenReadStream());

                var fileHash = await HashFileAttachmentHelper.CalculateFileHashAsync(file.FileAttachment.OpenReadStream());

                var fileRecord = new FileDto
                {
                    Name = file.Name,
                    Path = filePath,
                    Hash = fileHash,
                };

                var result = await _fileRepository.CreateFile(fileRecord.FromDto());

                return result.ToDto();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}"), MapToApiVersion("1.0")]
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

        [HttpDelete("{id}"), MapToApiVersion("1.0")]
        public async Task<ActionResult> Delete(int id)
        {
                try
                {
                var result = await _fileRepository.GetFile(id);
                if (result != null)
                {
                    await _fileAttachmentRepository.DeleteFile(result.Path);
                }
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