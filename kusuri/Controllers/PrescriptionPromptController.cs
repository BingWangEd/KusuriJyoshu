using kusuri.Classes;
using kusuri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

[Route("[controller]")]
public class PrescriptionPromptController(
    AppDbContext appDbContext,
    RedisService redisService,
    EmbeddingManager embeddingManager
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
                p.ModifiedAt.InZone(tokyoTimeZone).ToString("yyyy-MM-dd", null),
                p.PatientId
            ))
            .ToListAsync(cancellationToken);
        return history;
    }

    [HttpPost("edit/{patientId}/prompt/{id}")]
    public async Task<IActionResult> EditPrescriptionPrompt([FromBody] string content, int patientId, int id, CancellationToken cancellationToken)
    {
        var res = await editor.EditPrescriptionAsync(content, id, cancellationToken);
        if (!res)
        {
            return Ok("No change with prescription");
        }

        var embeddings = await embeddingManager.GetEmbeddings(content);
        await redisService.CreateHash(patientId, id, content, embeddings);

        return Ok("Edited prescription");
    }

    [HttpPost("add/{patientId}")]
    public async Task<IActionResult> AddPrescriptionPrompt([FromBody] string content, int patientId, CancellationToken cancellationToken)
    {
        var id = await editor.AddPrescriptionAsync(content, patientId, cancellationToken );
        var embeddings = await embeddingManager.GetEmbeddings(content);
        await redisService.CreateHash(patientId, id, content, embeddings);
        await redisService.CreateIndex(patientId);
        return Ok("added prescription");
    }

    [HttpPost("delete/{patientId}/prompt/{id}")]
    public async Task<IActionResult> DeletePrescriptionPrompt(int patientId, int id, CancellationToken cancellationToken)
    {
        await editor.DeletePrescriptionAsync(id, cancellationToken);
        await redisService.DeleteHash(patientId, id);
        return Ok("deleted prescription");
    }

    public record PrescriptionHistory(int Id, string Content, string Date, int PatientId);
}
