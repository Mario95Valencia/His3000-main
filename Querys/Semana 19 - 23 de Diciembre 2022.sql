---------------------------------------------------------------------Pre ingreso Pablo cambiar tabla 
USE [His3000]
GO

/***** Object:  Table [dbo].[PREADMISION]    Script Date: 14/12/2022 13:24:18 *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PREADMISION](
	[PRE_CODIGO] [bigint] IDENTITY(1,1) NOT NULL,
	[PRE_IDENTIFICACION] [nvarchar](25) NULL,
	[PRE_NOMBRE1] [nvarchar](255) NULL,
	[PRE_NOMBRE2] [nvarchar](255) NULL,
	[PRE_APELLIDO1] [nvarchar](255) NULL,
	[PRE_APELLIDO2] [nvarchar](255) NULL,
	[PRE_DIRECCION] [nvarchar](500) NULL,
	[PRE_TELEFONO] [nvarchar](15) NULL,
	[PRE_CELULAR] [nvarchar](15) NULL,
	[PRE_EMAIL] [nvarchar](50) NULL,
	[MED_CODIGO] [bigint] NULL,
	[TIA_CODIGO] [int] NULL,
	[TIR_CODIGO] [int] NULL,
	[PRE_ESTADO] [bit] NULL,
	[PRE_FECHA] [datetime] NULL,
	[ID_USUARIO] [int] NULL,
	[ATENCION] [bigint] NULL,
	[SEGURO_MEDICO] [varchar](500) NULL,
	[PRIORIDAD] [int] NULL,
	[PROCEDIMIENTO] [varchar](5000) NULL,
	[ESTADO] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PRE_CODIGO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PREADMISION] ADD  CONSTRAINT [DF_PREADMISION_ESTADO]  DEFAULT ((1)) FOR [ESTADO]
GO
