using kusuri.Classes;
using kusuri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionPromptController(
    AppDbContext appDbContext
) : ControllerBase
{
    private PrescriptionEditor editor = new PrescriptionEditor(appDbContext);

    [HttpGet("history/{patientId}")]
    public async Task<List<PrescriptionPrompt>> History(int patientId, CancellationToken cancellationToken)
    {
        var history = await appDbContext.PrescriptionPrompts.Where(p => p.PatientId == patientId).ToListAsync(cancellationToken);
        return history;
    }

    [HttpPost("add/{patientId}")]
    public async Task<IActionResult> AddPrescriptionPrompt([FromBody] string content, int patientId, CancellationToken cancellationToken)
    {
        await editor.AddPrescriptionAsync(content, patientId, cancellationToken );
        return Ok("added prescription");
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> DeletePrescriptionPrompt(int id, CancellationToken cancellationToken)
    {
        await editor.DeletePrescriptionAsync(id, cancellationToken);
        return Ok("deleted prescription");
    }
}
