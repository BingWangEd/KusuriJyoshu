using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using NRedisStack.RedisStackCommands;
using NRedisStack.Search;
using NRedisStack.Search.Literals.Enums;
using StackExchange.Redis;

namespace kusuri.Classes;
public class RedisService
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisService(string connectionString)
    {
        // Initialize the connection to the Redis server
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }
    
    public async Task<bool> CreateHash(int patientId, int promptId, int chunkId, string prompt, byte[] embeddings)
    {
        var hashKey = $"{patientId}:{promptId}_{chunkId}";
        await _db.HashSetAsync(hashKey,
        [
            new HashEntry("prompt", prompt),
            new HashEntry("embedding", embeddings),
        ]);
        return true;
    }

    public async Task<bool> DeleteHash(int patientId, int promptId)
    {
        var chunkSize = await GetAsync($"{patientId}:{promptId}_keys") ?? "0";

        for (var i = 0; i < int.Parse(chunkSize); i++)
        {
            var hashKey = $"{patientId}:{promptId}_{i}";
            await _db.KeyDeleteAsync(hashKey);
        }
        
        return true;
    }

    // Create index on vector field to support vector similarity semantic search
    public async Task<bool> CreateIndex(int patientId)
    {
        // Define the schema with a TextField and VectorField
        var schema = new Schema()
            .AddTextField("prompt")
            .AddVectorField("embedding", Schema.VectorField.VectorAlgo.HNSW, new Dictionary<string, object>
            {
                { "TYPE", "FLOAT64" },
                { "DIM", 768 }, // All Vertex API models produce an output with 768 dimensions by default
                { "DISTANCE_METRIC", "COSINE" }
            });
        
        var ft = _db.FT();
        try
        {
            await ft.CreateAsync(
                $"{patientId}_prompt", // index name
                new FTCreateParams().On(IndexDataType.HASH).Prefix($"{patientId}:"), // parameters
                schema
            );

            Console.WriteLine($"Index '{patientId}_prompt' created successfully");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Index '{patientId}_prompt' already exists or another error occurred: " + e.Message);
        }
        
        return false;
    }

    public async Task<bool> DeleteIndex(int patientId)
    {
        var ft = _db.FT();
        var indexName = $"{patientId}_prompt";

        try
        {
            await ft.DropIndexAsync(indexName);

            Console.WriteLine($"Index '{patientId}_prompt' deleted successfully");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to delete Index '{patientId}_prompt': " + e.Message);
        }
        
        return false;
    }

    public async Task<string> FindClosestAsync(int patientId, byte[] queryVector)
    {
        var ft = _db.FT();
        var indexName = $"{patientId}_prompt";
        var baseQuery = "*=>[KNN 5 @embedding $vector AS vector_score]"; // return 5 results
        var query = new Query(baseQuery)
            .AddParam("vector", queryVector)
            .ReturnFields("prompt", "vector_score")
            .SetSortBy("vector_score")
            .Dialect(2);
        var res = await ft.SearchAsync(indexName, query);

        Console.WriteLine($"found closest: {res}");

        string texts = "";
        foreach (var doc in res.Documents) {
            Console.Write($"id: {doc.Id}, ");

            foreach (var item in doc.GetProperties()) {
                Console.Write($" {item.Value}");
            }
        }

        foreach (var doc in res.Documents) {
            foreach (var item in doc.GetProperties()) {
                if (item.Key == "prompt")
                {
                    texts += item.Value;
                    texts += "\n";
                }
            }
        }

        return texts;
    }

    // Method to index (store) data in Redis
    public async Task<bool> SetAsync(string key, int value, TimeSpan? expiry = null)
    {
        try
        {
            // Store the key-value pair in Redis with optional expiration
            return await _db.StringSetAsync(key, value, expiry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting value in Redis: {ex.Message}");
            return false;
        }
    }

    // Method to read (retrieve) data from Redis
    public async Task<string?> GetAsync(string key)
    {
        try
        {
            // Retrieve the value from Redis for the given key
            return await _db.StringGetAsync(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting value from Redis: {ex.Message}");
            return null;
        }
    }

    // Optional: Check if a key exists in Redis
    public async Task<bool> KeyExistsAsync(string key)
    {
        try
        {
            // Check if the key exists in Redis
            return await _db.KeyExistsAsync(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking key existence in Redis: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> KeyDeleteAsync(string key)
    {
        try
        {
            return await _db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting key {key} in Redis: {ex.Message}");
            return false;
        }
    }
}
