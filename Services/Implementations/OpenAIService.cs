using System.Text.Json;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Services.Interfaces;
using OpenAI.Chat;

namespace lentynaBackEnd.Services.Implementations
{
    public class OpenAIService : IOpenAIService
    {
        private readonly ChatClient _chatClient;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(ILogger<OpenAIService> logger)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("OPENAI_API_KEY environment variable is not set. Add it to .env file.");
            }

            _chatClient = new ChatClient(model, apiKey);
            _logger = logger;
        }

        public async Task<List<Guid>> IeskoitKnyguPagalAprasymaAsync(string scenarijausAprasymas, List<KnygaSearchDto> knygos)
        {
            if (knygos.Count == 0)
            {
                return new List<Guid>();
            }

            var knyguSarasas = string.Join("\n", knygos.Select(k =>
                $"ID: {k.Id}\n" +
                $"Pavadinimas: {k.Pavadinimas}\n" +
                $"Aprasymas: {k.Aprasymas ?? "Nera aprasymo"}\n" +
                $"Zanras: {k.Zanras}\n" +
                $"---"));

            var systemPrompt = @"Tu esi knygų rekomendacijų ekspertas. Tavo užduotis - pagal naudotojo pateiktą scenarijaus ar siužeto aprašymą rasti tinkamiausias knygas iš pateikto sąrašo.

Analizuok:
1. Knygos aprašymą ir kaip jis atitinka naudotojo aprašytą scenarijų
2. Žanrą - ar jis tinka naudotojo ieškomai temai

Grąžink JSON formatą su tinkamiausių knygų ID sąrašu, surikiuotų pagal tinkamumą (geriausiai tinkanti pirma).
Grąžink tik tas knygas, kurios tikrai atitinka paieškos kriterijus. Jei nėra tinkamų knygų, grąžink tuščią sąrašą.

Atsakymo formatas (tik JSON, be jokio papildomo teksto):
{""bookIds"": [""guid1"", ""guid2"", ""guid3""]}";

            var userPrompt = $@"Naudotojo ieškomas scenarijus/siužetas:
{scenarijausAprasymas}

Knygų sąrašas:
{knyguSarasas}

Rask tinkamiausias knygas pagal aprašytą scenarijų.";

            try
            {
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt)
                };

                var options = new ChatCompletionOptions
                {
                    Temperature = 0.3f,
                    MaxOutputTokenCount = 500
                };

                var response = await _chatClient.CompleteChatAsync(messages, options);
                var content = response.Value.Content[0].Text;

                // Parse JSON response
                var jsonStart = content.IndexOf('{');
                var jsonEnd = content.LastIndexOf('}');

                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var jsonString = content.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    var result = JsonSerializer.Deserialize<AISearchResult>(jsonString);

                    if (result?.bookIds != null)
                    {
                        return result.bookIds
                            .Where(id => Guid.TryParse(id, out _))
                            .Select(id => Guid.Parse(id))
                            .ToList();
                    }
                }

                _logger.LogWarning("Failed to parse AI response: {Content}", content);
                return new List<Guid>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenAI API");
                throw;
            }
        }

        private class AISearchResult
        {
            public List<string>? bookIds { get; set; }
        }
    }
}
