USE [SystemaDeTurnos]
GO

/****** Object:  Table [dbo].[Paciente]    Script Date: 1/13/2020 9:15:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Paciente](
	[Id_Paciente] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Dni] [int] NOT NULL,
	[Fecha_Nac] [date] NULL,
	[Direccion] [varchar](100) NULL,
	[Telefono] [varchar](20) NULL,
	[Telefono_Contacto] [varchar](20) NULL,
	[Obra_Social] [varchar](50) NULL,
	[Numero_OS] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Dni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Medico](
	[Id_Medico] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Apellido] [varchar](50) NOT NULL,
	[Dni] [int] NOT NULL,
	[Matricula] [int] NOT NULL,
	[Telefono] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Medico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Dni] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE [SystemaDeTurnos]
GO

/****** Object:  Table [dbo].[Observacion]    Script Date: 1/13/2020 9:17:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Observacion](
	[Id_Observacion] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [date] NULL,
	[Id_Paciente] [int] NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Detalle] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Observacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Observacion]  WITH CHECK ADD  CONSTRAINT [FK_Observacion_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medico] ([Id_Medico])
GO

ALTER TABLE [dbo].[Observacion] CHECK CONSTRAINT [FK_Observacion_Medico]
GO

ALTER TABLE [dbo].[Observacion]  WITH CHECK ADD  CONSTRAINT [FK_Observacion_Paciente] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Paciente] ([Id_Paciente])
GO

ALTER TABLE [dbo].[Observacion] CHECK CONSTRAINT [FK_Observacion_Paciente]
GO

USE [SystemaDeTurnos]
GO

/****** Object:  Table [dbo].[Ficha]    Script Date: 1/13/2020 9:18:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ficha](
	[Id_Ficha] [int] IDENTITY(1,1) NOT NULL,
	[Id_Paciente] [int] NOT NULL,
	[Fecha_Ingreso] [date] NULL,
	[Diagnostico] [varchar](200) NULL,
	[Antecedentes] [varchar](200) NULL,
	[Contraindicaciones] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Ficha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ficha]  WITH CHECK ADD  CONSTRAINT [FK_Ficha_Paciente] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Paciente] ([Id_Paciente])
GO

ALTER TABLE [dbo].[Ficha] CHECK CONSTRAINT [FK_Ficha_Paciente]
GO

USE [SystemaDeTurnos]
GO

/****** Object:  Table [dbo].[Tratamiento]    Script Date: 1/13/2020 9:18:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tratamiento](
	[Id_Tratamiento] [int] IDENTITY(1,1) NOT NULL,
	[Id_Paciente] [int] NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Fecha] [date] NULL,
	[Detalle] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Tratamiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Tratamiento]  WITH CHECK ADD  CONSTRAINT [FK_Tratamiento_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medico] ([Id_Medico])
GO

ALTER TABLE [dbo].[Tratamiento] CHECK CONSTRAINT [FK_Tratamiento_Medico]
GO

ALTER TABLE [dbo].[Tratamiento]  WITH CHECK ADD  CONSTRAINT [FK_Tratamiento_Paciente] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Paciente] ([Id_Paciente])
GO

ALTER TABLE [dbo].[Tratamiento] CHECK CONSTRAINT [FK_Tratamiento_Paciente]
GO

USE [SystemaDeTurnos]
GO

/****** Object:  Table [dbo].[Turno]    Script Date: 1/13/2020 9:19:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Turno](
	[Id_Turno] [int] IDENTITY(1,1) NOT NULL,
	[Id_Paciente] [int] NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Fecha_Inicio] [date] NOT NULL,
	[Fecha_Fin] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Turno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Turno]  WITH CHECK ADD  CONSTRAINT [FK_Turno_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medico] ([Id_Medico])
GO

ALTER TABLE [dbo].[Turno] CHECK CONSTRAINT [FK_Turno_Medico]
GO

ALTER TABLE [dbo].[Turno]  WITH CHECK ADD  CONSTRAINT [FK_Turno_Paciente] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Paciente] ([Id_Paciente])
GO

ALTER TABLE [dbo].[Turno] CHECK CONSTRAINT [FK_Turno_Paciente]
GO

create table Color(
 Id_Color int primary key identity(1,1) not null,
 Hex varchar(10)
 )

 insert into Color(Hex) values
('#FFC0CB'),
('#DB7093'),
('#4682B4'),
('#FFE4B5'),
('#F0FFFF'),
('#BA55D3'),
('#6495ED'),
('#E6E6FA'),
('#00008B'),
('#FFF0F5'),
('#FFFFF0'),
('#FAF0E6'),
('#FAEBD7'),
('#CD853F'),
('#F0E68C'),
('#2E8B57'),
('#8B008B'),
('#5F9EA0'),
('#87CEEB'),
('#FF8C00'),
('#7B68EE'),
('#1E90FF'),
('#BDB76B'),
('#9932CC'),
('#800080'),
('#D8BFD8'),
('#556B2F'),
('#0000CD'),
('#FA8072'),
('#ADD8E6'),
('#DA70D6'),
('#008B8B'),
('#7B68EE'),
('#FFEBCD'),
('#FFFACD'),
('#00CED1'),
('#B0E0E6'),
('#0000FF'),
('#FAFAD2'),
('#F4A460'),
('#6B8E23'),
('#ADFF2F'),
('#FFA07A'),
('#FF00FF'),
('#3CB371'),
('#00FF00'),
('#FFF5EE'),
('#FF00FF'),
('#008080'),
('#FFDAB9');