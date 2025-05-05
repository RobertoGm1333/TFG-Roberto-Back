using Microsoft.AspNetCore.Mvc;
using ProtectoraAPI.Repositories;
using Models;

namespace ProtectoraAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class GatoController : ControllerBase
   {
       private readonly IGatoRepository _repository;

       public GatoController(IGatoRepository repository)
       {
           _repository = repository;
       }

       [HttpGet]
       public async Task<ActionResult<List<Gato>>> GetGatos()
       {
           var gatos = await _repository.GetAllAsync();
           return Ok(gatos);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<Gato>> GetGato(int id)
       {
           var gato = await _repository.GetByIdAsync(id);
           if (gato == null)
           {
               return NotFound();
           }
           return Ok(gato);
       }

       [HttpPost]
       public async Task<ActionResult<Gato>> CreateGato(Gato gato)
       {
           await _repository.AddAsync(gato);
           return CreatedAtAction(nameof(GetGato), new { id = gato.Id_Gato }, gato);
       }

       [HttpPut("{id}")]
       public async Task<IActionResult> UpdateGato(int id, Gato updatedGato)
       {
           var existingGato = await _repository.GetByIdAsync(id);
           if (existingGato == null)
           {
               return NotFound();
           }

           existingGato.Nombre_Gato = updatedGato.Nombre_Gato;
           existingGato.Raza = updatedGato.Raza;
           existingGato.Edad = updatedGato.Edad;
           existingGato.Esterilizado = updatedGato.Esterilizado;
           existingGato.Sexo = updatedGato.Sexo;
           existingGato.Descripcion_Gato = updatedGato.Descripcion_Gato;
           existingGato.Imagen_Gato = updatedGato.Imagen_Gato;

           await _repository.UpdateAsync(existingGato);
           return NoContent();
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteGato(int id)
       {
           var gato = await _repository.GetByIdAsync(id);
           if (gato == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }
   }
}
