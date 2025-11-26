using lentynaBackEnd.DTOs.Knygos;
using lentynaBackEnd.Models.Entities;

namespace lentynaBackEnd.Services.Interfaces
{
    public interface IOpenAIService
    {
        Task<List<Guid>> IeskoitKnyguPagalAprasymaAsync(string scenarijausAprasymas, List<KnygaSearchDto> knygos);
        Task<string> GeneruotiKnygosAtsiliepima(string knygosPavadinimas, List<Komentaras> komentarai);
    }
}
