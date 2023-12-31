CREATE PROCEDURE sp_ADMISIONES_nuevo
(@codigoPaciente int)  
AS 
BEGIN
 
 select top 20 ROW_NUMBER() OVER(
 ORDER BY ATE_FECHA_INGRESO) AS N_ORDEN, a.ATE_CODIGO AS ATE_CODIGO, a.ATE_FECHA_INGRESO AS FECHA_INGRESO,
 floor((cast(convert(varchar(8),a.ATE_FECHA_INGRESO,112) as int)-cast(convert(varchar(8),p.PAC_FECHA_NACIMIENTO,112) as int) ) / 10000) as EDAD,
 (M.MED_APELLIDO_PATERNO + ' ' + M.MED_NOMBRE1) as REFERIDO_DE, U.USR  AS COD_ADMISIONISTA, A.PAC_CODIGO AS COD_PACIENTE, 
 '' as PRIMERA,'X' as SUBSEC 
 from ATENCIONES a
 inner join PACIENTES p on a.PAC_CODIGO = p.PAC_CODIGO
 inner join MEDICOS m on a.MED_CODIGO = m.MED_CODIGO
 inner join USUARIOS u on a.ID_USUSARIO = u.ID_USUARIO
 where a.PAC_CODIGO=@codigoPaciente order by ATE_FECHA_INGRESO desc

END