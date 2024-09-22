using Google.Cloud.AIPlatform.V1;

public class ChatSession
    {
        private readonly string _modelPath;
        private readonly PredictionServiceClient _predictionServiceClient;

        private readonly List<Content> _contents;

        public ChatSession(string modelPath, string location)
        {
            _modelPath = modelPath;

            _predictionServiceClient = new PredictionServiceClientBuilder
            {
                Endpoint = $"{location}-aiplatform.googleapis.com"
            }.Build();

            // Initialize contents to send over in every request.
            _contents = new List<Content>();
        }

        public async Task<string> SendMessageAsync(string prompt)
        {
            var content = new Content
            {
                Role = "USER",
                Parts =
                {
                    new Part { Text = prompt }
                }
            };
            _contents.Add(content);

            var generateContentRequest = new GenerateContentRequest
            {
                Model = _modelPath,
                GenerationConfig = new GenerationConfig
                {
                    Temperature = 0.9f,
                    TopP = 1,
                    TopK = 32,
                    CandidateCount = 1,
                    MaxOutputTokens = 2048
                }
            };
            generateContentRequest.Contents.AddRange(_contents);

            GenerateContentResponse response = await _predictionServiceClient.GenerateContentAsync(generateContentRequest);

            _contents.Add(response.Candidates[0].Content);

            return response.Candidates[0].Content.Parts[0].Text;
        }
    }