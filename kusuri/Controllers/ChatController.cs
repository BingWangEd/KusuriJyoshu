using kusuri.Classes;
using kusuri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;

[Route("[controller]")]
public class ChatController(
    AppDbContext appDbContext,
    RedisService redisService,
    EmbeddingManager embeddingManager
) : ControllerBase
{
    private DateTimeZone tokyoTimeZone = DateTimeZoneProviders.Tzdb["Asia/Tokyo"];

    [HttpPost("{patientId}")]
    public async Task<IActionResult> Chat([FromBody] string content, int patientId, CancellationToken cancellationToken)
    {
        var projectId = "my-project-1512957438502";
        var location = "us-central1";
        var publisher = "google";
        var model = "gemini-1.5-pro";
        var chatSession = new ChatSession($"projects/{projectId}/locations/{location}/publishers/{publisher}/models/{model}", location);

        var queryVector = await embeddingManager.GetEmbeddings(content);
        var context = await redisService.FindClosestAsync(patientId, queryVector);

        var edittedPrompt = $"This is the prescription related information of the patient: {context}; According to this information, {content}. Please answer in Japanese.";
        Console.WriteLine(edittedPrompt);
        var response = await chatSession.SendMessageAsync(edittedPrompt);

        var chatUser = new Chat{
            Content = content,
            PatientId = patientId,
            ByBot = false,
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
        };
        await appDbContext.AddAsync(chatUser);

        var chatBot = new Chat{
            Content = response,
            PatientId = patientId,
            ByBot = true,
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
        };
        await appDbContext.AddAsync(chatBot);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return Ok(response);
    }

    [HttpGet("GetHistory/{patientId}")]
    public async Task<List<ChatHistory>> GetHistory(int patientId, CancellationToken cancellationToken)
    {
        var chats = await appDbContext.Chats
            .Where(c => c.PatientId == patientId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new ChatHistory(
                c.Id,
                c.Content,
                c.PatientId,
                c.ByBot,
                c.CreatedAt.InZone(tokyoTimeZone).ToString("yyyy-MM-dd hh:mm", null)
            ))
            .ToListAsync(cancellationToken);
        return chats;
    }

    public record ChatHistory(int Id, string Content, int PatientId, bool ByBot, string CreatedAt);
}
