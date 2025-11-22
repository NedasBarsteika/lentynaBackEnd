using lentynaBackEnd.Common;
using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IZanrasService
    {
        Task<IEnumerable<ZanrasDto>> GetAllAsync();
        Task<(Result Result, ZanrasDto? Zanras)> CreateAsync(string pavadinimas);
        Task<Result> DeleteAsync(Guid id);
    }
}
