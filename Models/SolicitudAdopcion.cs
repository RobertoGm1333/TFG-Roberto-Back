namespace Models;

public class SolicitudAdopcion {
    public int Id_Solicitud { get; set; }
    public int Id_Usuario { get; set; }
    public int Id_Gato { get; set; }
    public DateTime Fecha_Solicitud { get; set; }
    public string Estado { get; set; } = "";
    public string? NombreCompleto { get; set; }
    public int? Edad { get; set; }
    public string? Direccion { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? TipoVivienda { get; set; }
    public string? PropiedadAlquiler { get; set; }
    public bool? PermiteAnimales { get; set; }
    public int? NumeroPersonas { get; set; }
    public bool? HayNinos { get; set; }
    public string? EdadesNinos { get; set; }
    public bool? ExperienciaGatos { get; set; }
    public bool? TieneOtrosAnimales { get; set; }
    public bool? CortarUnas { get; set; }
    public bool? AnimalesVacunadosEsterilizados { get; set; }
    public string? HistorialMascotas { get; set; }
    public string? MotivacionAdopcion { get; set; }
    public string? ProblemasComportamiento { get; set; }
    public string? EnfermedadesCostosas { get; set; }
    public string? Vacaciones { get; set; }
    public bool? SeguimientoPostAdopcion { get; set; }
    public bool? VisitaHogar { get; set; }
    public string? Comentario_Protectora { get; set; }

    public SolicitudAdopcion() {}

    public SolicitudAdopcion(int id_Usuario, int id_Gato, string comentario_Protectora) {
        Id_Usuario = id_Usuario;
        Id_Gato = id_Gato;
        Comentario_Protectora = comentario_Protectora;
    }

    public void MostrarDetalles() {
        Console.WriteLine($"Solicitud #{Id_Solicitud} - Usuario {Id_Usuario} quiere adoptar al gato {Id_Gato}. Estado: {Estado}");
    }
}
