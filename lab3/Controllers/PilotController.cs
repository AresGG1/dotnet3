using System.ComponentModel.DataAnnotations;
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
public class PilotController : Controller
{
    private readonly IPilotService _pilotService;

    public PilotController(IPilotService pilotService)
    {
        _pilotService = pilotService;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PagedList<PilotResponse>>> GetAsync(
        [FromQuery] PilotParameters parameters)
    {
        var pilots = await _pilotService.GetAsync(parameters);
        Response.Headers.Add("X-Pagination", pilots.SerializeMetadata());
        
        return Ok(pilots);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PilotResponse>> GetByIdAsync([FromRoute] int id)
    {
        return Ok(await _pilotService.GetByIdAsync(id));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> InsertAsync([FromBody] PilotRequest request)
    {
        var pilot = await _pilotService.InsertAsync(request);

        return CreatedAtRoute("GetByIdAsync", new {id = pilot.Id}, pilot);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateAsync([FromRoute] int id, [FromBody] PilotRequest request)
    {
        await _pilotService.UpdateAsync(request, id);
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        await _pilotService.DeleteAsync(id);
        
        return NoContent();
    }

    
    [HttpPost("assign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AssignPilot([FromBody] PilotAircraftRequest pilotAircraftRequest)
    { 
        await _pilotService.AssignPilot(pilotAircraftRequest);

        return NoContent();
    }
    
    [HttpPost("removeAssign")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RemoveAssign([FromBody] PilotAircraftRequest pilotAircraftRequest)
    {
        await _pilotService.RemoveAssignation(pilotAircraftRequest);

        return NoContent();
    }
    
}
