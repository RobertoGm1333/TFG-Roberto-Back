namespace Models;

public class SolicitudAdopcion {

    public int Id_Solicitud { get; set; }
    public int Id_Usuario { get; set; }
    public int Id_Gato { get; set; }
    public DateTime Fecha_Solicitud { get; set; } = DateTime.Now;
    public string Estado { get; set; } = "pendiente";
    public string Comentario_Usuario { get; set; } = "";
    public string Comentario_Protectora { get; set; } = "";

    public SolicitudAdopcion() {}

    public SolicitudAdopcion(int id_Usuario, int id_Gato, string comentario_Usuario, string comentario_Protectora) {
        Id_Usuario = id_Usuario;
        Id_Gato = id_Gato;
        Comentario_Usuario = comentario_Usuario;
        Comentario_Protectora = comentario_Protectora;
    }

    public void MostrarDetalles() {
        Console.WriteLine($"Solicitud #{Id_Solicitud} - Usuario {Id_Usuario} quiere adoptar al gato {Id_Gato}. Estado: {Estado}");
    }
}
