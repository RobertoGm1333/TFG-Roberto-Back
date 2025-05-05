using Models;

namespace ProtectoraAPI.Repositories
{
    public interface ISolicitudAdopcionRepository
    {
        Task<List<SolicitudAdopcion>> GetAllAsync();
        Task<SolicitudAdopcion?> GetByIdAsync(int id);
        Task AddAsync(SolicitudAdopcion solicitud);
        Task UpdateAsync(SolicitudAdopcion solicitud);
        Task DeleteAsync(int id);
    }
}
