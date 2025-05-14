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
            // Validar los datos del modelo
            solicitud.ValidarDatos();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Primero verificar si el usuario y el gato existen
                string checkQuery = @"
                    SELECT 
                        (SELECT COUNT(*) FROM Usuario WHERE Id_Usuario = @Id_Usuario) as UserExists,
                        (SELECT COUNT(*) FROM Gato WHERE Id_Gato = @Id_Gato) as CatExists,
                        (SELECT Id_Protectora FROM Gato WHERE Id_Gato = @Id_Gato) as ProtectoraId";

                using (var checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
                    checkCommand.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);

                    using (var reader = await checkCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            bool userExists = reader.GetInt32(0) > 0;
                            bool catExists = reader.GetInt32(1) > 0;

                            if (!userExists)
                                throw new Exception("El usuario especificado no existe.");
                            if (!catExists)
                                throw new Exception("El gato especificado no existe.");

                            // Obtener y asignar el Id_Protectora
                            if (!reader.IsDBNull(2))
                            {
                                solicitud.Id_Protectora = reader.GetInt32(2);
                            }
                        }
                    }
                }

                string query = @"
                    INSERT INTO SolicitudAdopcion (
                        Id_Usuario, Id_Gato, Fecha_Solicitud, Estado, NombreCompleto, 
                        Edad, Direccion, DNI, Telefono, Email, TipoVivienda, PropiedadAlquiler,
                        PermiteAnimales, NumeroPersonas, HayNinos, EdadesNinos, ExperienciaGatos,
                        TieneOtrosAnimales, CortarUnas, AnimalesVacunadosEsterilizados, HistorialMascotas,
                        MotivacionAdopcion, ProblemasComportamiento, EnfermedadesCostosas,
                        Vacaciones, SeguimientoPostAdopcion, VisitaHogar, Fotos_Hogar, Fotos_DNI, Comentario_Protectora
                    )
                    OUTPUT INSERTED.Id_Solicitud
                    VALUES (
                        @Id_Usuario, @Id_Gato, @Fecha_Solicitud, @Estado, @NombreCompleto,
                        @Edad, @Direccion, @DNI, @Telefono, @Email, @TipoVivienda, @PropiedadAlquiler,
                        @PermiteAnimales, @NumeroPersonas, @HayNinos, @EdadesNinos, @ExperienciaGatos,
                        @TieneOtrosAnimales, @CortarUnas, @AnimalesVacunadosEsterilizados, @HistorialMascotas,
                        @MotivacionAdopcion, @ProblemasComportamiento, @EnfermedadesCostosas,
                        @Vacaciones, @SeguimientoPostAdopcion, @VisitaHogar, @Fotos_Hogar, @Fotos_DNI, @Comentario_Protectora
                    )";

                try
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        AddSolicitudParameters(command, solicitud);
                        var idSolicitud = await command.ExecuteScalarAsync();
                        if (idSolicitud != null)
                        {
                            solicitud.Id_Solicitud = Convert.ToInt32(idSolicitud);
                        }
                        else
                        {
                            throw new Exception("No se pudo obtener el ID de la solicitud creada.");
                        }
                    }
                }
                catch (SqlException ex)
                {
                    var message = "Error al insertar en la base de datos: ";
                    if (ex.Message.Contains("FK_"))
                    {
                        message += "Error de clave foránea. Asegúrese de que el usuario y el gato existen.";
                    }
                    else if (ex.Message.Contains("UQ_"))
                    {
                        message += "Ya existe una solicitud para este gato y usuario.";
                    }
                    else
                    {
                        message += ex.Message;
                    }
                    throw new Exception(message, ex);
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
                DNI = reader.IsDBNull(reader.GetOrdinal("DNI")) ? null : reader.GetString(reader.GetOrdinal("DNI")),
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
                Fotos_Hogar = reader.IsDBNull(reader.GetOrdinal("Fotos_Hogar")) ? null : reader.GetString(reader.GetOrdinal("Fotos_Hogar")),
                Fotos_DNI = reader.IsDBNull(reader.GetOrdinal("Fotos_DNI")) ? null : reader.GetString(reader.GetOrdinal("Fotos_DNI")),
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
            // Validar y truncar campos de texto si es necesario
            string TruncateString(string value, int maxLength)
            {
                if (string.IsNullOrEmpty(value)) return null;
                return value.Length <= maxLength ? value : value.Substring(0, maxLength);
            }

            // Campos requeridos
            command.Parameters.AddWithValue("@Id_Usuario", solicitud.Id_Usuario);
            command.Parameters.AddWithValue("@Id_Gato", solicitud.Id_Gato);
            command.Parameters.AddWithValue("@Fecha_Solicitud", solicitud.Fecha_Solicitud);
            command.Parameters.AddWithValue("@Estado", solicitud.Estado);

            // Campos opcionales con validación de longitud
            command.Parameters.AddWithValue("@NombreCompleto", (object)TruncateString(solicitud.NombreCompleto, 100) ?? DBNull.Value);
            command.Parameters.AddWithValue("@Edad", (object)solicitud.Edad ?? DBNull.Value);
            command.Parameters.AddWithValue("@Direccion", (object)TruncateString(solicitud.Direccion, 255) ?? DBNull.Value);
            command.Parameters.AddWithValue("@DNI", (object)TruncateString(solicitud.DNI, 20) ?? DBNull.Value);
            command.Parameters.AddWithValue("@Telefono", (object)TruncateString(solicitud.Telefono, 20) ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object)TruncateString(solicitud.Email, 100) ?? DBNull.Value);
            command.Parameters.AddWithValue("@TipoVivienda", (object)TruncateString(solicitud.TipoVivienda, 50) ?? DBNull.Value);
            command.Parameters.AddWithValue("@PropiedadAlquiler", (object)TruncateString(solicitud.PropiedadAlquiler, 50) ?? DBNull.Value);
            command.Parameters.AddWithValue("@PermiteAnimales", (object)solicitud.PermiteAnimales ?? DBNull.Value);
            command.Parameters.AddWithValue("@NumeroPersonas", (object)solicitud.NumeroPersonas ?? DBNull.Value);
            command.Parameters.AddWithValue("@HayNinos", (object)solicitud.HayNinos ?? DBNull.Value);
            command.Parameters.AddWithValue("@EdadesNinos", (object)TruncateString(solicitud.EdadesNinos, 100) ?? DBNull.Value);
            command.Parameters.AddWithValue("@ExperienciaGatos", (object)solicitud.ExperienciaGatos ?? DBNull.Value);
            command.Parameters.AddWithValue("@TieneOtrosAnimales", (object)solicitud.TieneOtrosAnimales ?? DBNull.Value);
            command.Parameters.AddWithValue("@CortarUnas", (object)solicitud.CortarUnas ?? DBNull.Value);
            command.Parameters.AddWithValue("@AnimalesVacunadosEsterilizados", (object)solicitud.AnimalesVacunadosEsterilizados ?? DBNull.Value);
            command.Parameters.AddWithValue("@HistorialMascotas", (object)TruncateString(solicitud.HistorialMascotas, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@MotivacionAdopcion", (object)TruncateString(solicitud.MotivacionAdopcion, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@ProblemasComportamiento", (object)TruncateString(solicitud.ProblemasComportamiento, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@EnfermedadesCostosas", (object)TruncateString(solicitud.EnfermedadesCostosas, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@Vacaciones", (object)TruncateString(solicitud.Vacaciones, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@SeguimientoPostAdopcion", (object)solicitud.SeguimientoPostAdopcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@VisitaHogar", (object)solicitud.VisitaHogar ?? DBNull.Value);
            command.Parameters.AddWithValue("@Fotos_Hogar", (object)TruncateString(solicitud.Fotos_Hogar, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@Fotos_DNI", (object)TruncateString(solicitud.Fotos_DNI, 1000) ?? DBNull.Value);
            command.Parameters.AddWithValue("@Comentario_Protectora", (object)TruncateString(solicitud.Comentario_Protectora, 1000) ?? DBNull.Value);
        }
    }
}
