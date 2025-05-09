using Microsoft.AspNetCore.Mvc;
using Models;
using ProtectoraAPI.Repositories;
using ProtectoraAPI.Services;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace ProtectoraAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatherineController : ControllerBase
    {
        private readonly IGatoRepository _gatoRepository;

        public CatherineController(IGatoRepository gatoRepository)
        {
            _gatoRepository = gatoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ConsultarIA([FromBody] ConsultaIARequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Mensaje))
                return BadRequest(new { error = "El mensaje no puede estar vacío." });

            try
            {
                var gatos = await _gatoRepository.GetAllAsync();

                if (gatos == null || gatos.Count == 0)
                    return NotFound(new { error = "No hay gatos disponibles." });

                var prompt = PromptService.ConstruirPrompt(request.Mensaje, gatos);

                var body = new
                {
                    model = "catherine",
                    prompt = prompt,
                    stream = false
                };

                using var httpClient = new HttpClient();
                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:11434/api/generate", content);
                if (!response.IsSuccessStatusCode)
                    return StatusCode(502, new { error = "Error al comunicarse con el modelo Catherine." });

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonDocument.Parse(jsonResponse);
                var respuesta = result.RootElement.GetProperty("response").GetString();

                return Ok(new { respuesta = respuesta?.Trim() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno", detalles = ex.Message });
            }
        }
    }

    public class ConsultaIARequest
    {
        public string Mensaje { get; set; }
    }
}
