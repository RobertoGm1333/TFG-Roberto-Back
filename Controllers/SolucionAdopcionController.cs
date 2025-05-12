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
        [HttpGet("mias")]
        public async Task<IActionResult> GetMisSolicitudes()
        {
            var idUsuario = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var solicitudes = await _repository.GetAllAsync();
            var misSolicitudes = solicitudes.Where(s => s.Id_Usuario == idUsuario).ToList();
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
        public async Task<IActionResult> GetSolicitudesDeProtectora(int idProtectora)
        {
            // Aquí debes crear el método en Repository que devuelva solicitudes de la protectora con JOIN correcto
            // Este endpoint es solo un placeholder hasta tener el método
            return BadRequest("Este endpoint requiere implementación en Repository con JOIN.");
        }
    }

    // DTO para cambiar estado
    public class CambiarEstadoDTO
    {
        public string Estado { get; set; }
        public string Comentario_Protectora { get; set; }
    }
}
