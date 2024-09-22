using kusuri.Classes;
using kusuri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Text;

[ApiController]
[Route("api/[controller]")]
public class TryController(
    RedisService redisService
) : ControllerBase
{
    private EmbeddingManager manager = new EmbeddingManager();

    [HttpGet("/GetEmbeddings")]
    public async Task GetEmbeddings()
    {
        var embeddings = await manager.GetEmbeddings("hello world");
        Console.WriteLine($"embeddings: {embeddings.Length}");
        var res = await redisService.CreateHash(2, 1, "test prompt", embeddings);
    }

    [HttpGet("/CreateIndex")]
    public async Task CreateIndex()
    {
        var res = await redisService.CreateIndex(2);
    }

    [HttpGet("/TestAll")]
    public async Task TestAll()
    {
        var text1 = "hello world";
        var embeddings1 = await manager.GetEmbeddings(text1);
        var res1 = await redisService.CreateHash(2, 2, text1, embeddings1);

        var text2 = "chemistry class";
        var embeddings2 = await manager.GetEmbeddings(text2);
        var res2 = await redisService.CreateHash(2, 3, text2, embeddings2);

        var query = "hi world";
        var queryEmbeddings = await manager.GetEmbeddings(query);

        await redisService.FindClosestAsync(2, queryEmbeddings);
    }
}
