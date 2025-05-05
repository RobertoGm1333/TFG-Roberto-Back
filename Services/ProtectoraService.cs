using System.Data.SqlClient;
using Models;
using ProtectoraAPI.Repositories;

namespace ProtectoraAPI.Services
{
    public class ProtectoraService : IProtectoraService
    {
        private readonly IProtectoraRepository _repository;

        public ProtectoraService(IProtectoraRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Protectora>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Protectora?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Protectora protectora)
        {
            await _repository.AddAsync(protectora);
        }

        public async Task UpdateAsync(Protectora protectora)
        {
            await _repository.UpdateAsync(protectora);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
