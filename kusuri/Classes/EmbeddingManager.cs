using System.Collections.Immutable;
using System.Net.Http.Headers;
using System.Text;
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf;
using Newtonsoft.Json;
using Value = Google.Protobuf.WellKnownTypes.Value;

namespace kusuri.Classes;
public class EmbeddingManager
{
    private PredictionServiceClient predictionServiceClient;
    private EndpointName endpoint;
    public EmbeddingManager() {
        var projectId = "my-project-1512957438502";
        var model = "textembedding-gecko-multilingual@001";
        var locationId = "us-central1";
        var publisher = "google";
        predictionServiceClient = new PredictionServiceClientBuilder
        {
            Endpoint = $"{locationId}-aiplatform.googleapis.com"
        }.Build();
        endpoint = EndpointName.FromProjectLocationPublisherModel(projectId, locationId, publisher, model);
    }
    
    public async Task<byte[]> GetEmbeddings(string text)
    {
        var instances = new List<Value>
        {
            Value.ForStruct(new()
            {
                Fields =
                {
                    ["content"] = Value.ForString(text),
                }
            })
        };

        var response = await predictionServiceClient.PredictAsync(endpoint, instances, null);
        var values = response.Predictions.First().StructValue.Fields["embeddings"].StructValue.Fields["values"].ListValue.Values;

        var res = values.Select(v => v.NumberValue).ToArray();
        return GetBytesAlt(res);
    }

    private static byte[] GetBytesAlt(double[] values)
    {
        var result = new byte[values.Length * sizeof(double)];
        Buffer.BlockCopy(values, 0, result, 0, result.Length);
        return result;

    }
}
