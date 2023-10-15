using BLL.DTO.Request;
using BLL.DTO.Response;
using BLL.Interfaces.Services;
using DAL.Pagination;
using DAL.Parameters;
using lab3.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace lab3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AircraftController : Controller
{

    private readonly IAircraftService _aircraftService;

    public AircraftController(IAircraftService aircraftService)
    {
        _aircraftService = aircraftService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedList<AircraftResponse>>> GetAsync(
        [FromQuery] AircraftParameters parameters)
    {
        var aircrafts = await _aircraftService.GetAsync(parameters);
        Response.Headers.Add("X-Pagination", aircrafts.SerializeMetadata());
        
        return Ok(aircrafts);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AircraftResponse>> GetByIdAsync([FromRoute] int id)
    {
        return Ok(await _aircraftService.GetByIdAsync(id));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> InsertAsync([FromBody] AircraftRequest request)
    {
        var aircraft = await _aircraftService.InsertAsync(request);

        return CreatedAtAction("GetById", new { id = aircraft.Id }, aircraft);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] AircraftRequest request)
    {
        await _aircraftService.UpdateAsync(request, id);
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        await _aircraftService.GetByIdAsync(id);
        await _aircraftService.DeleteAsync(id);
        
        return NoContent();
    }
    
}
