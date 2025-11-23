using System.Text.Json;
using lentynaBackEnd.Services.Interfaces;

namespace lentynaBackEnd.Services.Implementations
{
    public class MeteoService : IMeteoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MeteoService> _logger;
        private const string BaseUrl = "https://api.meteo.lt/v1";
        private const string PlaceCode = "kaunas"; // KTU miestelis - Kaunas

        public MeteoService(HttpClient httpClient, ILogger<MeteoService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetOroPrognozeAsync(DateTime data)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/places/{PlaceCode}/forecasts/long-term");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Meteo API returned {StatusCode}", response.StatusCode);
                    return GenerateFallbackWeather(data);
                }

                var json = await response.Content.ReadAsStringAsync();
                var forecast = JsonSerializer.Deserialize<MeteoForecastResponse>(json);

                if (forecast?.forecastTimestamps == null || forecast.forecastTimestamps.Count == 0)
                {
                    return GenerateFallbackWeather(data);
                }

                // Find forecast closest to the meeting date (around noon)
                var targetTime = data.Date.AddHours(12);
                var closestForecast = forecast.forecastTimestamps
                    .OrderBy(f => Math.Abs((DateTime.Parse(f.forecastTimeUtc) - targetTime).TotalHours))
                    .FirstOrDefault();

                if (closestForecast == null)
                {
                    return GenerateFallbackWeather(data);
                }

                var conditionLt = TranslateCondition(closestForecast.conditionCode ?? "");
                var recommendation = GetRecommendation(closestForecast.conditionCode ?? "", closestForecast.airTemperature);

                return $"KTU miestelis ({data:yyyy-MM-dd}): {closestForecast.airTemperature}°C, {conditionLt}. " +
                       $"Vėjas: {closestForecast.windSpeed} m/s. {recommendation}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather from Meteo.lt API");
                return GenerateFallbackWeather(data);
            }
        }

        private static string TranslateCondition(string conditionCode)
        {
            return conditionCode?.ToLower() switch
            {
                "clear" => "Giedra",
                "partly-cloudy" => "Mažai debesuota",
                "cloudy-with-sunny-intervals" => "Debesuota su pragiedruliais",
                "cloudy" => "Debesuota",
                "thunder" => "Perkūnija",
                "isolated-thunderstorms" => "Vietomis perkūnija",
                "thunderstorms" => "Audra",
                "heavy-rain-with-thunderstorms" => "Liūtis su perkūnija",
                "light-rain" => "Nedidelis lietus",
                "rain" => "Lietus",
                "heavy-rain" => "Smarkus lietus",
                "light-sleet" => "Nedidelė šlapdriba",
                "sleet" => "Šlapdriba",
                "freezing-rain" => "Lijundra",
                "hail" => "Kruša",
                "light-snow" => "Nedidelis sniegas",
                "snow" => "Sniegas",
                "heavy-snow" => "Smarkus sniegas",
                "fog" => "Rūkas",
                "null" or null => "Nežinoma",
                _ => conditionCode ?? "Nežinoma"
            };
        }

        private static string GetRecommendation(string conditionCode, double temperature)
        {
            var isRainy = conditionCode?.ToLower() switch
            {
                "light-rain" or "rain" or "heavy-rain" or "thunderstorms" or
                "heavy-rain-with-thunderstorms" or "sleet" or "light-sleet" or
                "freezing-rain" or "hail" => true,
                _ => false
            };

            var isSnowy = conditionCode?.ToLower() switch
            {
                "light-snow" or "snow" or "heavy-snow" => true,
                _ => false
            };

            if (isRainy || isSnowy)
            {
                return "Rekomenduojame susitikti viduje.";
            }

            if (temperature < 5)
            {
                return "Šalta - rekomenduojame susitikti viduje.";
            }

            if (temperature > 25)
            {
                return "Šilta - galima susitikti lauke, ieškokite pavėsio.";
            }

            return "Galima susitikti lauke.";
        }

        private static string GenerateFallbackWeather(DateTime date)
        {
            return $"KTU miestelis ({date:yyyy-MM-dd}): Orų prognozė laikinai nepasiekiama. " +
                   "Patikrinkite meteo.lt prieš susitikimą.";
        }

        // API Response DTOs
        private class MeteoForecastResponse
        {
            public MeteoPlace? place { get; set; }
            public string? forecastType { get; set; }
            public string? forecastCreationTimeUtc { get; set; }
            public List<ForecastTimestamp> forecastTimestamps { get; set; } = new();
        }

        private class MeteoPlace
        {
            public string? code { get; set; }
            public string? name { get; set; }
            public string? administrativeDivision { get; set; }
            public string? country { get; set; }
            public string? countryCode { get; set; }
        }

        private class ForecastTimestamp
        {
            public string forecastTimeUtc { get; set; } = string.Empty;
            public double airTemperature { get; set; }
            public double windSpeed { get; set; }
            public double windGust { get; set; }
            public string? conditionCode { get; set; }
            public double cloudCover { get; set; }
            public double seaLevelPressure { get; set; }
            public double relativeHumidity { get; set; }
            public double totalPrecipitation { get; set; }
        }
    }
}
