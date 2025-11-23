namespace lentynaBackEnd.Services.Interfaces
{
    public interface IMeteoService
    {
        Task<string> GetOroPrognozeAsync(DateTime data);
    }
}
