using kusuri.Classes;
using kusuri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Text;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionPromptController(
    AppDbContext appDbContext
) : ControllerBase
{
    private PrescriptionEditor editor = new PrescriptionEditor(appDbContext);
    private DateTimeZone tokyoTimeZone = DateTimeZoneProviders.Tzdb["Asia/Tokyo"];

    [HttpGet("history/{patientId}")]
    public async Task<List<PrescriptionHistory>> History(int patientId, CancellationToken cancellationToken)
    {
        var history = await appDbContext.PrescriptionPrompts
            .Where(
                p => p.PatientId == patientId && p.Status == Status.Active
            )
            .OrderBy(p => p.ModifiedAt)
            .Select(p => new PrescriptionHistory(
                p.Id,
                p.Content,
                p.ModifiedAt.InZone(tokyoTimeZone).ToString("yyyy-mm-dd", null),
                p.PatientId

            ))
            .ToListAsync(cancellationToken);
        return history;
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> EditPrescriptionPrompt([FromBody] string content, int id, CancellationToken cancellationToken)
    {
        await editor.EditPrescriptionAsync(content, id, cancellationToken);
        return Ok("Edited prescription");
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

    public record PrescriptionHistory(int Id, string Content, string Date, int PatientId);
}
