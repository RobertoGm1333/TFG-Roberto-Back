namespace Models;

public class Usuario {

    public int Id_Usuario {get; set;}
    public string Nombre {get; set;} = "";
    public string Apellido {get; set;} = "";
    public string Contraseña {get; set;} = "";
    public string Email {get; set;} = "";
    public DateTime Fecha_Registro {get; set;} = DateTime.Now;

    public Usuario() {}

    public Usuario(string nombre, string apellido, string contraseña, string email, DateTime fecha_Registro) {
        Nombre = nombre;
        Apellido = apellido;
        Contraseña = contraseña;
        Email = email;
        Fecha_Registro = fecha_Registro;
    }

    public void MostrarDetalles() {
        Console.WriteLine($"Usuario: {Nombre} {Apellido}, Contraseña: {Contraseña}, Email: {Email} y Fecha del Registro: {Fecha_Registro}");
    }
}
