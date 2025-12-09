using System.Text.Json;
using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;
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

        public async Task<string> GeneruotiKnygosAtsiliepima(string knygosPavadinimas, List<Komentaras> komentarai)
        {
            if (komentarai.Count == 0)
            {
                return "Kol kas nėra jokių atsiliepimų apie šią knygą. Būkite pirmas, kuris pasidalins savo nuomone!";
            }

            var komentaruSarasas = string.Join("\n\n", komentarai.Select((k, index) =>
                $"Atsiliepimas {index + 1}:\n" +
                $"Įvertinimas: {k.vertinimas}/5\n" +
                $"Komentaras: {k.komentaro_tekstas}"));

            var systemPrompt = @"Tu esi knygų kritikos ekspertas ir turi parašyti objektyvų, apibendrintą atsiliepimą apie knygą remiantis skaitytojų pateiktais atsiliepimais.

Tavo užduotis:
1. Išanalizuoti visus pateiktus skaitytojų atsiliepimus
2. Apibendrint pagrindinius knygos privalumus ir trūkumus, kuriuos mini skaitytojai
3. Parašyti trumpą (2-4 sakinių), informatyvų apibendrinimą lietuvių kalba
4. Atspindėti bendrą skaitytojų nuotaiką (pozityvi, neutrali, ar mišri)
5. Būti objektyviam ir nešališkam

Atsakymas turi būti tik tekstas, be jokių papildomų formatavimų ar įžangų.";

            var userPrompt = $@"Knyga: {knygosPavadinimas}

Skaitytojų atsiliepimai:
{komentaruSarasas}

Parašyk apibendrintą atsiliepimą apie šią knygą.";

            try
            {
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt)
                };

                var options = new ChatCompletionOptions
                {
                    Temperature = 0.5f,
                    MaxOutputTokenCount = 300
                };

                var response = await _chatClient.CompleteChatAsync(messages, options);
                var content = response.Value.Content[0].Text.Trim();

                _logger.LogInformation("Generated AI review for book: {BookTitle}", knygosPavadinimas);

                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating AI review for book: {BookTitle}", knygosPavadinimas);
                return "Atsiprašome, nepavyko sugeneruoti automatinio atsiliepimo. Bandykite vėliau.";
            }
        }

        public async Task<List<Guid>> Generuoti_Rekomendaciju_sarasaAsync(
            List<Irasas> perskaitytosKnygos,
            List<Knyga> kandidatuKnygos,
            IEnumerable<Guid>? praleistiKnyguIds = null)
        {
            var perskaitytos = perskaitytosKnygos
                .Where(i => string.Equals(i.tipas.ToString(), "skaityta", StringComparison.OrdinalIgnoreCase) && i.Knyga != null)
                .Select(i => i.Knyga!)
                .ToList();

            var praleisti = new HashSet<Guid>(praleistiKnyguIds ?? Enumerable.Empty<Guid>());
            var kandidatai = kandidatuKnygos
                .Where(k => !praleisti.Contains(k.Id))
                .ToList();

            if (perskaitytos.Count == 0 || kandidatai.Count == 0)
            {
                return new List<Guid>();
            }

            var perskaitytuSarasas = string.Join("\n\n", perskaitytos.Select(k =>
                $"{k.knygos_pavadinimas} | Autorius: {(k.Autorius != null ? $"{k.Autorius.vardas} {k.Autorius.pavarde}" : "Nenurodytas")} | Zanras: {k.Zanras?.pavadinimas ?? "Nenurodytas"} | Aprasymas: {Truncate(k.aprasymas, 180)}"));

            var kandidatuSarasas = string.Join("\n\n", kandidatai.Select(k =>
                $"ID: {k.Id}\n" +
                $"Pavadinimas: {k.knygos_pavadinimas}\n" +
                $"Autorius: {(k.Autorius != null ? $"{k.Autorius.vardas} {k.Autorius.pavarde}" : "Nenurodytas")}\n" +
                $"Zanras: {k.Zanras?.pavadinimas ?? "Nenurodytas"}\n" +
                $"Bestseleris: {(k.bestseleris ? "taip" : "ne")}\n" +
                $"Aprasymas: {Truncate(k.aprasymas, 180)}"));

            var praleidimoTekstas = praleisti.Count > 0 ? string.Join(", ", praleisti) : "nera";

            var systemPrompt = @"Tu esi personalizuotu knygu rekomendaciju generatorius.
Naudodamas perskaitytu knygu sarasa ir galimu kandidatu sarasa, parink iki 5 knygu, kurios labiausiai patiktu skaitytojui.
Neitrauk knygu, kuriu ID yra praleidimo sarase.
Atsakymas turi buti tik JSON:
{""bookIds"": [""guid1"", ""guid2""]}";

            var userPrompt = $@"Perskaitytos knygos:
{perskaitytuSarasas}

Knygos, kuriu nereikia rekomenduoti:
{praleidimoTekstas}

Galimi kandidatai:
{kandidatuSarasas}

Parink iki 5 labiausiai tinkančiu knygu. Grazink tik JSON su ID.";

            try
            {
                var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(systemPrompt),
                    new UserChatMessage(userPrompt)
                };

                var options = new ChatCompletionOptions
                {
                    Temperature = 0.35f,
                    MaxOutputTokenCount = 300
                };

                var response = await _chatClient.CompleteChatAsync(messages, options);
                var content = response.Value.Content[0].Text;
                _logger.LogInformation("AI recommendation raw response: {Content}", content);

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
                            .Select(Guid.Parse)
                            .ToList();
                    }
                }

                _logger.LogWarning("Failed to parse AI recommendation response: {Content}", content);
                return new List<Guid>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating recommendations via OpenAI");
                return new List<Guid>();
            }
        }

        private static string Truncate(string? text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "Nera aprasymo";
            }

            return text.Length <= maxLength
                ? text
                : text.Substring(0, maxLength) + "...";
        }

        private class AISearchResult
        {
            public List<string>? bookIds { get; set; }
        }
    }
}
