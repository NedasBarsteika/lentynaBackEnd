namespace lentynaBackEnd.DTOs.Common
{
    public class ErrorResponseDto
    {
        public int statusKodas { get; set; }
        public string zinute { get; set; } = string.Empty;
        public List<string>? klaidos { get; set; }

        public ErrorResponseDto(int statusCode, string message, List<string>? errors = null)
        {
            statusKodas = statusCode;
            zinute = message;
            klaidos = errors;
        }
    }
}
