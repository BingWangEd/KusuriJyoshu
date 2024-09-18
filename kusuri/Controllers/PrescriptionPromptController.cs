using kusuri.Classes;
using kusuri.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionPromptController(
    AppDbContext appDbContext
) : ControllerBase
{
    private PrescriptionEditor editor = new PrescriptionEditor(appDbContext);

    [HttpPost]
    public async Task<IActionResult> AddPrescriptionPrompt(string content, int patientId, CancellationToken cancellationToken)
    {
        await editor.AddPrescriptionAsync(content, patientId, cancellationToken );
        return Ok("added prescription");
    }

    [HttpPost]
    public async Task<IActionResult> DeletePrescriptionPrompt(int id, CancellationToken cancellationToken)
    {
        await editor.DeletePrescriptionAsync(id, cancellationToken);
        return Ok("deleted prescription");
    }
}
