using System.Data.SqlClient;
using Models;
using ProtectoraAPI.Repositories;

namespace ProtectoraAPI.Services
{
    public class DeseadoService : IDeseadoService
    {
        private readonly IDeseadoRepository _repository;

        public DeseadoService(IDeseadoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Deseado>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Deseado?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Deseado deseado)
        {
            await _repository.AddAsync(deseado);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
