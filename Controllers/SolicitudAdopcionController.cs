using Microsoft.AspNetCore.Mvc;
using ProtectoraAPI.Repositories;
using Models;

namespace ProtectoraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudAdopcionController : ControllerBase
    {
        private readonly ISolicitudAdopcionRepository _repository;

        public SolicitudAdopcionController(ISolicitudAdopcionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<SolicitudAdopcion>>> GetSolicitudes()
        {
            var solicitudes = await _repository.GetAllAsync();
            return Ok(solicitudes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitudAdopcion>> GetSolicitud(int id)
        {
            var solicitud = await _repository.GetByIdAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            return Ok(solicitud);
        }

        [HttpPost]
        public async Task<ActionResult<SolicitudAdopcion>> CreateSolicitud(SolicitudAdopcion solicitud)
        {
            try
            {
                // Validaciones básicas
                if (solicitud == null)
                {
                    return BadRequest(new { message = "La solicitud no puede estar vacía" });
                }

                // Validar campos requeridos
                var validationErrors = new List<string>();

                if (string.IsNullOrWhiteSpace(solicitud.NombreCompleto))
                    validationErrors.Add("El nombre completo es requerido");
                if (string.IsNullOrWhiteSpace(solicitud.DNI))
                    validationErrors.Add("El DNI es requerido");
                if (string.IsNullOrWhiteSpace(solicitud.Email))
                    validationErrors.Add("El email es requerido");
                if (string.IsNullOrWhiteSpace(solicitud.Telefono))
                    validationErrors.Add("El teléfono es requerido");
                if (solicitud.Id_Usuario <= 0)
                    validationErrors.Add("ID de usuario inválido");
                if (solicitud.Id_Gato <= 0)
                    validationErrors.Add("ID de gato inválido");
                if (solicitud.Edad.HasValue && (solicitud.Edad.Value < 18 || solicitud.Edad.Value > 120))
                    validationErrors.Add("La edad debe estar entre 18 y 120 años");
                if (solicitud.NumeroPersonas.HasValue && solicitud.NumeroPersonas.Value <= 0)
                    validationErrors.Add("El número de personas debe ser mayor que 0");

                if (validationErrors.Any())
                {
                    return BadRequest(new { message = "Errores de validación", errors = validationErrors });
                }

                solicitud.Fecha_Solicitud = DateTime.Now;
                solicitud.Estado = "pendiente";
                
                // Validar longitud de campos de texto e imágenes
                const int MAX_IMAGE_SIZE = 500000; // 500KB por imagen

                if (!string.IsNullOrEmpty(solicitud.Fotos_Hogar) && solicitud.Fotos_Hogar.Length > MAX_IMAGE_SIZE)
                {
                    return BadRequest(new { message = "Las imágenes del hogar son demasiado grandes. El tamaño máximo es 500KB por imagen." });
                }
                if (!string.IsNullOrEmpty(solicitud.Fotos_DNI) && solicitud.Fotos_DNI.Length > MAX_IMAGE_SIZE)
                {
                    return BadRequest(new { message = "Las imágenes del DNI son demasiado grandes. El tamaño máximo es 500KB por imagen." });
                }

                // Asegurarse de que los campos de imágenes sean null si están vacíos
                solicitud.Fotos_Hogar = string.IsNullOrEmpty(solicitud.Fotos_Hogar) ? null : solicitud.Fotos_Hogar;
                solicitud.Fotos_DNI = string.IsNullOrEmpty(solicitud.Fotos_DNI) ? null : solicitud.Fotos_DNI;

                try
                {
                    await _repository.AddAsync(solicitud);
                    return CreatedAtAction(nameof(GetSolicitud), new { id = solicitud.Id_Solicitud }, solicitud);
                }
                catch (Exception ex) when (ex.Message.Contains("El usuario especificado no existe"))
                {
                    return BadRequest(new { message = "El usuario especificado no existe en el sistema" });
                }
                catch (Exception ex) when (ex.Message.Contains("El gato especificado no existe"))
                {
                    return BadRequest(new { message = "El gato especificado no existe en el sistema" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "Error al crear la solicitud", error = ex.Message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSolicitud(int id, SolicitudAdopcion updated)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            updated.Id_Solicitud = id;
            await _repository.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _repository.GetByIdAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<SolicitudAdopcion>>> GetSolicitudesPorUsuario(int idUsuario)
        {
            try
            {
                var solicitudes = await _repository.GetByUsuarioIdAsync(idUsuario);
                if (solicitudes == null || !solicitudes.Any())
                {
                    return NotFound(new { message = "No hay solicitudes para este usuario." });
                }
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las solicitudes: {ex.Message}");
            }
        }

        [HttpGet("gato/{idGato}")]
        public async Task<ActionResult<List<SolicitudAdopcion>>> GetSolicitudesPorGato(int idGato)
        {
            var solicitudes = await _repository.GetByGatoIdAsync(idGato);
            return Ok(solicitudes);
        }

        [HttpGet("protectora/{idProtectora}")]
        public async Task<ActionResult<List<object>>> GetSolicitudesPorProtectora(int idProtectora)
        {
            var solicitudes = await _repository.GetSolicitudesByProtectoraAsync(idProtectora);
            return Ok(solicitudes);
        }

        [HttpPut("estado/{id}")]
        public async Task<IActionResult> UpdateEstado(int id, [FromBody] CambiarEstadoDTO datos)
        {
            var solicitud = await _repository.GetByIdAsync(id);
            if (solicitud == null)
                return NotFound();

            await _repository.UpdateEstadoAsync(id, datos.Estado, datos.Comentario_Protectora);
            return NoContent();
        }

        [HttpGet("usuario/{idUsuario}/gato/{idGato}")]
        public async Task<ActionResult<SolicitudAdopcion>> GetSolicitudPorUsuarioYGato(int idUsuario, int idGato)
        {
            try
            {
                var solicitudes = await _repository.GetByUsuarioIdAsync(idUsuario);
                var solicitud = solicitudes.FirstOrDefault(s => s.Id_Gato == idGato);
                
                if (solicitud == null)
                {
                    return NotFound(new { message = "No existe una solicitud para este usuario y gato." });
                }

                return Ok(solicitud);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la solicitud: {ex.Message}");
            }
        }

        [HttpPost("{id}/imagenes")]
        public async Task<IActionResult> SubirImagenes(int id, [FromForm] ImagenesRequest imagenes)
        {
            try
            {
                var solicitud = await _repository.GetByIdAsync(id);
                if (solicitud == null)
                    return NotFound("Solicitud no encontrada");

                if (imagenes.FotosHogar != null)
                {
                    // Aquí implementarías la lógica para guardar la imagen
                    // Por ejemplo, guardarla en un directorio y almacenar la ruta
                    string rutaHogar = await GuardarImagen(imagenes.FotosHogar, "hogar");
                    solicitud.Fotos_Hogar = rutaHogar;
                }

                if (imagenes.FotosDNI != null)
                {
                    string rutaDNI = await GuardarImagen(imagenes.FotosDNI, "dni");
                    solicitud.Fotos_DNI = rutaDNI;
                }

                await _repository.UpdateAsync(solicitud);
                return Ok("Imágenes subidas correctamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al subir imágenes: {ex.Message}");
                return StatusCode(500, "Error al procesar las imágenes");
            }
        }

        private async Task<string> GuardarImagen(IFormFile archivo, string tipo)
        {
            try
            {
                string nombreArchivo = $"{Guid.NewGuid()}_{tipo}_{archivo.FileName}";
                string directorio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "solicitudes");
                
                if (!Directory.Exists(directorio))
                    Directory.CreateDirectory(directorio);

                string rutaCompleta = Path.Combine(directorio, nombreArchivo);
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                return $"/imagenes/solicitudes/{nombreArchivo}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar la imagen: {ex.Message}");
            }
        }

        public class ImagenesRequest
        {
            public IFormFile? FotosHogar { get; set; }
            public IFormFile? FotosDNI { get; set; }
        }
    }

    public class CambiarEstadoDTO
    {
        public string Estado { get; set; } = "";
        public string? Comentario_Protectora { get; set; }
    }
} 