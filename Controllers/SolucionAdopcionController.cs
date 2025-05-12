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
            solicitud.Fecha_Solicitud = DateTime.Now;
            solicitud.Estado = "pendiente";
            await _repository.AddAsync(solicitud);
            return CreatedAtAction(nameof(GetSolicitud), new { id = solicitud.Id_Solicitud }, solicitud);
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

        // 🆕 Ver mis solicitudes (usuario loggeado)
        [HttpGet("usuario/{userId}")]
        public async Task<IActionResult> GetMisSolicitudes(int userId)
        {
            var solicitudes = await _repository.GetAllAsync();
            var misSolicitudes = solicitudes.Where(s => s.Id_Usuario == userId).ToList();
            return Ok(misSolicitudes);
        }

        // 🆕 Cambiar estado y comentario (protectora)
        [HttpPut("estado/{id}")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] CambiarEstadoDTO datos)
        {
            var solicitud = await _repository.GetByIdAsync(id);
            if (solicitud == null)
                return NotFound();

            solicitud.Estado = datos.Estado;
            solicitud.Comentario_Protectora = datos.Comentario_Protectora;
            await _repository.UpdateAsync(solicitud);

            return Ok(solicitud);
        }

        // 🆕 Ver solicitudes por protectora (pendiente crear repo JOIN)
        [HttpGet("protectora/{idProtectora}")]
        public async Task<IActionResult> GetSolicitudesPorProtectora(int idProtectora)
        {
            var solicitudes = await _repository.GetSolicitudesByProtectoraAsync(idProtectora);
            return Ok(solicitudes);
        }

        // Filtrar solicitud por usuario y gato específico
        [HttpGet("usuario/{idUsuario}/gato/{idGato}")]
        public async Task<IActionResult> GetSolicitudPorUsuarioYGato(int idUsuario, int idGato)
        {
            var solicitudes = await _repository.GetAllAsync();
            var solicitud = solicitudes.FirstOrDefault(s => s.Id_Usuario == idUsuario && s.Id_Gato == idGato);
            
            if (solicitud == null)
            {
                return NotFound();
            }

            return Ok(solicitud);
        }

    }

    // DTO para cambiar estado
    public class CambiarEstadoDTO
    {
        public string Estado { get; set; }
        public string Comentario_Protectora { get; set; }
    }
}
