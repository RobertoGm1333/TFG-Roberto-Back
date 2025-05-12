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

        // 🆕 Nuevo método para obtener solicitudes de una protectora
        public async Task<List<SolicitudAdopcion>> GetByProtectoraAsync(int idProtectora)
        {
            var solicitudes = new List<SolicitudAdopcion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT s.Id_Solicitud, s.Id_Usuario, s.Id_Gato, s.Fecha_Solicitud, s.Estado, s.Comentario_Usuario, s.Comentario_Protectora
                                 FROM SolicitudAdopcion s
                                 JOIN Gato g ON s.Id_Gato = g.Id_Gato
                                 WHERE g.Id_Protectora = @IdProtectora
                                 ORDER BY s.Fecha_Solicitud DESC;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProtectora", idProtectora);

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
        public async Task<List<object>> GetSolicitudesByProtectoraAsync(int idProtectora)
        {
            var solicitudes = new List<object>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT 
                            s.Id_Solicitud,
                            s.Id_Usuario,
                            u.Nombre as Usuario,
                            s.Id_Gato,
                            g.Nombre_Gato as Gato,
                            g.Imagen_Gato as Imagen,
                            s.Fecha_Solicitud,
                            s.Estado,
                            s.Comentario_Usuario,
                            s.Comentario_Protectora
                         FROM SolicitudAdopcion s
                         JOIN Gato g ON s.Id_Gato = g.Id_Gato
                         JOIN Usuario u ON s.Id_Usuario = u.Id_Usuario
                         WHERE g.Id_Protectora = @IdProtectora
                         ORDER BY s.Fecha_Solicitud DESC;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProtectora", idProtectora);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var solicitud = new
                            {
                                Id_Solicitud = reader.GetInt32(0),
                                Id_Usuario = reader.GetInt32(1),
                                Usuario = reader.GetString(2),
                                Id_Gato = reader.GetInt32(3),
                                Gato = reader.GetString(4),
                                Imagen = reader.GetString(5),
                                Fecha_Solicitud = reader.GetDateTime(6),
                                Estado = reader.GetString(7),
                                Comentario_Usuario = reader.GetString(8),
                                Comentario_Protectora = reader.GetString(9)
                            };

                            solicitudes.Add(solicitud);
                        }
                    }
                }
            }

            return solicitudes;
        }

    }
}
