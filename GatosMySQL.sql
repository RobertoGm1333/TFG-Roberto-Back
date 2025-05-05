CREATE DATABASE GatosDB;

USE GatosDB;

CREATE TABLE Usuario (
    Id_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    Contraseña VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Fecha_Registro DATETIME NOT NULL,
);

INSERT INTO Usuario (Nombre, Apellido, Contraseña, Email, Fecha_Registro)
VALUES 
('Juan', 'Juanez', 'juan123', 'juanjuan@gmail.com', SYSDATETIME()),
('Fran', 'Franez', 'fran123', 'franfran@gmail.com', SYSDATETIME());

SELECT * FROM Usuario;


CREATE TABLE Protectora (
    Id_Protectora INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Protectora VARCHAR(100) NOT NULL,
    Direccion VARCHAR(100) NOT NULL,
    Correo_Protectora VARCHAR(100) NOT NULL,
    Telefono_Protectora VARCHAR(15) NOT NULL,
    Pagina_Web VARCHAR(100) NOT NULL,
    Imagen_Protectora VARCHAR(100) NOT NULL,
);

INSERT INTO Protectora (Nombre_Protectora, Direccion, Correo_Protectora, Telefono_Protectora, Pagina_Web, Imagen_Protectora)
VALUES 
('Bigotes Callejeros', 'El Picarral', 'Bigotescallejeros@gmail.com', '123456789', 'https://bigotescallejeros.wordpress.com/', '/Images/protectoras/BigotesCallejeros.png'),
('Adala', 'Casco antiguo', 'adala@gmail.com', '14141414', 'www.adalazaragoza.com', '/Images/protectoras/Adala.png');

SELECT * FROM Protectora;


CREATE TABLE Gato (
    Id_Gato INT IDENTITY(1,1) PRIMARY KEY,
    Id_Protectora INT NOT NULL,
    Nombre_Gato VARCHAR(100) NOT NULL,
    Raza VARCHAR(100) NOT NULL,
    Edad INT NOT NULL,
    Esterilizado BIT NOT NULL,
    Sexo VARCHAR(10) NOT NULL,
    Descripcion_Gato VARCHAR(1000) NOT NULL,
    Imagen_Gato VARCHAR(100),
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


SELECT * FROM Gato;


CREATE TABLE Deseados (
    Id_Deseado INT IDENTITY(1,1) PRIMARY KEY,
    Id_Usuario INT NOT NULL,
    Id_Gato INT NOT NULL,
    Fecha_Deseado DATETIME NOT NULL,
    FOREIGN KEY (Id_Usuario) REFERENCES Usuario(Id_Usuario) ON DELETE CASCADE,
    FOREIGN KEY (Id_Gato) REFERENCES Gato(Id_Gato) ON DELETE CASCADE,
);

INSERT INTO Deseados (Id_Usuario, Id_Gato, Fecha_Deseado)
VALUES
(1, 2, SYSDATETIME()),
(1, 2, SYSDATETIME());

SELECT * FROM Deseados;