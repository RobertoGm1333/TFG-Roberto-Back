using Models;
using ProtectoraAPI.Repositories;

namespace ProtectoraAPI.Services
{
    public class SolicitudAdopcionService : ISolicitudAdopcionService
    {
        private readonly ISolicitudAdopcionRepository _repository;

        public SolicitudAdopcionService(ISolicitudAdopcionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SolicitudAdopcion>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<SolicitudAdopcion?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(SolicitudAdopcion solicitud)
        {
            await _repository.AddAsync(solicitud);
        }

        public async Task UpdateAsync(SolicitudAdopcion solicitud)
        {
            await _repository.UpdateAsync(solicitud);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
