using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using His.Entidades;

namespace His.Datos
{
    public class DatHonorarioAutomatico
    {
        public List<CUENTAS_PACIENTES> listaxRubro(string rubro,DateTime desde,DateTime hasta)
        {
            using (var db = new HIS3000BDEntities(ConexionEntidades.ConexionEDM))
            {
                return db.CUENTAS_PACIENTES
                         .Where(x => x.PRO_CODIGO == rubro && x.CUE_FECHA >= desde && x.CUE_FECHA <= hasta)
                         .ToList();
            }
        }
        public HONORARIOS_MEDICOS recuperaHonorario(Int64 ate_codigo, Int64 med_codigo)
        {
            using (var db = new HIS3000BDEntities(ConexionEntidades.ConexionEDM))
            {
                return db.HONORARIOS_MEDICOS
                         .Where(x => x.ATE_CODIGO == ate_codigo && x.MEDICOS.MED_CODIGO == med_codigo)
                         .FirstOrDefault();
            }
        }
    }
}
