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
                            solicitudes.Add(MapSolicitudFromReader(reader));
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
                                Comentario_Protectora = reader.IsDBNull(8) ? null : reader.GetString(8)
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
                            return MapSolicitudFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public async Task AddAsync(SolicitudAdopcion solicitud)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO SolicitudAdopcion (
                        Id_Usuario, Id_Gato, Fecha_Solicitud, Estado, NombreCompleto, 
                        Edad, Direccion, Telefono, Email, TipoVivienda, PropiedadAlquiler,
                        PermiteAnimales, NumeroPersonas, HayNinos, EdadesNinos, ExperienciaGatos,
                        TieneOtrosAnimales, CortarUnas, AnimalesVacunadosEsterilizados, HistorialMascotas,
                        MotivacionAdopcion, ProblemasComportamiento, EnfermedadesCostosas,
                        Vacaciones, SeguimientoPostAdopcion, VisitaHogar, Comentario_Protectora
                    )
                    OUTPUT INSERTED.Id_Solicitud
                    VALUES (
                        @Id_Usuario, @Id_Gato, @Fecha_Solicitud, @Estado, @NombreCompleto,
                        @Edad, @Direccion, @Telefono, @Email, @TipoVivienda, @PropiedadAlquiler,
                        @PermiteAnimales, @NumeroPersonas, @HayNinos, @EdadesNinos, @ExperienciaGatos,
                        @TieneOtrosAnimales, @CortarUnas, @AnimalesVacunadosEsterilizados, @HistorialMascotas,
                        @MotivacionAdopcion, @ProblemasComportamiento, @EnfermedadesCostosas,
                        @Vacaciones, @SeguimientoPostAdopcion, @VisitaHogar, @Comentario_Protectora
                    )";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
                    command.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);
                    command.Parameters.AddWithValue("@Fecha_Solicitud", solicitud.Fecha_Solicitud);
                    command.Parameters.AddWithValue("@Estado", solicitud.Estado);
                    command.Parameters.AddWithValue("@NombreCompleto", solicitud.NombreCompleto ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Edad", solicitud.Edad ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Direccion", solicitud.Direccion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Telefono", solicitud.Telefono ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", solicitud.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TipoVivienda", solicitud.TipoVivienda ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PropiedadAlquiler", solicitud.PropiedadAlquiler ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PermiteAnimales", solicitud.PermiteAnimales ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@NumeroPersonas", solicitud.NumeroPersonas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@HayNinos", solicitud.HayNinos ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EdadesNinos", solicitud.EdadesNinos ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ExperienciaGatos", solicitud.ExperienciaGatos ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TieneOtrosAnimales", solicitud.TieneOtrosAnimales ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CortarUnas", solicitud.CortarUnas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AnimalesVacunadosEsterilizados", solicitud.AnimalesVacunadosEsterilizados ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@HistorialMascotas", solicitud.HistorialMascotas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MotivacionAdopcion", solicitud.MotivacionAdopcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProblemasComportamiento", solicitud.ProblemasComportamiento ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EnfermedadesCostosas", solicitud.EnfermedadesCostosas ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Vacaciones", solicitud.Vacaciones ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SeguimientoPostAdopcion", solicitud.SeguimientoPostAdopcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@VisitaHogar", solicitud.VisitaHogar ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Comentario_Protectora", solicitud.Comentario_Protectora ?? (object)DBNull.Value);

                    var idSolicitud = await command.ExecuteScalarAsync();
                    solicitud.Id_Solicitud = Convert.ToInt32(idSolicitud);
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
                                 Comentario_Protectora = @Comentario_Protectora
                                 WHERE Id_Solicitud = @Id_Solicitud";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id_Solicitud", solicitud.Id_Solicitud);
                    command.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
                    command.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);
                    command.Parameters.AddWithValue("@Fecha_Solicitud", solicitud.Fecha_Solicitud);
                    command.Parameters.AddWithValue("@Estado", solicitud.Estado);
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

                string query = @"SELECT s.Id_Solicitud, s.Id_Usuario, s.Id_Gato, s.Fecha_Solicitud, s.Estado, s.Comentario_Protectora
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
                                Comentario_Protectora = reader.GetString(5)
                            };

                            solicitudes.Add(solicitud);
                        }
                    }
                }
            }

            return solicitudes;
        }

        public async Task<List<SolicitudAdopcion>> GetByProtectoraIdAsync(int idProtectora)
        {
            var solicitudes = new List<SolicitudAdopcion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT s.* 
                    FROM SolicitudAdopcion s
                    INNER JOIN Gato g ON s.Id_Gato = g.Id_Gato
                    WHERE g.Id_Protectora = @IdProtectora";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdProtectora", idProtectora);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            solicitudes.Add(MapSolicitudFromReader(reader));
                        }
                    }
                }
            }
            return solicitudes;
        }

        private SolicitudAdopcion MapSolicitudFromReader(SqlDataReader reader)
        {
            return new SolicitudAdopcion
            {
                Id_Solicitud = reader.GetInt32(reader.GetOrdinal("Id_Solicitud")),
                Id_Usuario = reader.GetInt32(reader.GetOrdinal("Id_Usuario")),
                Id_Gato = reader.GetInt32(reader.GetOrdinal("Id_Gato")),
                Fecha_Solicitud = reader.GetDateTime(reader.GetOrdinal("Fecha_Solicitud")),
                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                NombreCompleto = reader.IsDBNull(reader.GetOrdinal("NombreCompleto")) ? null : reader.GetString(reader.GetOrdinal("NombreCompleto")),
                Edad = reader.IsDBNull(reader.GetOrdinal("Edad")) ? null : reader.GetInt32(reader.GetOrdinal("Edad")),
                Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion")),
                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                TipoVivienda = reader.IsDBNull(reader.GetOrdinal("TipoVivienda")) ? null : reader.GetString(reader.GetOrdinal("TipoVivienda")),
                PropiedadAlquiler = reader.IsDBNull(reader.GetOrdinal("PropiedadAlquiler")) ? null : reader.GetString(reader.GetOrdinal("PropiedadAlquiler")),
                PermiteAnimales = reader.IsDBNull(reader.GetOrdinal("PermiteAnimales")) ? null : reader.GetBoolean(reader.GetOrdinal("PermiteAnimales")),
                NumeroPersonas = reader.IsDBNull(reader.GetOrdinal("NumeroPersonas")) ? null : reader.GetInt32(reader.GetOrdinal("NumeroPersonas")),
                HayNinos = reader.IsDBNull(reader.GetOrdinal("HayNinos")) ? null : reader.GetBoolean(reader.GetOrdinal("HayNinos")),
                EdadesNinos = reader.IsDBNull(reader.GetOrdinal("EdadesNinos")) ? null : reader.GetString(reader.GetOrdinal("EdadesNinos")),
                ExperienciaGatos = reader.IsDBNull(reader.GetOrdinal("ExperienciaGatos")) ? null : reader.GetBoolean(reader.GetOrdinal("ExperienciaGatos")),
                TieneOtrosAnimales = reader.IsDBNull(reader.GetOrdinal("TieneOtrosAnimales")) ? null : reader.GetBoolean(reader.GetOrdinal("TieneOtrosAnimales")),
                CortarUnas = reader.IsDBNull(reader.GetOrdinal("CortarUnas")) ? null : reader.GetBoolean(reader.GetOrdinal("CortarUnas")),
                AnimalesVacunadosEsterilizados = reader.IsDBNull(reader.GetOrdinal("AnimalesVacunadosEsterilizados")) ? null : reader.GetBoolean(reader.GetOrdinal("AnimalesVacunadosEsterilizados")),
                HistorialMascotas = reader.IsDBNull(reader.GetOrdinal("HistorialMascotas")) ? null : reader.GetString(reader.GetOrdinal("HistorialMascotas")),
                MotivacionAdopcion = reader.IsDBNull(reader.GetOrdinal("MotivacionAdopcion")) ? null : reader.GetString(reader.GetOrdinal("MotivacionAdopcion")),
                ProblemasComportamiento = reader.IsDBNull(reader.GetOrdinal("ProblemasComportamiento")) ? null : reader.GetString(reader.GetOrdinal("ProblemasComportamiento")),
                EnfermedadesCostosas = reader.IsDBNull(reader.GetOrdinal("EnfermedadesCostosas")) ? null : reader.GetString(reader.GetOrdinal("EnfermedadesCostosas")),
                Vacaciones = reader.IsDBNull(reader.GetOrdinal("Vacaciones")) ? null : reader.GetString(reader.GetOrdinal("Vacaciones")),
                SeguimientoPostAdopcion = reader.IsDBNull(reader.GetOrdinal("SeguimientoPostAdopcion")) ? null : reader.GetBoolean(reader.GetOrdinal("SeguimientoPostAdopcion")),
                VisitaHogar = reader.IsDBNull(reader.GetOrdinal("VisitaHogar")) ? null : reader.GetBoolean(reader.GetOrdinal("VisitaHogar")),
                Comentario_Protectora = reader.IsDBNull(reader.GetOrdinal("Comentario_Protectora")) ? null : reader.GetString(reader.GetOrdinal("Comentario_Protectora"))
            };
        }

        public async Task<List<SolicitudAdopcion>> GetByUsuarioIdAsync(int idUsuario)
        {
            var solicitudes = new List<SolicitudAdopcion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM SolicitudAdopcion WHERE Id_Usuario = @IdUsuario";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            solicitudes.Add(MapSolicitudFromReader(reader));
                        }
                    }
                }
            }
            return solicitudes;
        }

        public async Task<List<SolicitudAdopcion>> GetByGatoIdAsync(int idGato)
        {
            var solicitudes = new List<SolicitudAdopcion>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM SolicitudAdopcion WHERE Id_Gato = @IdGato";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdGato", idGato);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            solicitudes.Add(MapSolicitudFromReader(reader));
                        }
                    }
                }
            }
            return solicitudes;
        }

        public async Task UpdateEstadoAsync(int id, string nuevoEstado, string? comentarioProtectora)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE SolicitudAdopcion SET Estado = @Estado, Comentario_Protectora = @Comentario WHERE Id_Solicitud = @Id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Estado", nuevoEstado);
                    command.Parameters.AddWithValue("@Comentario", comentarioProtectora ?? (object)DBNull.Value);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void AddSolicitudParameters(SqlCommand command, SolicitudAdopcion solicitud)
        {
            command.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
            command.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);
            command.Parameters.AddWithValue("@Fecha_Solicitud", solicitud.Fecha_Solicitud);
            command.Parameters.AddWithValue("@Estado", solicitud.Estado);
            command.Parameters.AddWithValue("@NombreCompleto", solicitud.NombreCompleto ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Edad", solicitud.Edad ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Direccion", solicitud.Direccion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", solicitud.Telefono ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", solicitud.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TipoVivienda", solicitud.TipoVivienda ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PropiedadAlquiler", solicitud.PropiedadAlquiler ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PermiteAnimales", solicitud.PermiteAnimales ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@NumeroPersonas", solicitud.NumeroPersonas ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@HayNinos", solicitud.HayNinos ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EdadesNinos", solicitud.EdadesNinos ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ExperienciaGatos", solicitud.ExperienciaGatos ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TieneOtrosAnimales", solicitud.TieneOtrosAnimales ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CortarUnas", solicitud.CortarUnas ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@AnimalesVacunadosEsterilizados", solicitud.AnimalesVacunadosEsterilizados ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@HistorialMascotas", solicitud.HistorialMascotas ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MotivacionAdopcion", solicitud.MotivacionAdopcion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ProblemasComportamiento", solicitud.ProblemasComportamiento ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EnfermedadesCostosas", solicitud.EnfermedadesCostosas ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Vacaciones", solicitud.Vacaciones ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@SeguimientoPostAdopcion", solicitud.SeguimientoPostAdopcion ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@VisitaHogar", solicitud.VisitaHogar ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Comentario_Protectora", solicitud.Comentario_Protectora ?? (object)DBNull.Value);
        }
    }
}
