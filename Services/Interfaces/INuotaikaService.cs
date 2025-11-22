using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface INuotaikaService
    {
        Task<IEnumerable<NuotaikaDto>> GetAllAsync();
        Task<(Result Result, NuotaikaDto? Nuotaika)> CreateAsync(string pavadinimas);
        Task<Result> DeleteAsync(Guid id);
    }
}
