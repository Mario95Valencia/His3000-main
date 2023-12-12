-- CREATE PROCEDURE sp_HMDentroPaciente
-- @numfac nvarchar(25)
-- AS
-- SELECT * FROM Sic3000..FacturaPago WHERE numdoc = @numfac
-- GO



ALTER PROCEDURE sp_ListaPedidosEstaciones(@CodigoEstacion as int, @EstadoDetallePedido as int,@FiltroFecha as bit,@FechaInicio as date,@FechaFin as date)      
as      
begin      
 if @EstadoDetallePedido=2      
 begin      
  if @FiltroFecha=1      
  begin      
   select       
   PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   pedidos.PED_FECHA AS [FECHA],  
   PACIENTES.PAC_APELLIDO_PATERNO + ' '  + PACIENTES.PAC_APELLIDO_MATERNO + ' ' + PACIENTES.PAC_NOMBRE1 + ' ' + PACIENTES.PAC_NOMBRE2 AS PACIENTE,
   PEE_NOMBRE AS [ESTACION] ,      
   CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,      
   MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS, PACIENTES, ATENCIONES     
   where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion  
   AND PEDIDOS.ATE_CODIGO = ATENCIONES.ATE_CODIGO
   AND ATENCIONES.PAC_CODIGO = PACIENTES.PAC_CODIGO
   --and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido      
   and CAST(CONVERT(varchar(11),ped_fecha,103) as DATE) between @FechaInicio and @FechaFin      
   order by [NUMERO PEDIDO] desc      
  end      
  else      
  begin      
   select       
   PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   pedidos.PED_FECHA AS [FECHA],  
   PACIENTES.PAC_APELLIDO_PATERNO + ' '  + PACIENTES.PAC_APELLIDO_MATERNO + ' ' + PACIENTES.PAC_NOMBRE1 + ' ' + PACIENTES.PAC_NOMBRE2 AS PACIENTE,
   PEE_NOMBRE AS [ESTACION] ,      
   CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,         
   MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS,PACIENTES, ATENCIONES       
   where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion   
      AND PEDIDOS.ATE_CODIGO = ATENCIONES.ATE_CODIGO
   AND ATENCIONES.PAC_CODIGO = PACIENTES.PAC_CODIGO
   --and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido         
   order by [NUMERO PEDIDO] desc      
        
  end      
 end      
       
 if @EstadoDetallePedido<>2      
 begin        
  if @FiltroFecha=1      
  begin      
         
   select       
   PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   pedidos.PED_FECHA AS [FECHA],      
   PACIENTES.PAC_APELLIDO_PATERNO + ' '  + PACIENTES.PAC_APELLIDO_MATERNO + ' ' + PACIENTES.PAC_NOMBRE1 + ' ' + PACIENTES.PAC_NOMBRE2 AS PACIENTE,
   PEE_DESCRIPCION AS [ESTACION] ,      
   CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,        
   MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS, PACIENTES, ATENCIONES          
   where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion      
   and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido        
   and CAST(CONVERT(varchar(11),ped_fecha,103) as DATE) between @FechaInicio and @FechaFin      
   order by [NUMERO PEDIDO] desc      
          
  end      
  else      
  begin      
   select       
   PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   pedidos.PED_FECHA AS [FECHA],      
   PACIENTES.PAC_APELLIDO_PATERNO + ' '  + PACIENTES.PAC_APELLIDO_MATERNO + ' ' + PACIENTES.PAC_NOMBRE1 + ' ' + PACIENTES.PAC_NOMBRE2 AS PACIENTE,
   PEE_DESCRIPCION AS [ESTACION] ,      
   CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,        
   MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS , PACIENTES, ATENCIONES         
   where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion      
   and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido 
      AND PEDIDOS.ATE_CODIGO = ATENCIONES.ATE_CODIGO
   AND ATENCIONES.PAC_CODIGO = PACIENTES.PAC_CODIGO
   order by [NUMERO PEDIDO] desc      
        
  end      
 end      
       
end


