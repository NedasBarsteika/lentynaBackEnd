using lentynaBackEnd.DTOs.Knygos;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<List<Guid>> IeskoitKnyguPagalAprasymaAsync(string scenarijausAprasymas, List<KnygaSearchDto> knygos);
    }
}
