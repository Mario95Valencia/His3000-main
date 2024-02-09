using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using His.Entidades;
using His.Negocio;
using Infragistics.Win.UltraWinGrid;

namespace His.Maintenance
{
    public partial class frmHonorarioAutomaticos : Form
    {
        List<CUENTAS_PACIENTES> cp = new List<CUENTAS_PACIENTES>();
        ATENCIONES ultimaAtencion = new ATENCIONES();
        DtoPacientes paciente = new DtoPacientes();
        public frmHonorarioAutomaticos()
        {
            InitializeComponent();
            dtpFiltroDesde.Value = Convert.ToDateTime(String.Format("{0:g}", "01/" + DateTime.Now.Month + "/" + (DateTime.Now.Year).ToString()));
            dtpFiltroHasta.Value = DateTime.Now;
            cp = NegHonorarioAutomatico.listaxRubro("109078", dtpFiltroDesde.Value.Date, dtpFiltroHasta.Value.Date);
            UltraGridHCEX.DataSource = cp;
        }

        public bool creacionHonorarioLiquidacion(CUENTAS_PACIENTES cp, FORMA_PAGO fp, int referido)
        {
            PARAMETROS_DETALLE obj = new PARAMETROS_DETALLE();
            obj = NegLiquidacion.parametrosHonorarios(18);
            if (Convert.ToBoolean(obj.PAD_ACTIVO))
            {

                bool valido = true;
                if (fp.FOR_CODIGO == 1) //es efectivo en el his
                {
                    //creacion del honorario
                    HONORARIOS_MEDICOS nuevoHonorario;
                    MEDICOS m = NegMedicos.recuperarMedico(Convert.ToInt32(cp.MED_CODIGO));
                    USUARIOS u = NegUsuarios.RecuperarUsuarioID(Convert.ToInt16(cp.ID_USUARIO));
                    nuevoHonorario = new HONORARIOS_MEDICOS();
                    nuevoHonorario.HOM_CODIGO = Convert.ToInt32(NegHonorariosMedicos.ultimoCodigoHonorarios() + 1);
                    nuevoHonorario.MEDICOSReference.EntityKey = m.EntityKey;
                    nuevoHonorario.ATE_CODIGO = ultimaAtencion.ATE_CODIGO;
                    nuevoHonorario.FOR_CODIGO = Convert.ToInt16(fp.FOR_CODIGO);
                    nuevoHonorario.TMO_CODIGO = 201; //default
                    nuevoHonorario.USUARIOSReference.EntityKey = u.EntityKey;
                    nuevoHonorario.HOM_FECHA_INGRESO = (DateTime)cp.CUE_FECHA;
                    nuevoHonorario.NUM_PAGO = null;
                    nuevoHonorario.HOM_FACTURA_MEDICO = ""; //default
                    nuevoHonorario.HOM_FACTURA_FECHA = DateTime.Now;
                    nuevoHonorario.HOM_VALOR_NETO = (decimal)cp.CUE_VALOR_UNITARIO;
                    nuevoHonorario.HOM_COMISION_CLINICA = 0;
                    if (referido == 1) //hospitalario
                        nuevoHonorario.HOM_APORTE_LLAMADA = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(fp.FOR_REFERIDO)) / 100), 2);
                    else
                    {

                        if (m.MED_CODIGO_LIBRO == "1")
                        {
                            //?
                        }
                        if (Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 102 ||
                            Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 130 ||
                            Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 146 ||
                            Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 154)
                        {
                            nuevoHonorario.HOM_APORTE_LLAMADA = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(fp.FOR_REFERIDO)) / 100), 2);
                        }
                        else
                            nuevoHonorario.HOM_APORTE_LLAMADA = 0;

                    }
                    RETENCIONES_FUENTE rf = NegRetencionesFuente.recuperarPorId(Convert.ToInt32(m.RETENCIONES_FUENTEReference.EntityKey.EntityKeyValues[0].Value));
                    nuevoHonorario.HOM_RETENCION = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(rf.RET_PORCENTAJE)) / 100), 2);
                    nuevoHonorario.HOM_ESTADO = His.Parametros.HonorariosPAR.HonorarioCreado;
                    nuevoHonorario.HOM_VALOR_PAGADO = 0;
                    nuevoHonorario.HOM_VALOR_CANCELADO = 0;
                    nuevoHonorario.HOM_VALOR_TOTAL = nuevoHonorario.HOM_VALOR_NETO - nuevoHonorario.HOM_COMISION_CLINICA - nuevoHonorario.HOM_APORTE_LLAMADA - nuevoHonorario.HOM_RETENCION;
                    nuevoHonorario.HOM_POR_PAGAR = nuevoHonorario.HOM_VALOR_TOTAL;
                    nuevoHonorario.HOM_RECORTE = 0;
                    nuevoHonorario.HOM_NETO_PAGAR = nuevoHonorario.HOM_VALOR_TOTAL;
                    nuevoHonorario.HOM_OBSERVACION = "CONSULTA EXTERNA";
                    nuevoHonorario.HOM_PACIENTE = paciente.PAC_APELLIDO_PATERNO + " " + paciente.PAC_APELLIDO_MATERNO + " " + paciente.PAC_NOMBRE1 + " " + paciente.PAC_NOMBRE2;
                    nuevoHonorario.HOM_VALE = cp.NumVale;
                    nuevoHonorario.HOM_LOTE = "";

                    NegHonorariosMedicos.CrearHonorarioMedico(nuevoHonorario);
                    string FecCaducidad = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                    NegHonorariosMedicos.saveHMDatosAdicionales(nuevoHonorario.HOM_CODIGO, FecCaducidad, 0, "0", "107", 0, 0);
                    NegHonorariosMedicos.CrearHonorarioAuditoria(nuevoHonorario, false, false, "NUEVO", "107", 0, Convert.ToInt64(cp.MED_CODIGO));
                }
                else
                {
                    //creacion del honorario
                    HONORARIOS_MEDICOS nuevoHonorario;
                    MEDICOS m = NegMedicos.recuperarMedico(Convert.ToInt32(cp.MED_CODIGO));
                    USUARIOS u = NegUsuarios.RecuperarUsuarioID(Convert.ToInt16(cp.ID_USUARIO));
                    nuevoHonorario = new HONORARIOS_MEDICOS();
                    nuevoHonorario.HOM_CODIGO = Convert.ToInt32(NegHonorariosMedicos.ultimoCodigoHonorarios() + 1);
                    nuevoHonorario.MEDICOSReference.EntityKey = m.EntityKey;
                    nuevoHonorario.ATE_CODIGO = ultimaAtencion.ATE_CODIGO;
                    nuevoHonorario.FOR_CODIGO = Convert.ToInt16(fp.FOR_CODIGO);
                    nuevoHonorario.TMO_CODIGO = 201; //default
                    nuevoHonorario.USUARIOSReference.EntityKey = u.EntityKey;
                    nuevoHonorario.HOM_FECHA_INGRESO = (DateTime)cp.CUE_FECHA;
                    nuevoHonorario.NUM_PAGO = null;
                    nuevoHonorario.HOM_FACTURA_MEDICO = ""; //default
                    nuevoHonorario.HOM_FACTURA_FECHA = DateTime.Now;
                    nuevoHonorario.HOM_VALOR_NETO = (decimal)cp.CUE_VALOR_UNITARIO;
                    nuevoHonorario.HOM_COMISION_CLINICA = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(fp.FOR_COMISION)) / 100), 2);
                    if (referido == 1) //hospitalario
                        nuevoHonorario.HOM_APORTE_LLAMADA = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(fp.FOR_REFERIDO)) / 100), 2);
                    else
                    {

                        if (m.MED_CODIGO_LIBRO == "1")
                        {
                            //?
                        }
                        if (Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 102 ||
                            Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 130 ||
                            Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 146 ||
                            Convert.ToInt32(m.ESPECIALIDADES_MEDICASReference.EntityKey.EntityKeyValues[0].Value) == 154)
                        {
                            nuevoHonorario.HOM_APORTE_LLAMADA = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(fp.FOR_REFERIDO)) / 100), 2);
                        }
                        else
                            nuevoHonorario.HOM_APORTE_LLAMADA = 0;

                    }
                    RETENCIONES_FUENTE rf = NegRetencionesFuente.recuperarPorId(Convert.ToInt32(m.RETENCIONES_FUENTEReference.EntityKey.EntityKeyValues[0].Value));
                    nuevoHonorario.HOM_RETENCION = (decimal)Math.Round((Convert.ToDouble(cp.CUE_VALOR_UNITARIO) * (Convert.ToDouble(rf.RET_PORCENTAJE)) / 100), 2);
                    nuevoHonorario.HOM_ESTADO = His.Parametros.HonorariosPAR.HonorarioCreado;
                    nuevoHonorario.HOM_VALOR_PAGADO = 0;
                    nuevoHonorario.HOM_VALOR_CANCELADO = 0;
                    nuevoHonorario.HOM_VALOR_TOTAL = nuevoHonorario.HOM_VALOR_NETO - nuevoHonorario.HOM_COMISION_CLINICA - nuevoHonorario.HOM_APORTE_LLAMADA - nuevoHonorario.HOM_RETENCION;
                    nuevoHonorario.HOM_POR_PAGAR = nuevoHonorario.HOM_VALOR_TOTAL;
                    nuevoHonorario.HOM_RECORTE = 0;
                    nuevoHonorario.HOM_NETO_PAGAR = nuevoHonorario.HOM_VALOR_TOTAL;
                    nuevoHonorario.HOM_OBSERVACION = "CONSULTA EXTERNA";
                    nuevoHonorario.HOM_PACIENTE = paciente.PAC_APELLIDO_PATERNO + " " + paciente.PAC_APELLIDO_MATERNO + " " + paciente.PAC_NOMBRE1 + " " + paciente.PAC_NOMBRE2;
                    nuevoHonorario.HOM_VALE = cp.NumVale;
                    nuevoHonorario.HOM_LOTE = "";

                    NegHonorariosMedicos.CrearHonorarioMedico(nuevoHonorario);
                    string FecCaducidad = Convert.ToString(cp.CUE_FECHA);
                    NegHonorariosMedicos.saveHMDatosAdicionales(nuevoHonorario.HOM_CODIGO, FecCaducidad, 0, "0", "107", 0, 0);
                    NegHonorariosMedicos.CrearHonorarioAuditoria(nuevoHonorario, false, false, "NUEVO", "107", 0, Convert.ToInt64(cp.MED_CODIGO));
                }

                return valido;
            }
            else
            {
                return false;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            foreach (var item in cp)
            {
                Int64 ate_codigo = (long)item.ATE_CODIGO;
                Int64 med_codigo = (long)item.MED_CODIGO;
                HONORARIOS_MEDICOS hmed = NegHonorarioAutomatico.recuperaHonorario(ate_codigo, med_codigo);
                if (hmed == null)
                {
                    Int32 ate = (int)item.ATE_CODIGO;
                    ultimaAtencion = NegAtenciones.RecuperarAtencionID(ate);
                    if (ultimaAtencion.ATE_FACTURA_PACIENTE != null)
                    {

                        paciente = NegPacientes.RecuperarDtoPacienteID(Convert.ToInt32(ultimaAtencion.PACIENTESReference.EntityKey.EntityKeyValues[0].Value));
                        DataTable FormaPagoxDentro = NegHonorariosMedicos.HMDentroPago(ultimaAtencion.ATE_FACTURA_PACIENTE);
                        TIPO_REFERIDO tr = NegTipoReferido.RecuperarReferido(Convert.ToInt32(ultimaAtencion.TIPO_REFERIDOReference.EntityKey.EntityKeyValues[0].Value));
                        if (FormaPagoxDentro.Rows.Count > 0) //Valida que tenga factura
                        {
                            int index1 = Convert.ToInt32(FormaPagoxDentro.Rows[0]["forpag"].ToString()); //Codigo de la FORMA DE PAGO
                            int index2 = Convert.ToInt32(FormaPagoxDentro.Rows[0]["claspag"].ToString()); //Codigo de la clasificacion
                                                                                                          //el seguro no esta contemplado.

                            FORMA_PAGO fp = NegFormaPago.recuperarFormaPago_forpag(index1.ToString());
                            if (index2 == 2) //es efectivo
                            {
                                if (!creacionHonorarioLiquidacion(item, fp, tr.TIR_CODIGO))
                                {
                                    MessageBox.Show("Honorario de consulta externa no termino el proceso, intentelo de manera manual.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else if (index2 == 5) //otros documentos
                            {
                                if (index1 == 15) //Anticipos
                                {
                                    DataTable AnticipoFormaP = NegFormaPago.FormaPagoAnticipo(FormaPagoxDentro.Rows[0]["cheque_caduca"].ToString());
                                    if (AnticipoFormaP.Rows.Count > 0)
                                    {
                                        if (AnticipoFormaP.Rows[0][1].ToString().Contains("EFECTIVO"))
                                        {
                                            fp = NegFormaPago.recuperarFormaPago_forpag("6");
                                            if (!creacionHonorarioLiquidacion(item, fp, tr.TIR_CODIGO))
                                            {
                                                MessageBox.Show("Honorario de consulta externa no termino el proceso, intentelo de manera manual.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            }
                                        }
                                        else
                                        {
                                            if (!creacionHonorarioLiquidacion(item, fp, tr.TIR_CODIGO))
                                            {
                                                MessageBox.Show("Honorario de consulta externa no termino el proceso, intentelo de manera manual.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (index2 == 4)
                            {
                                if (!creacionHonorarioLiquidacion(item, fp, tr.TIR_CODIGO))
                                {
                                    MessageBox.Show("Honorario de consulta externa no termino el proceso, intentelo de manera manual.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }

                        }
                    }
                }

            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            cp = NegHonorarioAutomatico.listaxRubro("109078", dtpFiltroDesde.Value.Date, dtpFiltroHasta.Value.Date);
            UltraGridHCEX.DataSource = cp;
        }
    }
}