alter PROCEDURE sp_ReporteEdades_PacientexAtencion  
@tip_codigo int,  
@fechadesde datetime,  
@fechahasta datetime  
AS  
BEGIN  
 SELECT '1' AS 'NRO', '1 - 4 años' AS 'RANGO', ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'M'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) <= 4   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS HOMBRES,  
 ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'F'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) <= 4   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS MUJERES  
  
 UNION  
  
 SELECT '2' AS 'NRO', '5 - 14 años' AS 'RANGO', ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'M'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 5 AND 14    
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS HOMBRES,  
 ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'F'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 5 AND 14   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS MUJERES  
  
 UNION  
  
 SELECT '3' AS 'NRO', '15 - 19 años' AS 'RANGO', ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'M'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 15 AND 19   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS HOMBRES,  
 ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'F'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 15 AND 19    
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS MUJERES  
  
 UNION   
  
 SELECT '4' AS 'NRO', '20 - 49 años' AS 'RANGO', ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'M'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 20 AND 49   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS HOMBRES,  
 ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'F'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 20 AND 49   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS MUJERES  
  
 UNION  
  
 SELECT '5' AS 'NRO', '50 - 64 años' AS 'RANGO', ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'M'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 50 AND 64   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS HOMBRES,  
 ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'F'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) BETWEEN 50 AND 64  
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS MUJERES  
  
 UNION  
  
 SELECT '6' AS 'NRO', '65 > años' AS 'RANGO', ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'M'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) >= 65    
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS HOMBRES,  
 ISNULL((SELECT COUNT(P.PAC_GENERO)   
 FROM PACIENTES P  
 INNER JOIN ATENCIONES A ON P.PAC_CODIGO = A.PAC_CODIGO  
 INNER JOIN TIPO_INGRESO TI ON A.TIP_CODIGO = TI.TIP_CODIGO  
 WHERE A.TIP_CODIGO = @tip_codigo AND P.PAC_GENERO = 'F'   
 AND DATEDIFF(YEAR, PAC_FECHA_NACIMIENTO, @fechahasta) >= 65   
 AND A.ATE_FECHA_INGRESO BETWEEN @fechadesde AND @fechahasta  
 ), 0) AS MUJERES  
END  





