using System.Text;
using Models;

namespace ProtectoraAPI.Services
{
    public static class PromptService
    {
        public static string ConstruirPrompt(string mensajeUsuario, List<Gato> gatos)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Usuario: {mensajeUsuario}\n");
            sb.AppendLine("Gatos disponibles:");

            foreach (var g in gatos)
            {
                sb.AppendLine($"{g.Nombre_Gato}, {g.Edad} años, {g.Sexo}, raza {g.Raza}, descripción: {g.Descripcion_Gato}");
            }

            sb.AppendLine("\nTu respuesta:");
            return sb.ToString();
        }
    }
}
