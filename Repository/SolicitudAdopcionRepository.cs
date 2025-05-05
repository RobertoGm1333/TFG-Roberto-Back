using Microsoft.Data.SqlClient;
using Models;

namespace ProtectoraAPI.Repositories
{
    public class SolicitudAdopcionRepository : ISolicitudAdopcionRepository
    {
        private readonly string _connectionString;

        public SolicitudAdopcionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<SolicitudAdopcion>> GetAllAsync()
        {
            var solicitudes = new List<SolicitudAdopcion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM SolicitudAdopcion";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var solicitud = new SolicitudAdopcion
                            {
                                Id_Solicitud = reader.GetInt32(0),
                                Id_Usuario = reader.GetInt32(1),
                                Id_Gato = reader.GetInt32(2),
                                Fecha_Solicitud = reader.GetDateTime(3),
                                Estado = reader.GetString(4),
                                Comentario_Usuario = reader.GetString(5),
                                Comentario_Protectora = reader.GetString(6)
                            };

                            solicitudes.Add(solicitud);
                        }
                    }
                }
            }
            return solicitudes;
        }

        public async Task<SolicitudAdopcion?> GetByIdAsync(int id)
        {
            SolicitudAdopcion? solicitud = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM SolicitudAdopcion WHERE Id_Solicitud = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            solicitud = new SolicitudAdopcion
                            {
                                Id_Solicitud = reader.GetInt32(0),
                                Id_Usuario = reader.GetInt32(1),
                                Id_Gato = reader.GetInt32(2),
                                Fecha_Solicitud = reader.GetDateTime(3),
                                Estado = reader.GetString(4),
                                Comentario_Usuario = reader.GetString(5),
                                Comentario_Protectora = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return solicitud;
        }

        public async Task AddAsync(SolicitudAdopcion solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO SolicitudAdopcion (Id_Usuario, Id_Gato, Fecha_Solicitud, Estado, Comentario_Usuario, Comentario_Protectora) VALUES (@Id_Usuario, @Id_Gato, @Fecha_Solicitud, @Estado, @Comentario_Usuario, @Comentario_Protectora)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
                    command.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);
                    command.Parameters.AddWithValue("@Fecha_Solicitud", solicitud.Fecha_Solicitud);
                    command.Parameters.AddWithValue("@Estado", solicitud.Estado);
                    command.Parameters.AddWithValue("@Comentario_Usuario", solicitud.Comentario_Usuario);
                    command.Parameters.AddWithValue("@Comentario_Protectora", solicitud.Comentario_Protectora);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(SolicitudAdopcion solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE SolicitudAdopcion SET 
                                 Id_Usuario = @Id_Usuario,
                                 Id_Gato = @Id_Gato,
                                 Fecha_Solicitud = @Fecha_Solicitud,
                                 Estado = @Estado,
                                 Comentario_Usuario = @Comentario_Usuario,
                                 Comentario_Protectora = @Comentario_Protectora
                                 WHERE Id_Solicitud = @Id_Solicitud";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Solicitud", solicitud.Id_Solicitud);
                    command.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
                    command.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);
                    command.Parameters.AddWithValue("@Fecha_Solicitud", solicitud.Fecha_Solicitud);
                    command.Parameters.AddWithValue("@Estado", solicitud.Estado);
                    command.Parameters.AddWithValue("@Comentario_Usuario", solicitud.Comentario_Usuario);
                    command.Parameters.AddWithValue("@Comentario_Protectora", solicitud.Comentario_Protectora);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "DELETE FROM SolicitudAdopcion WHERE Id_Solicitud = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
