using Microsoft.AspNetCore.Mvc;
using Storage.Exceptions;
using Storage.Services;

namespace Storage.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController: ControllerBase
{
    private StorageService _service;
    public StorageController(StorageService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public IActionResult GetEntries(string? path)
    {
        try
        {
            var files = _service.GetEntries(path);
            return Ok(files);
        }
        catch (ForbiddenException e)
        {
            return StatusCode(403, e.Message);
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file,string? path)
    {
        try
        {
            var filePath = await _service.UploadFile(file, path);
            return NoContent();
        }
        catch (ForbiddenException e)
        {
            return StatusCode(403, e.Message);
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}