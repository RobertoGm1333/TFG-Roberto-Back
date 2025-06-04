CREATE DATABASE GatosDB;
USE GatosDB;

-- Tabla Usuario
CREATE TABLE Usuario (
    Id_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Contraseña VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Fecha_Registro DATETIME NOT NULL,
    Rol VARCHAR(20) NOT NULL DEFAULT 'usuario',
    Activo BIT NOT NULL DEFAULT 1
);

INSERT INTO Usuario (Nombre, Apellido, Contraseña, Email, Fecha_Registro, Rol, Activo)
VALUES 
('Roberto', 'Gomez', 'rob123', 'robadmin@gmail.com', SYSDATETIME(), 'admin', 1),
('Fran', 'Franez', 'fran123', 'franfran@gmail.com', SYSDATETIME(), 'protectora', 1),
('Paco', 'Sulivan', 'paco123', 'pacopaco@gmail.com', SYSDATETIME(), 'protectora', 1),
('Zarpa', 'Zarpa', 'Zarpa123', 'info@zarpa.org', SYSDATETIME(), 'protectora', 1),
('Gatolandia', 'Gatolandia', 'Gatolandia123', 'gatolandiazgz@gmail.com', SYSDATETIME(), 'protectora', 1),
('Juan', 'Franez', 'juan123', 'juanjuan@gmail.com', SYSDATETIME(), 'usuario', 1),
('Adriana', 'Franez', 'adriana123', 'adrianaadriana@gmail.com', SYSDATETIME(), 'usuario', 1);

-- Tabla Protectora
CREATE TABLE Protectora (
    Id_Protectora INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Protectora VARCHAR(100) NOT NULL,
    Direccion VARCHAR(100) NOT NULL,
    Ubicacion VARCHAR(5000) NOT NULL,
    Correo_Protectora VARCHAR(100) NOT NULL,
    Telefono_Protectora VARCHAR(15) NOT NULL,
    Pagina_Web VARCHAR(100) NOT NULL,
    Imagen_Protectora VARCHAR(5000) NOT NULL,
    Descripcion_Protectora VARCHAR(1000) NOT NULL,
    Id_Usuario INT NOT NULL,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario)
);

