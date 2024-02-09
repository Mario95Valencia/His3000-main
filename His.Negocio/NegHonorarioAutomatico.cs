using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using His.Datos;
using His.Entidades;

namespace His.Negocio
{
    public class NegHonorarioAutomatico
    {
        public static List<CUENTAS_PACIENTES> listaxRubro(string rubro, DateTime desde, DateTime hasta)
        {
            return new DatHonorarioAutomatico().listaxRubro(rubro, desde, hasta);
        }
        public static HONORARIOS_MEDICOS recuperaHonorario(Int64 ate_codigo, Int64 med_codigo)
        {
            return new DatHonorarioAutomatico().recuperaHonorario(ate_codigo, med_codigo);
        }
    }
}
