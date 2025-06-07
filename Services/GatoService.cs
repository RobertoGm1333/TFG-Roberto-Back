using System.Data.SqlClient;
using Models;
using ProtectoraAPI.Repositories;

namespace ProtectoraAPI.Services
{
    public class GatoService : IGatoService
    {
        private readonly IGatoRepository _repository;

        public GatoService(IGatoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Gato>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Gato?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Gato gato)
        {
            await _repository.AddAsync(gato);
        }

        public async Task UpdateAsync(Gato gato)
        {
            await _repository.UpdateAsync(gato);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
