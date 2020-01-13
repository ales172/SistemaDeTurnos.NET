CREATE DATABASE SystemaDeTurnos;
GO
USE SystemaDeTurnos;
GO

CREATE TABLE Paciente
(
	Id_Paciente INT PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(50) NOT NULL,
	Apellido VARCHAR(50) NOT NULL,
	Dni INT UNIQUE NOT NULL,
	Fecha_Nac DATE,
	Direccion VARCHAR(100),
	Telefono VARCHAR(20),
	Telefono_Contacto VARCHAR(20),
	Obra_Social VARCHAR(50),
	Numero_OS VARCHAR(30)
)
GO

CREATE TABLE Medico
(
	Id_Medico INT PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(50) NOT NULL,
	Apellido VARCHAR(50) NOT NULL,
	Dni INT UNIQUE NOT NULL,
	Matricula INT UNIQUE NOT NULL,
	Telefono VARCHAR(20)
)
GO

CREATE TABLE Observacion
(
	Id_Observacion INT PRIMARY KEY IDENTITY(1,1),
	Fecha DATE,
	Id_Paciente INT NOT NULL,
	Id_Medico INT NOT NULL,
	Detalle VARCHAR(500),
	CONSTRAINT FK_Observacion_Paciente
		FOREIGN KEY (Id_Paciente) 
		REFERENCES Paciente(Id_Paciente),
	CONSTRAINT FK_Observacion_Medico
		FOREIGN KEY (Id_Medico) 
		REFERENCES Medico(Id_Medico)	
)
GO

CREATE TABLE Ficha
(
	Id_Ficha INT PRIMARY KEY IDENTITY(1,1),
	Id_Paciente INT NOT NULL,
	Fecha_Ingreso DATE,
	Diagnostico VARCHAR(200),
	Antecedentes VARCHAR(200),
	Contraindicaciones VARCHAR(200),
	CONSTRAINT FK_Ficha_Paciente
		FOREIGN KEY (Id_Paciente) 
		REFERENCES Paciente(Id_Paciente)
)
GO

CREATE TABLE Tratamiento
(
	Id_Tratamiento INT PRIMARY KEY IDENTITY(1,1),
	Id_Paciente INT NOT NULL,
	Id_Medico INT NOT NULL,
	Fecha DATE,
	Detalle VARCHAR(500),
	CONSTRAINT FK_Tratamiento_Paciente
		FOREIGN KEY (Id_Paciente) 
		REFERENCES Paciente(Id_Paciente),
	CONSTRAINT FK_Tratamiento_Medico
		FOREIGN KEY (Id_Medico) 
		REFERENCES Medico(Id_Medico)
)
GO

CREATE TABLE Turno
(
	Id_Turno INT PRIMARY KEY IDENTITY(1,1),
	Id_Paciente INT NOT NULL,
	Id_Medico INT NOT NULL,
	Fecha DATE NOT NULL,
	Inicio TIME NOT NULL,
	Fin TIME NOT NULL
	CONSTRAINT FK_Turno_Paciente
		FOREIGN KEY (Id_Paciente) 
		REFERENCES Paciente(Id_Paciente),
	CONSTRAINT FK_Turno_Medico
		FOREIGN KEY (Id_Medico) 
		REFERENCES Medico(Id_Medico)
)
GO