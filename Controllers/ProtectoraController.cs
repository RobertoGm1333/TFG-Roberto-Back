using Microsoft.AspNetCore.Mvc;
using ProtectoraAPI.Repositories;
using Models;

namespace ProtectoraAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class ProtectoraController : ControllerBase
   {
       private readonly IProtectoraRepository _repository;

       public ProtectoraController(IProtectoraRepository repository)
       {
           _repository = repository;
       }

       [HttpGet]
       public async Task<ActionResult<List<Protectora>>> GetProtectoras()
       {
           var protectoras = await _repository.GetAllAsync();
           return Ok(protectoras);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<Protectora>> GetProtectora(int id)
       {
           var protectora = await _repository.GetByIdAsync(id);
           if (protectora == null)
           {
               return NotFound();
           }
           return Ok(protectora);
       }

       [HttpPost]
       public async Task<ActionResult<Protectora>> CreateProtectora(Protectora protectora)
       {
           await _repository.AddAsync(protectora);
           return CreatedAtAction(nameof(GetProtectora), new { id = protectora.Id_Protectora }, protectora);
       }

       [HttpPut("{id}")]
       public async Task<IActionResult> UpdateProtectora(int id, Protectora updatedProtectora)
       {
           var existingProtectora = await _repository.GetByIdAsync(id);
           if (existingProtectora == null)
           {
               return NotFound();
           }

           existingProtectora.Nombre_Protectora = updatedProtectora.Nombre_Protectora;
           existingProtectora.Direccion = updatedProtectora.Direccion;
           existingProtectora.Email = updatedProtectora.Email;
           existingProtectora.Telefono = updatedProtectora.Telefono;
           existingProtectora.Pagina_Web = updatedProtectora.Pagina_Web;
           existingProtectora.Imagen_Protectora = updatedProtectora.Imagen_Protectora;

           await _repository.UpdateAsync(existingProtectora);
           return NoContent();
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteProtectora(int id)
       {
           var protectora = await _repository.GetByIdAsync(id);
           if (protectora == null)
           {
               return NotFound();
           }
           await _repository.DeleteAsync(id);
           return NoContent();
       }
   }
}