ALTER PROCEDURE sp_ListaPedidosEstaciones(@CodigoEstacion as int, @EstadoDetallePedido as int,@FiltroFecha as bit,@FechaInicio as date,@FechaFin as date)      
as      
begin      
 if @EstadoDetallePedido=2      
 begin      
  if @FiltroFecha=1      
  begin      
   --select       
   --PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   --pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   --pedidos.PED_FECHA AS [FECHA],      
   --PEE_NOMBRE AS [ESTACION] ,      
   --CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,      
   --MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   --PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   --DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   --PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   --isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   --from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS      
   --where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   --and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   --AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   --AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   --and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion      
   ----and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido      
   --and CAST(CONVERT(varchar(11),ped_fecha,103) as DATE) between @FechaInicio and @FechaFin      
   --order by [NUMERO PEDIDO] desc      
	SELECT P.PED_CODIGO AS 'CODIGO', P.PED_FECHA AS 'F. PEDIDO', PE.PEE_NOMBRE AS ESTACION,
	PA.PAC_APELLIDO_PATERNO + ' ' + PA.PAC_APELLIDO_MATERNO + ' ' + PA.PAC_NOMBRE1 + ' ' + PA.PAC_NOMBRE2 AS PACIENTE,
	U.APELLIDOS + ' ' + U.NOMBRES AS 'PEDIDO POR',
	M.MED_APELLIDO_PATERNO + ' ' + M.MED_APELLIDO_MATERNO + ' ' + M.MED_NOMBRE1 + ' ' + M.MED_NOMBRE2 AS MEDICO,
	CP.PRO_CODIGO AS 'COD. PRODUCTO', CP.CUE_DETALLE AS PRODUCTO, CP.CUE_CANTIDAD AS CANTIDAD
	FROM PEDIDOS P 
	INNER JOIN PEDIDOS_DETALLE PD ON P.PED_CODIGO = PD.PED_CODIGO
	INNER JOIN CUENTAS_PACIENTES CP ON P.PED_CODIGO = CP.Codigo_Pedido
	LEFT JOIN PEDIDOS_ESTACIONES PE ON P.PEE_CODIGO = PE.PEE_CODIGO
	INNER JOIN USUARIOS U ON P.ID_USUARIO = U.ID_USUARIO
	INNER JOIN ATENCIONES A ON CP.ATE_CODIGO = A.ATE_CODIGO
	INNER JOIN PACIENTES PA ON A.PAC_CODIGO = PA.PAC_CODIGO
	INNER JOIN MEDICOS M ON P.MED_CODIGO = M.MED_CODIGO
	WHERE PE.PEE_CODIGO = @CodigoEstacion AND PD.PDD_ESTADO = @EstadoDetallePedido
	AND P.PED_FECHA BETWEEN @FechaInicio AND @FechaFin
	ORDER BY P.PED_FECHA DESC
  end      
  else      
  begin      
   --select       
   --PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   --pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   --pedidos.PED_FECHA AS [FECHA],      
   --PEE_NOMBRE AS [ESTACION] ,      
   --CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,         
   --MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   --PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   --DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   --PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   --isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   --from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS      
   --where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   --and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   --AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   --AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   --and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion      
   ----and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido         
   --order by [NUMERO PEDIDO] desc  
   
	SELECT P.PED_CODIGO AS 'CODIGO', P.PED_FECHA AS 'F. PEDIDO', PE.PEE_NOMBRE AS ESTACION,
	PA.PAC_APELLIDO_PATERNO + ' ' + PA.PAC_APELLIDO_MATERNO + ' ' + PA.PAC_NOMBRE1 + ' ' + PA.PAC_NOMBRE2 AS PACIENTE,
	U.APELLIDOS + ' ' + U.NOMBRES AS 'PEDIDO POR',
	M.MED_APELLIDO_PATERNO + ' ' + M.MED_APELLIDO_MATERNO + ' ' + M.MED_NOMBRE1 + ' ' + M.MED_NOMBRE2 AS MEDICO,
	CP.PRO_CODIGO AS 'COD. PRODUCTO', CP.CUE_DETALLE AS PRODUCTO, CP.CUE_CANTIDAD AS CANTIDAD
	FROM PEDIDOS P 
	INNER JOIN PEDIDOS_DETALLE PD ON P.PED_CODIGO = PD.PED_CODIGO
	INNER JOIN CUENTAS_PACIENTES CP ON P.PED_CODIGO = CP.Codigo_Pedido
	LEFT JOIN PEDIDOS_ESTACIONES PE ON P.PEE_CODIGO = PE.PEE_CODIGO
	INNER JOIN USUARIOS U ON P.ID_USUARIO = U.ID_USUARIO
	INNER JOIN ATENCIONES A ON CP.ATE_CODIGO = A.ATE_CODIGO
	INNER JOIN PACIENTES PA ON A.PAC_CODIGO = PA.PAC_CODIGO
	INNER JOIN MEDICOS M ON P.MED_CODIGO = M.MED_CODIGO
	WHERE PE.PEE_CODIGO = @CodigoEstacion AND PD.PDD_ESTADO = @EstadoDetallePedido
	ORDER BY P.PED_FECHA DESC 
  end      
 end      
       
 if @EstadoDetallePedido<>2      
 begin        
  if @FiltroFecha=1      
  begin      
         
   --select       
   --PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   --pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   --pedidos.PED_FECHA AS [FECHA],      
   --PEE_DESCRIPCION AS [ESTACION] ,      
   --CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,        
   --MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   --PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   --DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   --PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   --isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   --from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS      
   --where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   --and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   --AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   --AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   --and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion      
   --and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido        
   --and CAST(CONVERT(varchar(11),ped_fecha,103) as DATE) between @FechaInicio and @FechaFin      
   --order by [NUMERO PEDIDO] desc   
   
	SELECT P.PED_CODIGO AS 'CODIGO', P.PED_FECHA AS 'F. PEDIDO', PE.PEE_NOMBRE AS ESTACION,
	PA.PAC_APELLIDO_PATERNO + ' ' + PA.PAC_APELLIDO_MATERNO + ' ' + PA.PAC_NOMBRE1 + ' ' + PA.PAC_NOMBRE2 AS PACIENTE,
	U.APELLIDOS + ' ' + U.NOMBRES AS 'PEDIDO POR',
	M.MED_APELLIDO_PATERNO + ' ' + M.MED_APELLIDO_MATERNO + ' ' + M.MED_NOMBRE1 + ' ' + M.MED_NOMBRE2 AS MEDICO,
	CP.PRO_CODIGO AS 'COD. PRODUCTO', CP.CUE_DETALLE AS PRODUCTO, CP.CUE_CANTIDAD AS CANTIDAD
	FROM PEDIDOS P 
	INNER JOIN PEDIDOS_DETALLE PD ON P.PED_CODIGO = PD.PED_CODIGO
	INNER JOIN CUENTAS_PACIENTES CP ON P.PED_CODIGO = CP.Codigo_Pedido
	LEFT JOIN PEDIDOS_ESTACIONES PE ON P.PEE_CODIGO = PE.PEE_CODIGO
	INNER JOIN USUARIOS U ON P.ID_USUARIO = U.ID_USUARIO
	INNER JOIN ATENCIONES A ON CP.ATE_CODIGO = A.ATE_CODIGO
	INNER JOIN PACIENTES PA ON A.PAC_CODIGO = PA.PAC_CODIGO
	INNER JOIN MEDICOS M ON P.MED_CODIGO = M.MED_CODIGO
	WHERE PE.PEE_CODIGO =@CodigoEstacion AND PD.PDD_ESTADO = @EstadoDetallePedido
	AND P.PED_FECHA BETWEEN @FechaInicio AND @FechaFin
	ORDER BY P.PED_FECHA DESC
          
  end      
  else      
  begin      
   --select       
   --PEDIDOS_DETALLE.PDD_ESTADO AS [ESTADO PEDIDO],      
   --pedidos.PED_CODIGO as [NUMERO PEDIDO],      
   --pedidos.PED_FECHA AS [FECHA],      
   --PEE_DESCRIPCION AS [ESTACION] ,      
   --CONCAT(USUARIOS.NOMBRES, ' ', USUARIOS.APELLIDOS) USUARIO,        
   --MEDICOS.MED_APELLIDO_PATERNO + ' ' + MEDICOS.MED_APELLIDO_MATERNO + ' ' + MEDICOS.MED_NOMBRE1 + ' ' + MEDICOS.MED_NOMBRE2 AS [MEDICO],      
   --PEDIDOS_DETALLE.PRO_CODIGO AS [CODIGO PRODUCTO],      
   --DBO.DatosProducto(PEDIDOS_DETALLE.PRO_CODIGO) AS [DESCRIPCION PRODUCTO],      
   --PEDIDOS_DETALLE.PDD_CANTIDAD as CANTIDAD,    
   --isnull(dbo.f_ValorDevuelto(PEDIDOS_DETALLE.PDD_CODIGO),0) as [CANTIDAD DEVUELTA]    
   --from pedidos,PEDIDOS_DETALLE ,PEDIDOS_ESTACIONES,USUARIOS,MEDICOS      
   --where pedidos.PED_CODIGO=PEDIDOS_DETALLE.PED_CODIGO      
   --and pedidos.PEE_CODIGO=PEDIDOS_ESTACIONES.PEE_CODIGO      
   --AND PEDIDOS.ID_USUARIO=USUARIOS.ID_USUARIO      
   --AND MEDICOS.MED_CODIGO=PEDIDOS.MED_CODIGO      
   --and PEDIDOS_ESTACIONES.PEE_CODIGO=@CodigoEstacion      
   --and PEDIDOS_DETALLE.PDD_ESTADO=@EstadoDetallePedido        
   --order by [NUMERO PEDIDO] desc  
	SELECT P.PED_CODIGO AS 'CODIGO', P.PED_FECHA AS 'F. PEDIDO', PE.PEE_NOMBRE AS ESTACION,
	PA.PAC_APELLIDO_PATERNO + ' ' + PA.PAC_APELLIDO_MATERNO + ' ' + PA.PAC_NOMBRE1 + ' ' + PA.PAC_NOMBRE2 AS PACIENTE,
	U.APELLIDOS + ' ' + U.NOMBRES AS 'PEDIDO POR',
	M.MED_APELLIDO_PATERNO + ' ' + M.MED_APELLIDO_MATERNO + ' ' + M.MED_NOMBRE1 + ' ' + M.MED_NOMBRE2 AS MEDICO,
	CP.PRO_CODIGO AS 'COD. PRODUCTO', CP.CUE_DETALLE AS PRODUCTO, CP.CUE_CANTIDAD AS CANTIDAD
	FROM PEDIDOS P 
	INNER JOIN PEDIDOS_DETALLE PD ON P.PED_CODIGO = PD.PED_CODIGO
	INNER JOIN CUENTAS_PACIENTES CP ON P.PED_CODIGO = CP.Codigo_Pedido
	LEFT JOIN PEDIDOS_ESTACIONES PE ON P.PEE_CODIGO = PE.PEE_CODIGO
	INNER JOIN USUARIOS U ON P.ID_USUARIO = U.ID_USUARIO
	INNER JOIN ATENCIONES A ON CP.ATE_CODIGO = A.ATE_CODIGO
	INNER JOIN PACIENTES PA ON A.PAC_CODIGO = PA.PAC_CODIGO
	INNER JOIN MEDICOS M ON P.MED_CODIGO = M.MED_CODIGO
	WHERE PE.PEE_CODIGO =@CodigoEstacion AND PD.PDD_ESTADO = @EstadoDetallePedido
	ORDER BY P.PED_FECHA DESC   
  end      
 end      
       
end