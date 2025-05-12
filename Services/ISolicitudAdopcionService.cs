using Models;

namespace ProtectoraAPI.Services
{
    public interface ISolicitudAdopcionService
    {
        Task<List<SolicitudAdopcion>> GetAllAsync();
        Task<SolicitudAdopcion?> GetByIdAsync(int id);
        Task AddAsync(SolicitudAdopcion solicitud);
        Task UpdateAsync(SolicitudAdopcion solicitud);
        Task DeleteAsync(int id);
        Task<List<object>> GetSolicitudesByProtectoraAsync(int idProtectora);
    }
}