INSERT INTO Protectora (Nombre_Protectora, Direccion, Ubicacion, Correo_Protectora, Telefono_Protectora, Pagina_Web, Imagen_Protectora, Descripcion_Protectora, Id_Usuario)
VALUES 
('Bigotes Callejeros', 'Zaragoza', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5962.117626099076!2d-0.8885988235248283!3d41.65447127938542!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0xd5914ea701a7919%3A0x1cd9cb8c1bef89d4!2sC.%20del%20Conde%20de%20Aranda%2C%2015%2C%20Casco%20Antiguo%2C%2050004%20Zaragoza!5e0!3m2!1ses!2ses!4v1747118352684!5m2!1ses!2ses', 'bigotescallejeroszgz@gmail.com', '123456789', 'https://bigotescallejeros.wordpress.com/', '/Images/protectoras/BigotesCallejeros.png', 'Somos una pequeña protectora de animales de Zaragoza que se encarga de velar por el bienestar de los gatos abandonados y darles la calidad de vida que merecen.' ,2),
('Adala', 'Zaragoza', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2982.4430154893716!2d-0.9094092235259463!3d41.62455468124995!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0xd596acc8696b2c1%3A0x81fd841dde9baa53!2sP.%C2%BA%20de%20los%20Infantes%20de%20Espa%C3%B1a%2C%2050012%2C%20Zaragoza!5e0!3m2!1ses!2ses!4v1747115917216!5m2!1ses!2ses','adalazaragoza@gmail.com', '654 616 982', 'https://adalazaragoza.com/', '/Images/protectoras/Adala.png', 'ADALA es una asociación sin ánimo de lucro cuyo objetivo es mejorar la vida de animales maltratados y/o abandonados. ADALA está compuesta por una red de casas de acogida que abren las puertas de su hogar a nuestros animales hasta que son adoptados. No contamos con refugio propio.' ,3),
('Z.A.R.P.A.', 'Zaragoza', 'https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2981.6737879318166!2d-0.8878358239275067!3d41.64118207126875!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0xd5914e0eb716f37%3A0xe5b13ba935b9bcd3!2sC.%20de%20Crist%C3%B3bal%20Col%C3%B3n%2C%206%2C%2050007%20Zaragoza!5e0!3m2!1ses!2ses!4v1747387190914!5m2!1ses!2ses', 'info@zarpa.org', '123456789', 'https://zarpa.org/', '/Images/protectoras/Zarpa.png', 'Zarpa es una asociación sin ánimo de lucro que se dedica a mejorar la vida de los animales abandonados o maltratados. Te invitamos a colaborar, hay muchas formas de hacerlo…' ,4),
('Gatolandia', 'Zaragoza', 'https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d190783.12323243095!2d-0.877433!3d41.656038!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0xd5914ed1a21b48b%3A0xb215a2ea52d5adc7!2sAyuntamiento%20de%20Zaragoza!5e0!3m2!1ses!2sus!4v1747387576444!5m2!1ses!2sus', 'gatolandiazgz@gmail.com', '123456789', 'https://gatolandiazgz.wordpress.com/', '/Images/protectoras/Gatolandia.png', 'Somos una Asociación de Zaragoza sin ánimo de lucro, que rescata gatos abandonados, les curamos las heridas que hayan podido sufrir, difundimos su historia y fotos a través de redes sociales y les buscamos un buen hogar. No cobramos nada, solo nos interesa el bienestar del animal. Lo que nos gustaría es poder llegar a salvar muchos más de esos gatitos abandonados que muchas veces, por falta de dinero, no podemos atender. El importe se destinará íntegro a pagar los gastos veterinarios.' ,5);

-- Tabla Gato
CREATE TABLE Gato (
    Id_Gato INT IDENTITY(1,1) PRIMARY KEY,
    Id_Protectora INT NOT NULL,
    Nombre_Gato VARCHAR(100) NOT NULL,
    Raza VARCHAR(100) NOT NULL,
    Edad INT NOT NULL,
    Esterilizado BIT NOT NULL,
    Sexo VARCHAR(10) NOT NULL,
    Descripcion_Gato VARCHAR(1000) NOT NULL,
    Imagen_Gato VARCHAR(5000),
    Visible BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (Id_Protectora) REFERENCES Protectora(Id_Protectora)
);
 
INSERT INTO Gato (Id_Protectora, Nombre_Gato, Raza, Edad, Esterilizado, Sexo, Descripcion_Gato, Imagen_Gato)
VALUES 
(2, 'Widow', 'Pardo', 4, 0, 'Macho', 'Al haber vivido mucho tiempo en la calle es algo desconfiado. Necesita que le den su espacio para no sentirse amenazado.', '/Images/gatos/Widow.png'),
(2, 'Claudia', 'Gris', 1, 1, 'Hembra', 'Tiene el típico carácter de un gato, cercana pero cuando ella quiere.', '/Images/gatos/Claudia.png'),
(2, 'Sira', 'Pardo', 1, 1, 'Hembra', 'Es una gata que se encontró en un polígono y al principio es un poco tímida pero con un poco de paciencia es muy cariñosa.', '/Images/gatos/Sira.png'),
(2, 'Milu', 'Tuxedo', 7, 1, 'Macho', 'Es muy bueno.', '/Images/gatos/Milu.png'),
(2, 'Lupita', 'Blanca', 1, 1, 'Hembra', 'Necesita una familia con paciencia, tiene muchos miedos y necesita tiempo para volver a confiar.', '/Images/gatos/Lupita.png'),
(2, 'Charlotte', 'Tuxedo', 1, 1, 'Hembra', 'Es muy buena y un amor.', '/Images/gatos/Charlotte.png'),
(1, 'Martita', 'Naranja y negro', 3, 1, 'Hembra', 'Es muy sociable, tranquila y se adapta a otros gatos.', '/Images/gatos/Martita.png'),
(1, 'Tito', 'Pardo', 1, 1, 'Macho', 'Hubo que amputarle el rabo por una infección pero no le impide jugar y dar cariño.', '/Images/gatos/Tito.png'),
(1, 'Melocotón', 'Naranja y negro', 2, 1, 'Macho', 'Necesita compañía y se lleva genial con otros gatos y personas.', '/Images/gatos/Melocoton.png'),
(1, 'Lucas', 'Pardo', 1, 1, 'Macho', 'Necesita una adopción estable, alguien que realmente ame a los animales y tenga paciencia para respetar su espacio, y que poco a poco se vaya acercando.', '/Images/gatos/Lucas.png'),
(1, 'Chloe', 'Blanco y pardo', 1, 1, 'Hembra', 'Necesita una adopción estable, alguien que realmente ame a los animales y tenga paciencia para respetar su espacio, y que poco a poco se vaya acercando.', '/Images/gatos/Chloe.png'),
(1, 'Carter', 'Blanco y pardo', 1, 1, 'Macho', 'Es un cachorrito muy juguetón al que le encanta socializar y pasar el rato con todo el mundo.', '/Images/gatos/Carter.png');

-- Tabla Deseados
CREATE TABLE Deseados (
    Id_Deseado INT IDENTITY(1,1) PRIMARY KEY,
    Id_Usuario INT NOT NULL,
    Id_Gato INT NOT NULL,
    Fecha_Deseado DATETIME NOT NULL,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario) ON DELETE CASCADE,
    FOREIGN KEY (Id_Gato) REFERENCES Gato(Id_Gato) ON DELETE CASCADE
);

INSERT INTO Deseados (Id_Usuario, Id_Gato, Fecha_Deseado)
VALUES
(1, 2, SYSDATETIME()),
(1, 1, SYSDATETIME());

-- NUEVA Tabla SolicitudAdopcionExtendida (reemplazo completo)
CREATE TABLE SolicitudAdopcion (
    Id_Solicitud INT IDENTITY(1,1) PRIMARY KEY,
    Id_Usuario INT NOT NULL,
    Id_Gato INT NOT NULL,
    Fecha_Solicitud DATETIME NOT NULL DEFAULT GETDATE(),
    Estado VARCHAR(20) NOT NULL DEFAULT 'pendiente',
    NombreCompleto VARCHAR(100),
    Edad INT,
    Direccion VARCHAR(255),
    DNI VARCHAR(20),
    Telefono VARCHAR(20),
    Email VARCHAR(100),
    TipoVivienda VARCHAR(50),
    PropiedadAlquiler VARCHAR(50),
    PermiteAnimales BIT,
    NumeroPersonas INT,
    HayNinos BIT,
    EdadesNinos VARCHAR(100),
    ExperienciaGatos BIT,
    TieneOtrosAnimales BIT,
    CortarUnas BIT,
    AnimalesVacunadosEsterilizados BIT,
    HistorialMascotas VARCHAR(1000),
    MotivacionAdopcion VARCHAR(1000),
    ProblemasComportamiento VARCHAR(1000),
    EnfermedadesCostosas VARCHAR(1000),
    Vacaciones VARCHAR(1000),
    SeguimientoPostAdopcion BIT,
    VisitaHogar BIT,
    Comentario_Protectora VARCHAR(1000),
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario),
    FOREIGN KEY (Id_Gato) REFERENCES Gato(Id_Gato)
);

