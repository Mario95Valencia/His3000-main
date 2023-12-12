USE [Cg3000]
GO

/****** Object:  View [dbo].[saldosCueprin]    Script Date: 28/03/2022 12:29:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER view [dbo].[saldosCueprin]
as
(
select  cp.codcue_cp,cp.nomcue_cp,
isnull((select (sum(dm.debe-dm.haber)) as saldo from Cgdetmae dm where dm.codcue_cp =cp.codcue_cp ),0) as saldo
from Cgcueprin cp
--order by codcue_cp 
)
GO

---------------------------------------------------------------------
CREATE VIEW Vista_HonorariosCxE
as
SELECT M.MED_CODIGO AS CODIGO, 
M.MED_APELLIDO_PATERNO + ' ' + M.MED_APELLIDO_MATERNO + ' ' + M.MED_NOMBRE1 + ' ' + M.MED_NOMBRE2 AS MEDICO,
COUNT(HM.HOM_CODIGO) AS CANTIDAD, SUM(HM.HOM_VALOR_TOTAL) AS TOTAL
FROM His3000..MEDICOS M
INNER JOIN His3000..HONORARIOS_MEDICOS HM ON M.MED_CODIGO = HM.MED_CODIGO
INNER JOIN His3000..ATENCIONES A ON HM.ATE_CODIGO = A.ATE_CODIGO
WHERE A.TIP_CODIGO = 4
GROUP BY M.MED_CODIGO, M.MED_APELLIDO_PATERNO + ' ' + M.MED_APELLIDO_MATERNO + ' ' + M.MED_NOMBRE1 + ' ' + M.MED_NOMBRE2
go

------------------------------------------------------------------

CREATE VIEW PRODUCTOS_SIC3000
AS
SELECT * FROM SIC3000..Producto
GO

CREATE VIEW QUIROFANO_PROCEDIMIENTOS
AS
SELECT PC.PCI_CODIGO AS CODIGO, PCI_DESCRIPCION AS PROCEDIMIENTO,
COUNT(PC.PCI_CODIGO) AS CANTIDAD, PCI_BODEGA AS BODEGA
FROM PROCEDIMIENTOS_CIRUGIA PC
INNER JOIN QUIROFANO_PROCE_PRODU QP ON PC.PCI_CODIGO = QP.PCI_CODIGO
WHERE ATE_CODIGO IS NULL AND PC.PCI_ESTADO = 1
GROUP BY PC.PCI_CODIGO, PCI_DESCRIPCION, PCI_BODEGA
GO

-----------------------------------------------------------------
ALTER TABLE PROCEDIMIENTOS_CIRUGIA ADD PRIMARY KEY (PCI_CODIGO) 

----------------------------------------------------------------

ALTER PROCEDURE sp_QuirofanoNombreProcedimiento
@pci_descripcion nvarchar(200),  
@bodega int  
AS    
IF EXISTS(SELECT * FROM PROCEDIMIENTOS_CIRUGIA WHERE PCI_DESCRIPCION = @pci_descripcion and pci_bodega = @bodega)    
BEGIN    
 SELECT 'ESTADO' = 1 FROM PROCEDIMIENTOS_CIRUGIA WHERE PCI_DESCRIPCION = @pci_descripcion    
END   

------------------------------------------------------
USE [His3000]
GO

/****** Object:  Table [dbo].[LIQUIDACION]    Script Date: 04/04/2022 09:27:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LIQUIDACION](
	[LIQ_CODIGO] [bigint] IDENTITY(1,1) NOT NULL,
	[MED_CODIGO] [int] NULL,
	[HOM_CODIGO] [bigint] NULL,
	[ID_USUARIO] [int] NULL,
	[LIQ_LIQUIDADO] [bit] NULL,
	[LIQ_NUMDOC] [bigint] NULL,
	[LIQ_FECHA] [datetime] NULL,
	[LIQ_ASIENTO] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[LIQ_CODIGO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LIQUIDACION] ADD  DEFAULT ((0)) FOR [MED_CODIGO]
GO

ALTER TABLE [dbo].[LIQUIDACION] ADD  DEFAULT ((0)) FOR [HOM_CODIGO]
GO

ALTER TABLE [dbo].[LIQUIDACION] ADD  DEFAULT ((1)) FOR [ID_USUARIO]
GO

ALTER TABLE [dbo].[LIQUIDACION] ADD  DEFAULT ((0)) FOR [LIQ_LIQUIDADO]
GO


