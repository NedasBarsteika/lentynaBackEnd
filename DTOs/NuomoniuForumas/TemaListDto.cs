namespace lentynaBackEnd.DTOs.NuomoniuForumas
{
    public class TemaListDto
    {
        public Guid Id { get; set; }
        public string pavadinimas { get; set; } = string.Empty;
        public DateTime sukurimo_data { get; set; }
        public string autorius_slapyvardis { get; set; } = string.Empty;
    }
}
