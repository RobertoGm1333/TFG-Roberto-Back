using Microsoft.AspNetCore.Mvc;
using ProtectoraAPI.Repositories;
using Models;

namespace ProtectoraAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class DeseadoController : ControllerBase
   {
       private readonly IDeseadoRepository _repository;

       public DeseadoController(IDeseadoRepository repository)
       {
           _repository = repository;
       }

       [HttpGet]
       public async Task<ActionResult<List<Deseado>>> GetDeseados()
       {
           var deseados = await _repository.GetAllAsync();
           return Ok(deseados);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<Deseado>> GetDeseado(int id)
       {
           var deseado = await _repository.GetByIdAsync(id);
           if (deseado == null)
           {
               return NotFound();
           }
           return Ok(deseado);
       }

        [HttpGet("usuario/{Id_Usuario}")]
       public async Task<IActionResult> ObtenerDeseadosPorUsuario(int Id_Usuario)
       {
           var deseados = await _repository.ObtenerDeseadosPorUsuarioAsync(Id_Usuario);

           if (deseados == null || !deseados.Any())
           {
               return NotFound(new { message = "No hay gatos en deseados para este usuario." });
           } 

           return Ok(deseados);
       }

       [HttpPost]
       public async Task<ActionResult<Deseado>> CreateDeseado(Deseado deseado)
       {
           await _repository.AddAsync(deseado);
           return CreatedAtAction(nameof(GetDeseado), new { id = deseado.Id_Deseado }, deseado);
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteDeseado(int id)
       {
           var deseado = await _repository.GetByIdAsync(id);
           if (deseado == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }
   }
}
