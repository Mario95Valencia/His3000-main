
ALTER TABLE HC_INTERCONSULTA
ADD HIN_ESTADO BIT DEFAULT 1

UPDATE HC_INTERCONSULTA SET HIN_ESTADO = 1


create procedure sp_TipoEmpresa
@ate_codigo bigint
as
select TE.TE_CODIGO from ATENCIONES A
INNER JOIN ATENCION_DETALLE_CATEGORIAS ADC ON A.ATE_CODIGO = ADC.ATE_CODIGO
INNER JOIN CATEGORIAS_CONVENIOS CC ON ADC.CAT_CODIGO = CC.CAT_CODIGO
INNER JOIN ASEGURADORAS_EMPRESAS AE ON CC.ASE_CODIGO = AE.ASE_CODIGO
INNER JOIN TIPO_EMPRESA TE ON AE.TE_CODIGO = TE.TE_CODIGO
where A.ATE_CODIGO = @ate_codigo
go




CREATE PROCEDURE sp_FichaMedico
@CodigoMedico INT
AS
select MED_CODIGO, CONCAT(MED_APELLIDO_PATERNO, ' ' , MED_APELLIDO_MATERNO,  ' ' ,MED_NOMBRE1,  ' ' ,MED_NOMBRE2) as DATOS
, MED_RUC AS RUC, MED_FECHA_NACIMIENTO AS FECHA_NACIMIENTO, MED_GENERO AS GENERO, MED_EMAIL AS MAIL,
MED_FECHA AS FechaIngreso, MED_DIRECCION_CONSULTORIO as DireccionConsultorio, EM.ESP_NOMBRE AS Especialidad,
MED_TELEFONO_CASA AS TelefonoCasa, MED_TELEFONO_CONSULTORIO AS TelefonoConsultorio, MED_TELEFONO_CELULAR AS TelefonoCelular,
TIM_NOMBRE AS TipoMedico, TIH_NOMBRE AS TipoHonorario, BAN_NOMBRE AS BANCO, MED_NUM_CUENTA AS CuentaMedico,
MED_TIPO_CUENTA AS Tipo_Cuenta, MED_CUENTA_CONTABLE AS CuentaContable, MED_AUTORIZACION_SRI AS AutorizacionSri,
MED_VALIDEZ_AUTORIZACION AS ValidezAutorizacion, MED_FACTURA_INICIAL AS FacturaInicial, MED_FACTURA_FINAL AS FacturaFinal,
RET_DESCRIPCION AS Retencion, RET_PORCENTAJE AS Porcentaje
from MEDICOS M 
INNER JOIN ESPECIALIDADES_MEDICAS EM ON M.ESP_CODIGO = EM.ESP_CODIGO
INNER JOIN TIPO_MEDICO TE ON M.TIM_CODIGO = TE.TIM_CODIGO
INNER JOIN TIPO_HONORARIO TH ON M.TIH_CODIGO = TH.TIH_CODIGO
INNER JOIN BANCOS B ON M.BAN_CODIGO = B.BAN_CODIGO
INNER JOIN RETENCIONES_FUENTE RF ON M.RET_CODIGO = RF.RET_CODIGO
GO


USE [His3000]
GO
/***** Object:  StoredProcedure [dbo].[sp_AnticiposSic]    Script Date: 12/04/2021 14:41:23 *****/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_AnticiposSic]
(
  @Ate_Codigo AS VARCHAR(30)
)
AS
BEGIN

    Select a.numrec Codigo, a.monto Monto, c.nomcli Nombre, c.ruccli Identificacion, (select ATE_NUMERO_ATENCION from ATENCIONES 
				where ATE_CODIGO=@Ate_Codigo) as Ate_Numero  
	from sic3000..Anticipo a, sic3000..Cliente c
	where a.codcli = c.codcli
	and c.ruccli = (select PAC_IDENTIFICACION from PACIENTES where
				PAC_CODIGO=(select PAC_CODIGO from ATENCIONES 
				where ATE_CODIGO=@Ate_Codigo))
	--and a.ATE_NUMERO_ATENCION= (select ATE_NUMERO_ATENCION from ATENCIONES 
	--			where ATE_CODIGO=@Ate_Codigo)
	and a.estado=1 order by 1 asc 

END


USE [His3000]
GO

/***** Object:  Table [dbo].[PathFacturaElectronicaArchivo]    Script Date: 12/04/2021 14:49:14 *****/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PathFacturaElectronicaArchivo](
	[IdPath] [bigint] IDENTITY(1,1) NOT NULL,
	[Ate_codigo] [bigint] NOT NULL,
	[Path] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_PathFacturaElectronicaArchivo] PRIMARY KEY CLUSTERED 
(
	[IdPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE PROCEDURE sp_PathFacturaElectronicaArchivo
	@Ate_codigo as bigint,
	@Path varchar(1000)
AS
BEGIN 

	INSERT INTO PathFacturaElectronicaArchivo VALUES (@Ate_codigo, @Path)

END



CREATE PROCEDURE sp_PathFacturaElectronicaArchivo
	@Ate_codigo as bigint,
	@Path varchar(1000)
AS
BEGIN 

	INSERT INTO PathFacturaElectronicaArchivo VALUES (@Ate_codigo, @Path)

END




