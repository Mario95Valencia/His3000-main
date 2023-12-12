
--AGREGO CAMPO 
alter table HC_EVOLUCION_ENFERMERIA_DETALLE add EVD_FECHA_INSERT datetime default getdate() 
--CREO LLAVE PRIMARIA
alter table [HC_EVOLUCION_ENFERMERA] add primary key (EVO_CODIGO);
--LIMPIO TABLA PARA CAMBIAR TIPO DE DATO
delete from HC_EVOLUCION_ENFERMERIA_DETALLE
--CAMBIO TIPO DE DATO
ALTER TABLE HC_EVOLUCION_ENFERMERIA_DETALLE
ALTER COLUMN EVD_FECHA DATETIME;

--CREO CAMPO PARA GUARDAR LOGS DE MODIFICACIONES
ALTER TABLE HC_EVOLUCION_ENFERMERIA_DETALLE ADD ESTADO INT DEFAULT 1