namespace Models;

public class Protectora {

    public int Id_Protectora {get; set;}
    public string Nombre_Protectora {get; set;} = "";
    public string Direccion {get; set;} = "";
    public string Correo_Protectora {get; set;} = "";
    public string Telefono_Protectora {get; set;} = "";
    public string Pagina_Web {get;set;} = "";
    public string Imagen_Protectora {get;set;} = "";
    public int Id_Usuario {get; set;}

    public Protectora() {}

    public Protectora(string nombre_Protectora, string direccion, string telefono_Protectora, string correo_Protectora, string pagina_Web, string imagen_Protectora, int id_Usuario) {
        Nombre_Protectora = nombre_Protectora;
        Direccion = direccion;
        Telefono_Protectora = telefono_Protectora;
        Correo_Protectora = correo_Protectora;
        Pagina_Web = pagina_Web;
        Imagen_Protectora = imagen_Protectora;
        Id_Usuario = id_Usuario;
    }

    public void MostrarDetalles() {
        Console.WriteLine($"Protectora: {Nombre_Protectora}, Dirección: {Direccion}, Teléfono: {Telefono_Protectora}, Correo_Protectora: {Correo_Protectora}");
    }
}
