using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using His.Negocio;
using His.Entidades.Clases;
using His.Entidades;
using His.Formulario;

namespace His.ConsultaExterna
{
    public partial class Consulta : Form
    {
        bool band = true;
        public bool nuevaConsulta = false;
        DtoForm002 datos = new DtoForm002();
        public Int64 id_form002 = 0;
        ATENCIONES_SUBSECUENTES subsecuente = new ATENCIONES_SUBSECUENTES();
        bool emergecia = false;
        public Consulta()
        {
            InitializeComponent();
            inicializarGridPrescripciones();
            txt_horaAltaEmerencia.Text = DateTime.Now.ToString("hh:mm");
            txt_profesionalEmergencia.Text = Sesion.nomUsuario;
        }
        public Consulta(string ateCodigo)
        {
            InitializeComponent();
            inicializarGridPrescripciones();
            txt_horaAltaEmerencia.Text = DateTime.Now.ToString("hh:mm");
            txt_profesionalEmergencia.Text = Sesion.nomUsuario;
            if (ateCodigo != "")
            {
                txtatecodigo.Text = ateCodigo;
                CargaConsulta();
            }
        }
        public void CargaConsulta()
        {
            DataTable paciente = new DataTable();
            paciente = NegConsultaExterna.RecuperaPaciente(Convert.ToInt64(txtatecodigo.Text));
            DataTable signos = new DataTable();
            signos = NegConsultaExterna.RecuperaSignos(Convert.ToInt64(txtatecodigo.Text));

            consultaExterna = NegConsultaExterna.PacienteExisteCxE(txtatecodigo.Text.Trim());
            if (consultaExterna != null)
                CargarPacienteExiste();
            else
            {
                HabilitarBotones(true, false, false, false, false, false, false, false, false, false, false);
                nuevaConsulta = true;
                P_Central.Enabled = true;
            }


            if (signos.Rows.Count > 0)
            {
                string enteros = signos.Rows[0][3].ToString();
                string[] final = enteros.Split('.');
                txtPresionArteria1.Text = final[0];
                enteros = signos.Rows[0][4].ToString();
                final = enteros.Split('.');
                txtPresionArteria2.Text = final[0];
                //enteros = signos.Rows[0][5].ToString();
                //final = enteros.Split('.');
                txtPulso.Text = signos.Rows[0][5].ToString();
                //enteros = signos.Rows[0][6].ToString();
                //final = enteros.Split('.');
                txtFrecuenciaRespiratoria.Text = signos.Rows[0][6].ToString();
                //enteros = signos.Rows[0][7].ToString();
                //final = enteros.Split('.');
                txtTemperatura.Text = signos.Rows[0][7].ToString();
                final = enteros.Split('.');
                txtTemperatura.Text = signos.Rows[0][8].ToString();
                //enteros = signos.Rows[0][10].ToString();
                //final = enteros.Split('.');
                txtPeso.Text = signos.Rows[0][10].ToString();
                enteros = signos.Rows[0][11].ToString();
                final = enteros.Split('.');
                txtTalla.Text = signos.Rows[0][11].ToString();
            }
            lblHistoria.Text = txthistoria.Text;
            lblAteCodigo.Text = txtatecodigo.Text;
            lblNombre.Text = paciente.Rows[0][0].ToString();
            lblApellido.Text = paciente.Rows[0][1].ToString();
            lblEdad.Text = paciente.Rows[0][2].ToString();

            if (paciente.Rows[0][3].ToString().Trim() == "M")
            {
                lblSexo.Text = "Masculino";
            }
            else
            {
                lblSexo.Text = "Femenino";
            }
            //CargarPaciente(); //aqui se puede ver las n consultas externas del paciente
            P_Central.Visible = true;
            P_Datos.Visible = true;
            //btnBuscarPaciente.Visible = false;
            //btnGuarda.Visible = true;
            //btnNuevo.Visible = true;
        }
        #region FUNCIONES Y OBJETOS


        private void inicializarGridPrescripciones()
        {
            gridPrescripciones.EditMode = DataGridViewEditMode.EditOnKeystroke;
            PRES_FARMACOTERAPIA_INDICACIONES.SortMode = DataGridViewColumnSortMode.NotSortable;
            PRES_FARMACOS_INSUMOS.SortMode = DataGridViewColumnSortMode.NotSortable;
            PRES_FECHA.SortMode = DataGridViewColumnSortMode.NotSortable;
            PRES_ESTADO.SortMode = DataGridViewColumnSortMode.NotSortable;
            PRES_CODIGO.Visible = false;
            ID_USUARIO.Visible = false;
            NOM_USUARIO.Visible = false;
            PRES_ESTADO.Width = 20;
            PRES_FECHA.Width = 130;
            PRES_FARMACOTERAPIA_INDICACIONES.Width = 350;
            PRES_FARMACOS_INSUMOS.Width = 200;
        }


        Boolean permitir = true;
        public bool txtKeyPress(TextBox textbox, int code)
        {

            bool resultado;

            if (code == 46 && textbox.Text.Contains("."))//se evalua si es punto y si es punto se revisa si ya existe en el textbox
            {
                resultado = true;
            }
            else if ((((code >= 48) && (code <= 57)) || (code == 8) || code == 46)) //se evaluan las teclas validas
            {
                resultado = false;
            }
            else if (!permitir)
            {
                resultado = permitir;
            }
            else
            {
                resultado = true;
            }

            return resultado;

        }
        public void ValidaError(Control control, string campo)
        {
            error.SetError(control, campo);
        }

        public bool Valida()
        {
            if (txt_profesionalEmergencia.Visible == true)
            {
                if (txt_profesionalEmergencia.Text == "")
                {
                    ValidaError(txt_profesionalEmergencia, "INGRESE UN PROFECIONAL");
                    return false;
                }
            }
            if (txtDiagnostico1.Text == "")
            {
                ValidaError(txtDiagnostico1, "INGRESE CIE-10 PARA GUARDAR");
                return false;
            }
            if (txtMotivo.Text == "")
            {
                ValidaError(txtMotivo, "INGRESE MOTIVO DE CONSULTA");
                return false;
            }
            error.Clear();
            if (txtAntecedentesPersonales.Text == "")
            {
                ValidaError(txtAntecedentesPersonales, "INGRESE ANTECEDENTES PERSONALES");
                return false;
            }
            error.Clear();
            if (!chbCardiopatia.Checked && !chbDiabetes.Checked && !chbVascular.Checked && !chbHiperT.Checked && !chbCancer.Checked && !chbTuberculosis.Checked && !chbMental.Checked && !chbInfeccionsa.Checked && !chbMalFormado.Checked && !chbOtro.Checked)
            {
                ValidaError(label3, "DEBE SELECCIONAR POR LO MENOS UNO");
                return false;
            }
            if (chbOtro.Checked)
            {
                if (txtAntecedentesFamiliares.Text == "")
                {
                    ValidaError(txtAntecedentesFamiliares, "INGRESE ANTECEDENTES FAMILIARES");
                    return false;
                }
            }
            error.Clear();
            if (txtEnfermedadProblema.Text == "")
            {
                ValidaError(txtEnfermedadProblema, "INGRESE ENFERMEDAD O PROBLEMA ACTUAL");
                return false;
            }
            error.Clear();
            if (txtRevisionActual.Text == "")
            {
                ValidaError(txtRevisionActual, "INGRESE REVISIÓN ACTUAL DE ÓRGANOS Y SISTEMAS");
                return false;
            }
            error.Clear();
            if (txtTemperatura.Text == "")
            {
                ValidaError(txtTemperatura, "INGRESE TEMPERATURA");
                return false;
            }
            error.Clear();
            if (txtPresionArteria1.Text == "")
            {
                ValidaError(txtPresionArteria1, "INGRESE PRESIÓN ARTERIAL");
                return false;
            }
            error.Clear();
            if (txtPresionArteria2.Text == "")
            {
                ValidaError(txtPresionArteria2, "INGRESE PRESIÓN ARTERIAL");
                return false;
            }
            error.Clear();
            if (txtPeso.Text == "")
            {
                ValidaError(txtPeso, "INGRESE PESO");
                return false;
            }
            error.Clear();
            if (txtFrecuenciaRespiratoria.Text == "")
            {
                ValidaError(txtFrecuenciaRespiratoria, "INGRESE FRECUENCIA RESPIRATORIA");
                return false;
            }
            error.Clear();
            if (txtPulso.Text == "")
            {
                ValidaError(txtPulso, "INGRESE PULSO");
                return false;
            }
            error.Clear();
            if (txtTalla.Text == "")
            {
                ValidaError(txtTalla, "INGRESE TALLA");
                return false;
            }
            error.Clear();
            if (txtExamenFisico.Text == "")
            {
                ValidaError(txtExamenFisico, "INGRESE EXAMEN FÍSICO REGIONAL");
                return false;
            }
            error.Clear();
            if (txtPlanesTratamiento.Text == "")
            {
                ValidaError(txtPlanesTratamiento, "INGRESE PLANES DE TRATAMIENTO");
                return false;
            }
            error.Clear();
            if (txtEvolucion.Text == "")
            {
                ValidaError(txtEvolucion, "INGRESE EVOLUCIÓN");
                return false;
            }
            error.Clear();
            if (txtDiagnostico1.Text != "" && txtCieDiagnostico1.Text == "")
            {
                ValidaError(txtCieDiagnostico1, "INGRESE DIAGNOSTICO CIE10");
                return false;
            }
            error.Clear();
            if (txtDiagnostico2.Text != "" && txtCieDiagnostico2.Text == "")
            {
                ValidaError(txtCieDiagnostico2, "INGRESE DIAGNOSTICO CIE10");
                return false;
            }
            error.Clear();
            if (txtDiagnostico3.Text != "" && txtCieDiagnostico3.Text == "")
            {
                ValidaError(txtCieDiagnostico3, "INGRESE DIAGNOSTICO CIE10");
                return false;
            }
            error.Clear();
            if (txtDiagnostico4.Text != "" && txtCieDiagnostico4.Text == "")
            {
                ValidaError(txtCieDiagnostico4, "INGRESE DIAGNOSTICO CIE10");
                return false;
            }
            error.Clear();
            return true;
        }

        //public void limpiaCampos()
        //{

        //    //lblNombre.Text = "";
        //    //lblApellido.Text = "";
        //    //lblSexo.Text = "";

        //    //lblEdad.Text = "";
        //    //lblHistoria.Text = "";
        //    //lblAteCodigo.Text = "";


        //    txtMotivo.Text = "";
        //    txtAntecedentesPersonales.Text = "";

        //    txtAntecedentesFamiliares.Text = "";
        //    txtEnfermedadProblema.Text = "";

        //    txtRevisionActual.Text = "";
        //    dtpMedicion.Text = "";
        //    txtTemperatura.Text = "";
        //    txtPresionArteria1.Text = "";
        //    txtPresionArteria2.Text = "";
        //    txtPulso.Text = "";
        //    txtFrecuenciaRespiratoria.Text = "";
        //    txtPeso.Text = "";
        //    txtTalla.Text = "";

        //    txtExamenFisico.Text = "";
        //    txtDiagnostico1.Text = "";
        //    txtCieDiagnostico1.Text = "";

        //    txtCieDiagnostico2.Text = "";
        //    txtCieDiagnostico2.Text = "";
        //    if (chb5cp1.Checked)
        //        chb5cp1.Checked = false;
        //    if (chb5cp10.Checked)
        //        chb5cp10.Checked = false;
        //    if (chb5cp2.Checked)
        //        chb5cp2.Checked = false;
        //    if (chb5cp3.Checked)
        //        chb5cp3.Enabled = false;
        //    if (chb5cp4.Checked)
        //        chb5cp4.Checked = false;
        //    if (chb5cp5.Checked)
        //        chb5cp5.Checked = false;
        //    if (chb5cp6.Checked)
        //        chb5cp6.Checked = false;
        //    if (chb5cp7.Checked)
        //        chb5cp7.Checked = false;
        //    if (chb5cp8.Checked)
        //        chb5cp8.Checked = false;
        //    if (chb5cp9.Checked)
        //        chb5cp9.Checked = false;
        //    if (chbCardiopatia.Checked)
        //        chbCardiopatia.Checked = false;
        //    if (chbDiabetes.Checked)
        //        chbDiabetes.Checked = false;
        //    if (chbVascular.Checked)
        //        chbVascular.Checked = false;
        //    if (chbHiperT.Checked)
        //        chbHiperT.Checked = false;
        //    if (chbCancer.Checked)
        //        chbCancer.Checked = false;
        //    if (chbTuberculosis.Checked)
        //        chbTuberculosis.Checked = false;
        //    if (chbMental.Checked)
        //        chbMental.Checked = false;
        //    if (chbInfeccionsa.Checked)
        //        chbInfeccionsa.Checked = false;
        //    if (chbMalFormado.Checked)
        //        chbMalFormado.Checked = false;
        //    if (chbOtro.Checked)
        //        chbOtro.Checked = false;
        //    txtDiagnostico3.Text = "";
        //    txtCieDiagnostico3.Text = "";

        //    txtDiagnostico4.Text = "";
        //    txtCieDiagnostico4.Text = "";

        //    txtPlanesTratamiento.Text = "";
        //    //txt_profesionalEmergencia.Text = "";
        //    txt_CodMSPE.Text = "";
        //    txtEvolucion.Text = "";
        //    txtindicaciones.Text = "";

        //}


        public bool GuardaFormulario()
        {

            datos.historiaClinica = lblHistoria.Text;
            datos.ateCodigo = lblAteCodigo.Text;
            datos.nombrePaciente = lblNombre.Text;
            datos.apellidoPaciemte = lblApellido.Text;
            datos.edadPaciente = lblEdad.Text;

            datos.sexoPaciente = lblSexo.Text;
            datos.motivoConsulta = txtMotivo.Text;
            datos.antecedentesPersonales = txtAntecedentesPersonales.Text;
            if (chbCardiopatia.Checked)
                datos.cardiopatia = "X";
            else
                datos.cardiopatia = "O";
            if (chbDiabetes.Checked)
                datos.diabetes = "X";
            else
                datos.diabetes = "O";
            if (chbVascular.Checked)
                datos.vascular = "X";
            else
                datos.vascular = "O";
            if (chbHiperT.Checked)
                datos.hipertension = "X";
            else
                datos.hipertension = "O";
            if (chbCancer.Checked)
                datos.cancer = "X";
            else
                datos.cancer = "O";
            if (chbTuberculosis.Checked)
                datos.tuberculosis = "X";
            else
                datos.tuberculosis = "O";
            if (chbMental.Checked)
                datos.mental = "X";
            else
                datos.mental = "O";
            if (chbInfeccionsa.Checked)
                datos.infeccionsa = "X";
            else
                datos.infeccionsa = "O";
            if (chbMalFormado.Checked)
                datos.malFormado = "X";
            else
                datos.malFormado = "O";
            if (chbOtro.Checked)
                datos.otro = "X";
            else
                datos.otro = "O";
            datos.antecedentesFamiliares = txtAntecedentesFamiliares.Text;
            datos.enfermedadProblemaActual = txtEnfermedadProblema.Text;
            if (chb5cp1.Checked)
            {
                datos.sentidos = "X";
                datos.sentidossp = "O";
            }
            else
            {
                datos.sentidos = "O";
                datos.sentidossp = "X";
            }
            if (chb5cp2.Checked)
            {
                datos.respiratorio = "X";
                datos.respiratoriosp = "O";
            }
            else
            {
                datos.respiratorio = "O";
                datos.respiratoriosp = "X";
            }
            if (chb5cp3.Checked)
            {
                datos.cardioVascular = "X";
                datos.cardioVascularsp = "O";
            }
            else
            {
                datos.cardioVascular = "O";
                datos.cardioVascularsp = "X";
            }
            if (chb5cp4.Checked)
            {
                datos.digestivo = "X";
                datos.digestivosp = "O";
            }
            else
            {
                datos.digestivo = "O";
                datos.digestivosp = "X";
            }
            if (chb5cp5.Checked)
            {
                datos.genital = "X";
                datos.genitalsp = "O";
            }
            else
            {
                datos.genital = "O";
                datos.genitalsp = "X";
            }
            if (chb5cp6.Checked)
            {
                datos.urinario = "X";
                datos.urinariosp = "O";
            }
            else
            {
                datos.urinario = "O";
                datos.urinariosp = "X";
            }
            if (chb5cp7.Checked)
            {
                datos.esqueletico = "X";
                datos.esqueleticosp = "O";
            }
            else
            {
                datos.esqueletico = "O";
                datos.esqueleticosp = "X";
            }
            if (chb5cp8.Checked)
            {
                datos.endocrino = "X";
                datos.endocrinosp = "O";
            }
            else
            {
                datos.endocrino = "O";
                datos.endocrinosp = "X";
            }
            if (chb5cp9.Checked)
            {
                datos.linfatico = "X";
                datos.linfaticosp = "O";
            }
            else
            {
                datos.linfatico = "O";
                datos.linfaticosp = "X";
            }
            if (chb5cp10.Checked)
            {
                datos.nervioso = "X";
                datos.nerviososp = "O";
            }
            else
            {
                datos.nervioso = "O";
                datos.nerviososp = "X";
            }
            datos.detalleRevisionOrganos = txtRevisionActual.Text;
            datos.fechaMedicion = Convert.ToString(dtpMedicion.Value);
            datos.temperatura = txtTemperatura.Text;
            datos.presionArterial1 = txtPresionArteria1.Text;
            datos.presionArterial2 = txtPresionArteria2.Text;
            datos.pulso = txtPulso.Text;
            datos.frecuenciaRespiratoria = txtFrecuenciaRespiratoria.Text;
            datos.peso = txtPeso.Text;
            datos.talla = txtTalla.Text;
            if (chb7cp1.Checked)
            {
                datos.cabeza = "X";
                datos.cabezasp = "O";
            }
            else
            {
                datos.cabezasp = "X";
                datos.cabeza = "O";
            }
            if (chb7cp2.Checked)
            {
                datos.cuello = "X";
                datos.cuellosp = "O";
            }
            else
            {
                datos.cuellosp = "X";
                datos.cuello = "O";
            }
            if (chb7cp3.Checked)
            {
                datos.torax = "X";
                datos.toraxsp = "O";
            }
            else
            {
                datos.toraxsp = "X";
                datos.torax = "O";
            }
            if (chb7cp4.Checked)
            {
                datos.abdomen = "X";
                datos.abdomensp = "O";
            }
            else
            {
                datos.abdomensp = "X";
                datos.abdomen = "O";
            }
            if (chb7cp5.Checked)
            {
                datos.pelvis = "X";
                datos.pelvissp = "O";
            }
            else
            {
                datos.pelvissp = "X";
                datos.pelvis = "O";
            }
            if (chb7cp6.Checked)
            {
                datos.extremidades = "X";
                datos.extremidadessp = "O";
            }
            else
            {
                datos.extremidadessp = "X";
                datos.extremidades = "O";
            }
            datos.examenFisico = txtExamenFisico.Text;
            datos.diagnosticco1 = txtDiagnostico1.Text;
            datos.diagnosticco1cie = txtCieDiagnostico1.Text;
            if (txtCieDiagnostico1.Text == "")
            {
                datos.diagnosticco1prepre = "O";
                datos.diagnosticco1predef = "O";
            }
            else
            {
                if (chbPre1.Checked)
                {
                    datos.diagnosticco1prepre = "X";
                    datos.diagnosticco1predef = "O";
                }
                else
                {
                    datos.diagnosticco1prepre = "O";
                    datos.diagnosticco1predef = "X";
                }
            }
            datos.diagnosticco2 = txtDiagnostico2.Text;
            datos.diagnosticco2cie = txtCieDiagnostico2.Text;
            if (txtCieDiagnostico2.Text == "")
            {
                datos.diagnosticco2prepre = "O";
                datos.diagnosticco2predef = "O";
            }
            else
            {
                if (chbPre2.Checked)
                {
                    datos.diagnosticco2prepre = "X";
                    datos.diagnosticco2predef = "O";
                }
                else
                {
                    datos.diagnosticco2prepre = "O";
                    datos.diagnosticco2predef = "X";
                }
            }
            datos.diagnosticco3 = txtDiagnostico3.Text;
            datos.diagnosticco3cie = txtCieDiagnostico3.Text;

            if (txtCieDiagnostico3.Text == "")
            {
                datos.diagnosticco3prepre = "O";
                datos.diagnosticco3predef = "O";
            }
            else
            {
                if (chbPre3.Checked)
                {
                    datos.diagnosticco3prepre = "X";
                    datos.diagnosticco3predef = "O";
                }
                else
                {
                    datos.diagnosticco3prepre = "O";
                    datos.diagnosticco3predef = "X";
                }
            }
            datos.diagnosticco4 = txtDiagnostico4.Text;
            datos.diagnosticco4cie = txtCieDiagnostico4.Text;
            if (txtCieDiagnostico4.Text == "")
            {
                datos.diagnosticco4prepre = "O";
                datos.diagnosticco4predef = "O";
            }
            else
            {
                if (chbPre4.Checked)
                {
                    datos.diagnosticco4prepre = "X";
                    datos.diagnosticco4predef = "O";
                }
                else
                {
                    datos.diagnosticco4prepre = "O";
                    datos.diagnosticco4predef = "X";
                }
            }
            datos.planesTratamiento = txtPlanesTratamiento.Text;
            datos.evolucion = txtEvolucion.Text;
            datos.prescripciones = txtindicaciones.Text.Trim();
            datos.drTratatnte = Sesion.nomUsuario;
            if (button1.Visible == true)
            {
                datos.drTratatnte = txt_profesionalEmergencia.Text;
            }

            if (nuevaConsulta)
            {
                NegConsultaExterna.GuardaDatos002(datos);
                id_form002 = NegConsultaExterna.RecuperarId();
                return true;
            }
            else
            {
                if (NegConsultaExterna.EditarForm002(datos, id_form002))
                    return true;
                else
                    return false;
            }

        }



        #endregion

        private void txtMotivo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                txtAntecedentesPersonales.Focus();
            }
        }

        private void txtAntecedentesPersonales_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                txtAntecedentesFamiliares.Focus();
            }
        }

        private void txtAntecedentesFamiliares_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                pantab1.SelectedTab = pantab1.Tabs["CuatroCinco"];
                SendKeys.SendWait("{TAB}");
                txtEnfermedadProblema.Focus();
            }
        }

        private void txtEnfermedadProblema_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                txtRevisionActual.Focus();
            }
        }

        private void txtRevisionActual_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                pantab1.SelectedTab = pantab1.Tabs["SeisSiete"];
                SendKeys.SendWait("{TAB}");
                txtTemperatura.Focus();
            }
        }

        private void txtTemperatura_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender; // Convierto el sender a TextBox
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtPresionArteria1.Focus();
            }
        }

        private void txtPresionArteria1_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtPresionArteria2.Focus();
            }
        }

        private void txtPresionArteria2_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtPulso.Focus();
            }
        }

        private void txtPulso_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtFrecuenciaRespiratoria.Focus();
            }
        }

        private void txtFrecuenciaRespiratoria_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtPeso.Focus();
            }
        }

        private void txtPeso_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtTalla.Focus();
            }
        }

        private void txtTalla_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            e.Handled = txtKeyPress(textbox, Convert.ToInt32(e.KeyChar));
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtExamenFisico.Focus();
            }
        }

        private void txtExamenFisico_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                pantab1.SelectedTab = pantab1.Tabs["OchoNueve"];
                SendKeys.SendWait("{TAB}");
                txtDiagnostico1.Focus();
            }
        }

        private void txtDiagnostico1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                if (txtDiagnostico1.Text != "")
                {
                    //txtCieDiagnostico1.Enabled = true;
                    //txtCieDiagnostico1.Focus();
                    txtDiagnostico2.Focus();
                }
            }

        }

        private void txtCieDiagnostico1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbPre1.Focus();
            }
        }

        private void chbPre1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbDef1.Focus();
            }
        }

        private void chbDef1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtDiagnostico2.Focus();
            }
        }

        private void txtDiagnostico2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                if (txtDiagnostico2.Text != "")
                {
                    //txtCieDiagnostico2.Enabled = true;
                    //txtCieDiagnostico2.Focus();
                    txtDiagnostico3.Focus();
                }
            }
        }

        private void txtCieDiagnostico2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbPre2.Focus();
            }
        }

        private void chbPre2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbDef2.Focus();
            }
        }

        private void chbDef2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtDiagnostico3.Focus();
            }
        }

        private void txtDiagnostico3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                if (txtDiagnostico3.Text != "")
                {
                    //txtCieDiagnostico3.Enabled = true;
                    //txtCieDiagnostico3.Focus();
                    txtDiagnostico4.Focus();
                }
            }
        }

        private void txtCieDiagnostico3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbPre3.Focus();
            }
        }

        private void chbPre3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbDef3.Focus();
            }
        }

        private void chbDef3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtDiagnostico4.Focus();
            }
        }

        private void txtDiagnostico4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                if (txtDiagnostico4.Text != "")
                {
                    //txtCieDiagnostico4.Enabled = true;
                    //txtCieDiagnostico4.Focus();
                    txtPlanesTratamiento.Focus();
                }
            }
        }

        private void txtCieDiagnostico4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbPre4.Focus();
            }
        }

        private void chbPre4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                chbDef4.Focus();
            }
        }

        private void chbDef4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)09)
            {
                txtPlanesTratamiento.Focus();
            }
        }

        private void txtPlanesTratamiento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)09)
            {
                pantab1.SelectedTab = pantab1.Tabs["Evolucion"];
                SendKeys.SendWait("{TAB}");
                txtEvolucion.Focus();
            }
        }

        private void chb5cp1_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp1.Checked)
                chb5sp1.Checked = false;
            else
                chb5sp1.Checked = true;
        }

        private void chb5cp2_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp2.Checked)
                chb5sp2.Checked = false;
            else
                chb5sp2.Checked = true;
        }

        private void chb5cp3_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp3.Checked)
                chb5sp3.Checked = false;
            else
                chb5sp3.Checked = true;
        }

        private void chb5cp4_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp4.Checked)
                chb5sp4.Checked = false;
            else
                chb5sp4.Checked = true;
        }

        private void chb5cp5_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp5.Checked)
                chb5sp5.Checked = false;
            else
                chb5sp5.Checked = true;
        }

        private void chb5cp6_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp6.Checked)
                chb5sp6.Checked = false;
            else
                chb5sp6.Checked = true;
        }

        private void chb5cp7_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp7.Checked)
                chb5sp7.Checked = false;
            else
                chb5sp7.Checked = true;
        }

        private void chb5cp8_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp8.Checked)
                chb5sp8.Checked = false;
            else
                chb5sp8.Checked = true;
        }

        private void chb5cp9_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp9.Checked)
                chb5sp9.Checked = false;
            else
                chb5sp9.Checked = true;
        }

        private void chb5cp10_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5cp10.Checked)
                chb5sp10.Checked = false;
            else
                chb5sp10.Checked = true;
        }

        private void chb5sp1_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp1.Checked)
                chb5cp1.Checked = false;
            else
                chb5cp1.Checked = true;
        }

        private void chb5sp2_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp2.Checked)
                chb5cp2.Checked = false;
            else
                chb5cp2.Checked = true;
        }

        private void chb5sp3_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp3.Checked)
                chb5cp3.Checked = false;
            else
                chb5cp3.Checked = true;
        }

        private void chb5sp4_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp4.Checked)
                chb5cp4.Checked = false;
            else
                chb5cp4.Checked = true;
        }

        private void chb5sp5_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp5.Checked)
                chb5cp5.Checked = false;
            else
                chb5cp5.Checked = true;
        }

        private void chb5sp6_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp6.Checked)
                chb5cp6.Checked = false;
            else
                chb5cp6.Checked = true;
        }

        private void chb5sp7_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp7.Checked)
                chb5cp7.Checked = false;
            else
                chb5cp7.Checked = true;
        }

        private void chb5sp8_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp8.Checked)
                chb5cp8.Checked = false;
            else
                chb5cp8.Checked = true;
        }

        private void chb5sp9_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp9.Checked)
                chb5cp9.Checked = false;
            else
                chb5cp9.Checked = true;
        }

        private void chb5sp10_CheckedChanged(object sender, EventArgs e)
        {
            if (chb5sp10.Checked)
                chb5cp10.Checked = false;
            else
                chb5cp10.Checked = true;
        }

        private void chb7cp1_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7cp1.Checked)
                chb7sp1.Checked = false;
            else
                chb7sp1.Checked = true;
        }

        private void chb7cp2_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7cp2.Checked)
                chb7sp2.Checked = false;
            else
                chb7sp2.Checked = true;
        }

        private void chb7cp3_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7cp3.Checked)
                chb7sp3.Checked = false;
            else
                chb7sp3.Checked = true;
        }

        private void chb7cp4_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7cp4.Checked)
                chb7sp4.Checked = false;
            else
                chb7sp4.Checked = true;
        }

        private void chb7cp5_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7cp5.Checked)
                chb7sp5.Checked = false;
            else
                chb7sp5.Checked = true;
        }

        private void chb7cp6_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7cp6.Checked)
                chb7sp6.Checked = false;
            else
                chb7sp6.Checked = true;
        }

        private void chb7sp1_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7sp1.Checked)
                chb7cp1.Checked = false;
            else
                chb7cp1.Checked = true;
        }

        private void chb7sp2_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7sp2.Checked)
                chb7cp2.Checked = false;
            else
                chb7cp2.Checked = true;
        }

        private void chb7sp3_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7sp3.Checked)
                chb7cp3.Checked = false;
            else
                chb7cp3.Checked = true;
        }

        private void chb7sp4_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7sp4.Checked)
                chb7cp4.Checked = false;
            else
                chb7cp4.Checked = true;
        }

        private void chb7sp5_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7sp5.Checked)
                chb7cp5.Checked = false;
            else
                chb7cp5.Checked = true;
        }

        private void chb7sp6_CheckedChanged(object sender, EventArgs e)
        {
            if (chb7sp6.Checked)
                chb7cp6.Checked = false;
            else
                chb7cp6.Checked = true;
        }

        private void chbPre1_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico1.Text != "")
                if (chbPre1.Checked)
                    chbDef1.Checked = false;
                else
                    chbDef1.Checked = true;
        }

        private void chbPre2_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico2.Text != "")
                if (chbPre2.Checked)
                    chbDef2.Checked = false;
                else
                    chbDef2.Checked = true;
        }

        private void chbPre3_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico3.Text != "")
                if (chbPre3.Checked)
                    chbDef3.Checked = false;
                else
                    chbDef3.Checked = true;
        }

        private void chbPre4_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico4.Text != "")
                if (chbPre4.Checked)
                    chbDef4.Checked = false;
                else
                    chbDef4.Checked = true;
        }

        private void chbDef1_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico1.Text != "")
                if (chbDef1.Checked)
                    chbPre1.Checked = false;
                else
                    chbPre1.Checked = true;
        }

        private void chbDef2_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico2.Text != "")
                if (chbDef2.Checked)
                    chbPre2.Checked = false;
                else
                    chbPre2.Checked = true;
        }

        private void chbDef3_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico3.Text != "")
                if (chbDef3.Checked)
                    chbPre3.Checked = false;
                else
                    chbPre3.Checked = true;
        }

        private void chbDef4_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDiagnostico4.Text != "")
                if (chbDef4.Checked)
                    chbPre4.Checked = false;
                else
                    chbPre4.Checked = true;
        }

        public void HabilitarBotones(bool buscar, bool guardar, bool editar, bool imprimir, bool receta, bool certificado, bool imagen, bool laboratorio, bool abrir, bool cerrar, bool Subsecuente)
        {
            btnBuscar.Enabled = buscar;
            btnGuardar.Enabled = guardar;
            btnEditar.Enabled = editar;
            btnImprimir1.Enabled = imprimir;
            btnReceta1.Enabled = receta;
            btnCertificado1.Enabled = certificado;
            btnImagen.Enabled = imagen;
            btnLaboratorio.Enabled = laboratorio;
            btnAbrir.Enabled = abrir;
            btnCerrar.Enabled = cerrar;
            btnSubsecuente.Enabled = Subsecuente;
        }
        private void btnGuarda_Click(object sender, EventArgs e)
        {
            if (Valida())
            {
                if (GuardaFormulario())
                {
                    MessageBox.Show("Información Guardada Con Exito", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    P_Central.Enabled = false;
                    HabilitarBotones(false, false, true, true, true, true, true, true, false, true, true);
                }
                else
                    MessageBox.Show("Información No Se Guardo Comuniquese Con Sistemas", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            btnGuarda.Enabled = false;
            if (datos.sexoPaciente == "Masculino")
            {
                datos.sexoPaciente = "M";
            }
            else
                datos.sexoPaciente = "F";

            NegCertificadoMedico neg = new NegCertificadoMedico();
            //HCU_form002MSP Ds = new HCU_form002MSP();
            His.Formulario.HCU_form002MSP Ds = new His.Formulario.HCU_form002MSP();
            Ds.Tables[0].Rows.Add
                (new object[]
                {
                    datos.nombrePaciente.ToString(),
                    datos.apellidoPaciemte.ToString(),
                    datos.sexoPaciente.ToString(),
                    datos.edadPaciente.ToString(),
                    datos.historiaClinica.ToString().Trim(),
                    datos.ateCodigo.ToString(),
                    datos.motivoConsulta.ToString(),
                    datos.antecedentesPersonales.ToString(),
                    datos.cardiopatia.ToString(),
                    datos.diabetes.ToString(),
                    datos.vascular.ToString(),
                    datos.hipertension.ToString(),
                    datos.cancer.ToString(),
                    datos.tuberculosis.ToString(),
                    datos.mental.ToString(),
                    datos.infeccionsa.ToString(),
                    datos.malFormado.ToString(),
                    datos.otro.ToString(),
                    datos.antecedentesFamiliares.ToString(),
                    datos.enfermedadProblemaActual.ToString(),
                    datos.sentidos.ToString(),
                    datos.sentidossp.ToString(),
                    datos.respiratorio.ToString(),
                    datos.respiratoriosp.ToString(),
                    datos.cardioVascular.ToString(),
                    datos.cardioVascularsp.ToString(),
                    datos.digestivo.ToString(),
                    datos.digestivosp.ToString(),
                    datos.genital.ToString(),
                    datos.genitalsp.ToString(),
                    datos.urinario.ToString(),
                    datos.urinariosp.ToString(),
                    datos.esqueletico.ToString(),
                    datos.esqueleticosp.ToString(),
                    datos.endocrino.ToString(),
                    datos.endocrinosp.ToString(),
                    datos.linfatico.ToString(),
                    datos.linfaticosp.ToString(),
                    datos.nervioso.ToString(),
                    datos.nerviososp.ToString(),
                    datos.detalleRevisionOrganos.ToString(),
                    datos.fechaMedicion.ToString(),
                    datos.temperatura.ToString(),
                    datos.presionArterial1.ToString(),
                    datos.presionArterial2.ToString(),
                    datos.pulso.ToString(),
                    datos.frecuenciaRespiratoria.ToString(),
                    datos.peso.ToString(),
                    datos.talla.ToString(),
                    datos.cabeza.ToString(),
                    datos.cabezasp.ToString(),
                    datos.cuello.ToString(),
                    datos.cuellosp.ToString(),
                    datos.torax.ToString(),
                    datos.toraxsp.ToString(),
                    datos.abdomen.ToString(),
                    datos.abdomensp.ToString(),
                    datos.pelvis.ToString(),
                    datos.pelvissp.ToString(),
                    datos.extremidades.ToString(),
                    datos.extremidadessp.ToString(),
                    datos.examenFisico.ToString(),
                    datos.diagnosticco1.ToString(),
                    datos.diagnosticco1cie.ToString(),
                    datos.diagnosticco1prepre.ToString(),
                    datos.diagnosticco1predef.ToString(),
                    datos.diagnosticco2.ToString(),
                    datos.diagnosticco2cie.ToString(),
                    datos.diagnosticco2prepre.ToString(),
                    datos.diagnosticco2predef.ToString(),
                    datos.diagnosticco3.ToString(),
                    datos.diagnosticco3cie.ToString(),
                    datos.diagnosticco3predef.ToString(),
                    datos.diagnosticco3prepre.ToString(),
                    datos.diagnosticco4.ToString(),
                    datos.diagnosticco4cie.ToString(),
                    datos.diagnosticco4predef.ToString(),
                    datos.diagnosticco4prepre.ToString(),
                    datos.planesTratamiento.ToString(),
                    datos.evolucion.ToString(),
                    datos.prescripciones.ToString(),
                    Convert.ToString(dtp_fechaAltaEmerencia.Value),
                    Convert.ToString(DateTime.Now.ToString("hh:mm")),
                    datos.drTratatnte.ToString(),
                    Sesion.codMedico.ToString(),
                    neg.path(),
                    Sesion.nomEmpresa
                });
            PACIENTES pacien = new PACIENTES();
            pacien = NegPacientes.recuperarPacientePorAtencion(Convert.ToInt32(txtatecodigo.Text));
            if (NegParametros.ParametroFormularios())
                datos.historiaClinica = pacien.PAC_IDENTIFICACION;
            frmReportes x = new frmReportes(1, "ConsultaExterna", Ds);
            x.Show();
            //HCU_Form002MSPrpt report = new HCU_Form002MSPrpt();
            //report.SetDataSource(Ds);
            //CrystalDecisions.Windows.Forms.CrystalReportViewer vista = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            //vista.ReportSource = report;
            //vista.PrintReport();
        }
        public void CargarPacienteExiste(Int64 ATE_CODIGO = 0) //Cambios Edgar 20210203 antes no rescataba la informacion.
        {
            try
            {
                DataTable TPaciente = new DataTable(); //Trae el paciente con todas las consultas externas
                DataTable DatosPaciente = new DataTable(); //Contiene los datos del paciente
                //TPaciente = NegConsultaExterna.ExistePaciente(Convert.ToInt64(txtatecodigo.Text)); // Cambio para subsecuentes Mario 28/01/2023
                if (ATE_CODIGO == 0)
                    TPaciente = NegConsultaExterna.ExistePaciente(Convert.ToInt64(txtatecodigo.Text));
                else
                    TPaciente = NegConsultaExterna.ExistePaciente(ATE_CODIGO);

                if (TPaciente.Rows.Count > 0)
                {
                    if (id_form002 == 0)
                        id_form002 = Convert.ToInt64(TPaciente.Rows[TPaciente.Rows.Count - 1][5].ToString());
                    DatosPaciente = NegConsultaExterna.DatosPaciente(Convert.ToInt32(id_form002));
                    if (DatosPaciente.Rows.Count > 0)
                    {
                        txtMotivo.Text = DatosPaciente.Rows[0][7].ToString();
                        txtAntecedentesPersonales.Text = DatosPaciente.Rows[0][8].ToString();
                        //cardiopatia
                        if (DatosPaciente.Rows[0][9].ToString() == "X")
                        {
                            chbCardiopatia.Checked = true;
                        }
                        else
                            chbCardiopatia.Checked = false;
                        //diabetes
                        if (DatosPaciente.Rows[0][10].ToString() == "X")
                            chbDiabetes.Checked = true;
                        else
                            chbDiabetes.Checked = false;
                        //Vascular
                        if (DatosPaciente.Rows[0][11].ToString() == "X")
                            chbVascular.Checked = true;
                        else
                            chbVascular.Checked = false;
                        //HiperTension
                        if (DatosPaciente.Rows[0][12].ToString() == "X")
                            chbHiperT.Checked = true;
                        else
                            chbHiperT.Checked = false;
                        //Cancer
                        if (DatosPaciente.Rows[0][13].ToString() == "X")
                            chbCancer.Checked = true;
                        else
                            chbCancer.Checked = false;
                        //tuberculosis
                        if (DatosPaciente.Rows[0][14].ToString() == "X")
                            chbTuberculosis.Checked = true;
                        else
                            chbTuberculosis.Checked = false;
                        //mental
                        if (DatosPaciente.Rows[0][15].ToString() == "X")
                            chbMental.Checked = true;
                        else
                            chbMental.Checked = false;
                        //infecciosa
                        if (DatosPaciente.Rows[0][16].ToString() == "X")
                            chbInfeccionsa.Checked = true;
                        else
                            chbInfeccionsa.Checked = false;
                        //malformacion
                        if (DatosPaciente.Rows[0][17].ToString() == "X")
                            chbMalFormado.Checked = true;
                        else
                            chbMalFormado.Checked = false;
                        //otro
                        if (DatosPaciente.Rows[0][18].ToString() == "X")
                            chbOtro.Checked = true;
                        else
                            chbOtro.Checked = false;
                        //antecedentes familiares
                        txtAntecedentesFamiliares.Text = DatosPaciente.Rows[0][19].ToString();
                        //enfermedad actual
                        txtEnfermedadProblema.Text = DatosPaciente.Rows[0][20].ToString();
                        //sentidos
                        if (DatosPaciente.Rows[0][21].ToString() == "X")
                            chb5cp1.Checked = true;
                        else
                            chb5cp1.Checked = false;
                        //respiratorio
                        if (DatosPaciente.Rows[0][23].ToString() == "X")
                            chb5cp2.Checked = true;
                        else
                            chb5cp2.Checked = false;
                        //CardioVascular
                        if (DatosPaciente.Rows[0][25].ToString() == "X")
                            chb5cp3.Checked = true;
                        else
                            chb5cp3.Checked = false;
                        //digestivo
                        if (DatosPaciente.Rows[0][27].ToString() == "X")
                            chb5cp4.Checked = true;
                        else
                            chb5cp4.Checked = false;
                        //genital
                        if (DatosPaciente.Rows[0][29].ToString() == "X")
                            chb5cp5.Checked = true;
                        else
                            chb5cp5.Checked = false;
                        //urinario
                        if (DatosPaciente.Rows[0][31].ToString() == "X")
                            chb5cp6.Checked = true;
                        else
                            chb5cp6.Checked = false;
                        //esqueletico
                        if (DatosPaciente.Rows[0][33].ToString() == "X")
                            chb5cp7.Checked = true;
                        else
                            chb5cp7.Checked = false;
                        //endocrino
                        if (DatosPaciente.Rows[0][35].ToString() == "X")
                            chb5cp8.Checked = true;
                        else
                            chb5cp8.Checked = false;
                        //linfatico
                        if (DatosPaciente.Rows[0][37].ToString() == "X")
                            chb5cp9.Checked = true;
                        else
                            chb5cp9.Checked = false;
                        //nervioso
                        if (DatosPaciente.Rows[0][39].ToString() == "X")
                            chb5cp10.Checked = true;
                        else
                            chb5cp10.Checked = false;
                        //revision actual
                        txtRevisionActual.Text = DatosPaciente.Rows[0][41].ToString();
                        dtpMedicion.Value = Convert.ToDateTime(DatosPaciente.Rows[0][42].ToString());
                        txtTemperatura.Text = DatosPaciente.Rows[0][43].ToString();
                        txtPresionArteria1.Text = DatosPaciente.Rows[0][44].ToString();
                        txtPresionArteria2.Text = DatosPaciente.Rows[0][45].ToString();
                        txtPulso.Text = DatosPaciente.Rows[0][46].ToString();
                        txtFrecuenciaRespiratoria.Text = DatosPaciente.Rows[0][47].ToString();
                        txtPeso.Text = DatosPaciente.Rows[0][48].ToString();
                        txtTalla.Text = DatosPaciente.Rows[0][49].ToString();
                        //cabeza
                        if (DatosPaciente.Rows[0][50].ToString() == "X")
                            chb7cp1.Checked = true;
                        else
                            chb7cp1.Checked = false;
                        //cuello
                        if (DatosPaciente.Rows[0][52].ToString() == "X")
                            chb7cp2.Checked = true;
                        else
                            chb7cp2.Checked = false;
                        //torax
                        if (DatosPaciente.Rows[0][54].ToString() == "X")
                            chb7cp3.Checked = true;
                        else
                            chb7cp3.Checked = false;
                        //abdomen
                        if (DatosPaciente.Rows[0][56].ToString() == "X")
                            chb7cp4.Checked = true;
                        else
                            chb7cp4.Checked = false;
                        //pelvis
                        if (DatosPaciente.Rows[0][58].ToString() == "X")
                            chb7cp5.Checked = true;
                        else
                            chb7cp5.Checked = false;
                        //extremidades
                        if (DatosPaciente.Rows[0][60].ToString() == "X")
                            chb7cp6.Checked = true;
                        else
                            chb7cp5.Checked = false;
                        //examen fisico
                        txtExamenFisico.Text = DatosPaciente.Rows[0][62].ToString();
                        //Cie10
                        txtDiagnostico1.Text = DatosPaciente.Rows[0][63].ToString();
                        txtCieDiagnostico1.Text = DatosPaciente.Rows[0][64].ToString();
                        if (DatosPaciente.Rows[0][65].ToString() == "X")
                            chbPre1.Checked = true;
                        else
                            chbPre1.Checked = false;

                        txtDiagnostico2.Text = DatosPaciente.Rows[0][67].ToString();
                        txtCieDiagnostico2.Text = DatosPaciente.Rows[0][68].ToString();
                        if (DatosPaciente.Rows[0][69].ToString() == "X")
                            chbPre2.Checked = true;
                        else
                            chbPre2.Checked = false;


                        txtDiagnostico3.Text = DatosPaciente.Rows[0][71].ToString();
                        txtCieDiagnostico3.Text = DatosPaciente.Rows[0][72].ToString();
                        if (DatosPaciente.Rows[0][73].ToString() == "X")
                            chbPre3.Checked = true;
                        else
                            chbPre3.Checked = false;


                        txtDiagnostico4.Text = DatosPaciente.Rows[0][75].ToString();
                        txtCieDiagnostico4.Text = DatosPaciente.Rows[0][76].ToString();
                        if (DatosPaciente.Rows[0][77].ToString() == "X")
                            chbPre4.Checked = true;
                        else
                            chbPre4.Checked = false;
                        //tratamiento
                        txtPlanesTratamiento.Text = DatosPaciente.Rows[0][79].ToString();
                        txtEvolucion.Text = DatosPaciente.Rows[0][80].ToString();

                        txtindicaciones.Text = DatosPaciente.Rows[0][81].ToString();
                        dtp_fechaAltaEmerencia.Value = Convert.ToDateTime(DatosPaciente.Rows[0][82].ToString());
                        txt_horaAltaEmerencia.Text = Convert.ToDateTime(DatosPaciente.Rows[0][82].ToString()).ToShortTimeString();
                        txt_profesionalEmergencia.Text = DatosPaciente.Rows[0][84].ToString();
                        txt_CodMSPE.Text = DatosPaciente.Rows[0][85].ToString();
                        ATENCIONES validaAtencion = NegAtenciones.RecuperarAtencionID(Convert.ToInt64(txtatecodigo.Text.Trim()));
                        //FORMULARIOS_MSP_CERRADOS cerrado = NegConsultaExterna.ValidaCerrado(Convert.ToInt64(txtatecodigo.Text.Trim()));
                        if (subsecuente != null)
                        {
                            if (!NegConsultaExterna.PacienteCerradaCxE(subsecuente.ate_codigo_subsecuente))
                            {
                                HabilitarBotones(false, false, true, true, true, true, true, true, false, true, true);

                            }
                            else
                            {
                                HabilitarBotones(false, false, false, true, true, true, true, true, true, false, true);

                            }
                        }
                        else
                        {
                            if (validaAtencion.ESC_CODIGO != 1)
                            {
                                HabilitarBotones(false, false, false, true, true, true, true, true, true, false, false);
                                P_Central.Enabled = false;
                            }
                            else
                            {
                                HabilitarBotones(false, false, false, true, true, true, true, true, false, true, true);
                                P_Central.Enabled = true;
                            }

                        }
                    }
                    nuevaConsulta = false;
                }
                else
                {
                    HabilitarBotones(false, true, false, false, false, false, false, false, false, false, false);
                    P_Central.Enabled = true;
                    nuevaConsulta = true;
                }
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Usted Va Salir Del Formulario", "HIS3000", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                this.Close();
        }


        public TextBox txthistoria = new TextBox();
        public TextBox txtatecodigo = new TextBox();
        public TextBox txtaseguradora = new TextBox();
        Form002MSP consultaExterna = new Form002MSP();
        SIGNOSVITALES_CONSULTAEXTERNA sign = new SIGNOSVITALES_CONSULTAEXTERNA();
        private void btnBuscarPaciente_Click(object sender, EventArgs e)
        {
            DataTable paciente = new DataTable();
            //frmAyudaPacientesFacturacion ayuda = new frmAyudaPacientesFacturacion();
            //ayuda.campoPadre = txthistoria;
            //ayuda.campoAtencion = txtatecodigo;
            //ayuda.campoAseguradora = txtaseguradora;
            //ayuda.ShowDialog();
            Emergencia.frm_AyudaPacientes ayuda = new Emergencia.frm_AyudaPacientes(emergecia);
            ayuda.campoPadre = txthistoria;
            ayuda.campoAtencion = txtatecodigo;
            //ayuda.campoFecha = fechaNacimimiento;
            //ayuda.campoFechaAtencion = fechaAtencion;
            //ayuda.campoId = id;
            ayuda.triaje = true;
            //ayuda.campoAseguradora = txtaseguradora;
            ayuda.ShowDialog();
            if (txthistoria.Text != "")
            {
                paciente = NegConsultaExterna.RecuperaPaciente(Convert.ToInt64(txtatecodigo.Text));
                DataTable signos = new DataTable();
                signos = NegConsultaExterna.RecuperaSignos(Convert.ToInt64(txtatecodigo.Text));

                ////CONSULTO SI PACIENTE YA TIENE CONSULTA EXTERNA
                //consultaExterna = NegConsultaExterna.PacienteExisteCxE(txtatecodigo.Text.Trim());
                //if (consultaExterna != null)
                //    CargarPacienteExiste();

                if (signos.Rows.Count > 0)
                {
                    string enteros = signos.Rows[0][3].ToString();
                    string[] final = enteros.Split('.');
                    txtPresionArteria1.Text = final[0];
                    enteros = signos.Rows[0][4].ToString();
                    final = enteros.Split('.');
                    txtPresionArteria2.Text = final[0];
                    enteros = signos.Rows[0][5].ToString();
                    final = enteros.Split('.');
                    txtPulso.Text = final[0];
                    enteros = signos.Rows[0][6].ToString();
                    final = enteros.Split('.');
                    txtFrecuenciaRespiratoria.Text = final[0];
                    enteros = signos.Rows[0][7].ToString();
                    final = enteros.Split('.');
                    txtTemperatura.Text = final[0];
                    final = enteros.Split('.');
                    txtTemperatura.Text = signos.Rows[0][8].ToString();
                    enteros = signos.Rows[0][10].ToString();
                    final = enteros.Split('.');
                    txtPeso.Text = final[0];
                    enteros = signos.Rows[0][11].ToString();
                    final = enteros.Split('.');
                    txtTalla.Text = signos.Rows[0][11].ToString();
                }
                lblHistoria.Text = txthistoria.Text;
                lblAteCodigo.Text = txtatecodigo.Text;
                //lblAseguradora.Text = txtaseguradora.Text;
                lblNombre.Text = paciente.Rows[0][0].ToString();
                lblApellido.Text = paciente.Rows[0][1].ToString();
                lblEdad.Text = paciente.Rows[0][2].ToString();
                if (paciente.Rows[0][3].ToString().Trim() == "M")
                {
                    lblSexo.Text = "Masculino";
                }
                else
                {
                    lblSexo.Text = "Femenino";
                }
                CargarPaciente();
                P_Central.Visible = true;
                P_Datos.Visible = true;
                btnBuscarPaciente.Visible = false;
                btnGuarda.Visible = true;
                btnNuevo.Visible = true;
            }
        }
        public void CargarPaciente() //Cambios Edgar 20210203 antes no rescataba la informacion.
        {
            try
            {
                btnImprimir.Visible = true;
                btnGuarda.Visible = false;
                DataTable TPaciente = new DataTable(); //Trae el paciente con todas las consultas externas
                DataTable DatosPaciente = new DataTable(); //Contiene los datos del paciente
                TPaciente = NegConsultaExterna.ExistePaciente(Convert.ToInt64(txtatecodigo.Text));

                if (TPaciente.Rows.Count > 0)
                {
                    frm_PacientesConsultaExterna x = new frm_PacientesConsultaExterna();
                    x.Pacientes = TPaciente;
                    x.ShowDialog();
                    if (x.codigoConsultaExterna != "")
                    {
                        DatosPaciente = NegConsultaExterna.DatosPaciente(Convert.ToInt32(x.codigoConsultaExterna));
                        if (DatosPaciente.Rows.Count > 0)
                        {
                            txtMotivo.Text = DatosPaciente.Rows[0][7].ToString();
                            txtAntecedentesPersonales.Text = DatosPaciente.Rows[0][8].ToString();
                            //cardiopatia
                            if (DatosPaciente.Rows[0][9].ToString() == "X")
                            {
                                chbCardiopatia.Checked = true;
                            }
                            else
                                chbCardiopatia.Checked = false;
                            //diabetes
                            if (DatosPaciente.Rows[0][10].ToString() == "X")
                                chbDiabetes.Checked = true;
                            else
                                chbDiabetes.Checked = false;
                            //Vascular
                            if (DatosPaciente.Rows[0][11].ToString() == "X")
                                chbVascular.Checked = true;
                            else
                                chbVascular.Checked = false;
                            //HiperTension
                            if (DatosPaciente.Rows[0][12].ToString() == "X")
                                chbHiperT.Checked = true;
                            else
                                chbHiperT.Checked = false;
                            //Cancer
                            if (DatosPaciente.Rows[0][13].ToString() == "X")
                                chbCancer.Checked = true;
                            else
                                chbCancer.Checked = false;
                            //tuberculosis
                            if (DatosPaciente.Rows[0][14].ToString() == "X")
                                chbTuberculosis.Checked = true;
                            else
                                chbTuberculosis.Checked = false;
                            //mental
                            if (DatosPaciente.Rows[0][15].ToString() == "X")
                                chbMental.Checked = true;
                            else
                                chbMental.Checked = false;
                            //infecciosa
                            if (DatosPaciente.Rows[0][16].ToString() == "X")
                                chbInfeccionsa.Checked = true;
                            else
                                chbInfeccionsa.Checked = false;
                            //malformacion
                            if (DatosPaciente.Rows[0][17].ToString() == "X")
                                chbMalFormado.Checked = true;
                            else
                                chbMalFormado.Checked = false;
                            //otro
                            if (DatosPaciente.Rows[0][18].ToString() == "X")
                                chbOtro.Checked = true;
                            else
                                chbOtro.Checked = false;
                            //antecedentes familiares
                            txtAntecedentesFamiliares.Text = DatosPaciente.Rows[0][19].ToString();
                            //enfermedad actual
                            txtEnfermedadProblema.Text = DatosPaciente.Rows[0][20].ToString();
                            //sentidos
                            if (DatosPaciente.Rows[0][21].ToString() == "X")
                                chb5cp1.Checked = true;
                            else
                                chb5cp1.Checked = false;
                            //respiratorio
                            if (DatosPaciente.Rows[0][23].ToString() == "X")
                                chb5cp2.Checked = true;
                            else
                                chb5cp2.Checked = false;
                            //CardioVascular
                            if (DatosPaciente.Rows[0][25].ToString() == "X")
                                chb5cp3.Checked = true;
                            else
                                chb5cp3.Checked = false;
                            //digestivo
                            if (DatosPaciente.Rows[0][27].ToString() == "X")
                                chb5cp4.Checked = true;
                            else
                                chb5cp4.Checked = false;
                            //genital
                            if (DatosPaciente.Rows[0][29].ToString() == "X")
                                chb5cp5.Checked = true;
                            else
                                chb5cp5.Checked = false;
                            //urinario
                            if (DatosPaciente.Rows[0][31].ToString() == "X")
                                chb5cp6.Checked = true;
                            else
                                chb5cp6.Checked = false;
                            //esqueletico
                            if (DatosPaciente.Rows[0][33].ToString() == "X")
                                chb5cp7.Checked = true;
                            else
                                chb5cp7.Checked = false;
                            //endocrino
                            if (DatosPaciente.Rows[0][35].ToString() == "X")
                                chb5cp8.Checked = true;
                            else
                                chb5cp8.Checked = false;
                            //linfatico
                            if (DatosPaciente.Rows[0][37].ToString() == "X")
                                chb5cp9.Checked = true;
                            else
                                chb5cp9.Checked = false;
                            //nervioso
                            if (DatosPaciente.Rows[0][39].ToString() == "X")
                                chb5cp10.Checked = true;
                            else
                                chb5cp10.Checked = false;
                            //revision actual
                            txtRevisionActual.Text = DatosPaciente.Rows[0][41].ToString();
                            dtpMedicion.Value = Convert.ToDateTime(DatosPaciente.Rows[0][42].ToString());
                            txtTemperatura.Text = DatosPaciente.Rows[0][43].ToString();
                            txtPresionArteria1.Text = DatosPaciente.Rows[0][44].ToString();
                            txtPresionArteria2.Text = DatosPaciente.Rows[0][45].ToString();
                            txtPulso.Text = DatosPaciente.Rows[0][46].ToString();
                            txtFrecuenciaRespiratoria.Text = DatosPaciente.Rows[0][47].ToString();
                            txtPeso.Text = DatosPaciente.Rows[0][48].ToString();
                            txtTalla.Text = DatosPaciente.Rows[0][49].ToString();
                            //cabeza
                            if (DatosPaciente.Rows[0][50].ToString() == "X")
                                chb7cp1.Checked = true;
                            else
                                chb7cp1.Checked = false;
                            //cuello
                            if (DatosPaciente.Rows[0][52].ToString() == "X")
                                chb7cp2.Checked = true;
                            else
                                chb7cp2.Checked = false;
                            //torax
                            if (DatosPaciente.Rows[0][54].ToString() == "X")
                                chb7cp3.Checked = true;
                            else
                                chb7cp3.Checked = false;
                            //abdomen
                            if (DatosPaciente.Rows[0][56].ToString() == "X")
                                chb7cp4.Checked = true;
                            else
                                chb7cp4.Checked = false;
                            //pelvis
                            if (DatosPaciente.Rows[0][58].ToString() == "X")
                                chb7cp5.Checked = true;
                            else
                                chb7cp5.Checked = false;
                            //extremidades
                            if (DatosPaciente.Rows[0][60].ToString() == "X")
                                chb7cp6.Checked = true;
                            else
                                chb7cp5.Checked = false;
                            //examen fisico
                            txtExamenFisico.Text = DatosPaciente.Rows[0][62].ToString();
                            //Cie10
                            txtDiagnostico1.Text = DatosPaciente.Rows[0][63].ToString();
                            txtCieDiagnostico1.Text = DatosPaciente.Rows[0][64].ToString();
                            if (DatosPaciente.Rows[0][65].ToString() == "X")
                                chbPre1.Checked = true;
                            else
                                chbPre1.Checked = false;

                            txtDiagnostico2.Text = DatosPaciente.Rows[0][67].ToString();
                            txtCieDiagnostico2.Text = DatosPaciente.Rows[0][68].ToString();
                            if (DatosPaciente.Rows[0][69].ToString() == "X")
                                chbPre2.Checked = true;
                            else
                                chbPre2.Checked = false;


                            txtDiagnostico3.Text = DatosPaciente.Rows[0][71].ToString();
                            txtCieDiagnostico3.Text = DatosPaciente.Rows[0][72].ToString();
                            if (DatosPaciente.Rows[0][73].ToString() == "X")
                                chbPre3.Checked = true;
                            else
                                chbPre3.Checked = false;


                            txtDiagnostico4.Text = DatosPaciente.Rows[0][75].ToString();
                            txtCieDiagnostico4.Text = DatosPaciente.Rows[0][76].ToString();
                            if (DatosPaciente.Rows[0][77].ToString() == "X")
                                chbPre4.Checked = true;
                            else
                                chbPre4.Checked = false;
                            //tratamiento
                            txtPlanesTratamiento.Text = DatosPaciente.Rows[0][79].ToString();
                            txtEvolucion.Text = DatosPaciente.Rows[0][80].ToString();

                            txtindicaciones.Text = DatosPaciente.Rows[0][81].ToString();
                            dtp_fechaAltaEmerencia.Value = Convert.ToDateTime(DatosPaciente.Rows[0][82].ToString());
                            txt_horaAltaEmerencia.Text = DatosPaciente.Rows[0][83].ToString();
                            txt_profesionalEmergencia.Text = DatosPaciente.Rows[0][84].ToString();
                            txt_CodMSPE.Text = DatosPaciente.Rows[0][85].ToString();
                        }
                    }
                    nuevaConsulta = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtCieDiagnostico1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {

                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {

                    txtCieDiagnostico1.Text = busqueda.codigo;
                    chbPre1.Checked = true;
                }

            }
        }
        private void txtCieDiagnostico2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {

                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {

                    txtCieDiagnostico2.Text = busqueda.codigo;
                    chbPre2.Checked = true;
                }

            }
        }

        private void txtCieDiagnostico3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {

                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {

                    txtCieDiagnostico3.Text = busqueda.codigo;
                    chbPre3.Checked = true;
                }

            }
        }


        private void txtCieDiagnostico4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {

                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {

                    txtCieDiagnostico4.Text = busqueda.codigo;
                    chbPre4.Checked = true;
                }

            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Usted va generar una nueva consulta externa. ¿Desea Continuar?", "HIS3000", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                nuevaConsulta = true;
                limpiarCampos();
                //P_Central.Visible = false;
                P_Datos.Visible = false;
                btnNuevo.Visible = false;
                btnBuscarPaciente.Visible = false;
            }
        }

        private void gridPrescripciones_CellValueChanged(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            try
            {
                if (band == false)
                {
                    if (e.ColumnIndex == gridPrescripciones.Columns["PRES_ESTADO"].Index)
                    {
                        DataGridViewCheckBoxCell chkCell = (DataGridViewCheckBoxCell)gridPrescripciones.Rows[e.RowIndex].Cells["PRES_ESTADO"];
                        if (chkCell.Value != null)
                        {
                            if ((bool)chkCell.Value == true)
                            {
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FARMACOS_INSUMOS"].ReadOnly = false;
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].ReadOnly = false;
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].Value = DateTime.Now;
                            }
                            else
                            {
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FARMACOS_INSUMOS"].ReadOnly = false;
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FARMACOS_INSUMOS"].Value = string.Empty;
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].ReadOnly = true;
                                gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].Value = null;
                            }
                        }
                        else
                        {
                            gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FARMACOS_INSUMOS"].ReadOnly = false;
                            gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FARMACOS_INSUMOS"].Value = string.Empty;
                            gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].ReadOnly = true;
                            gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].Value = null;
                        }

                    }
                    else
                    {
                        gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FARMACOS_INSUMOS"].ReadOnly = false;
                        gridPrescripciones.Rows[e.RowIndex].Cells["PRES_FECHA"].ReadOnly = false;
                    }
                }
            }
            catch
            {

            }
        }

        private void btnreceta_Click(object sender, EventArgs e)
        {
            if (NegParametros.ParametroReceta())
            {
                frmRecetaNew x = new frmRecetaNew();
                x.Show();
            }
            else
            {
                His.Formulario.frm_RecetaMedica x = new His.Formulario.frm_RecetaMedica();
                x.Show();
            }
            //frmRecetaNew x = new frmRecetaNew();
            //x.ShowDialog();
        }

        private void btnCertificado_Click(object sender, EventArgs e)
        {
            frm_Certificados x = new frm_Certificados();
            x.ShowDialog();
        }

        public bool ValidarCie10(string cie10)
        {
            if (txtCieDiagnostico1.Text.Trim() == cie10)
                return false;
            if (txtCieDiagnostico2.Text.Trim() == cie10)
                return false;
            if (txtCieDiagnostico3.Text.Trim() == cie10)
                return false;
            if (txtCieDiagnostico4.Text.Trim() == cie10)
                return false;
            else
                return true;
        }
        private void txtDiagnostico1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {
                    if (ValidarCie10(busqueda.codigo))
                    {
                        txtCieDiagnostico1.Text = busqueda.codigo;
                        txtDiagnostico1.Text = busqueda.resultado;
                        chbPre1.Checked = true;
                    }
                    else
                    {
                        MessageBox.Show("Cie10 ya ha sido agregado.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    txtCieDiagnostico1.Text = "";
                    txtDiagnostico1.Text = "";
                    chbPre1.Checked = false;
                    chbDef1.Checked = false;
                }
            }
        }

        private void txtDiagnostico2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {
                    if (ValidarCie10(busqueda.codigo))
                    {
                        txtCieDiagnostico2.Text = busqueda.codigo;
                        txtDiagnostico2.Text = busqueda.resultado;
                        chbPre2.Checked = true;
                    }
                    else
                    {
                        MessageBox.Show("Cie10 ya ha sido agregado.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    txtCieDiagnostico2.Text = "";
                    txtDiagnostico2.Text = "";
                    chbPre2.Checked = false;
                    chbDef2.Checked = false;
                }
            }
        }

        private void txtDiagnostico3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {
                    if (ValidarCie10(busqueda.codigo))
                    {
                        txtCieDiagnostico3.Text = busqueda.codigo;
                        txtDiagnostico3.Text = busqueda.resultado;
                        chbPre3.Checked = true;
                    }
                    else
                    {
                        MessageBox.Show("Cie10 ya ha sido agregado.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    txtCieDiagnostico3.Text = "";
                    txtDiagnostico3.Text = "";
                    chbPre3.Checked = false;
                    chbDef3.Checked = false;
                }
            }
        }

        private void txtDiagnostico4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                frm_BusquedaCIE10 busqueda = new frm_BusquedaCIE10();
                busqueda.ShowDialog();
                if (busqueda.codigo != null)
                {
                    if (ValidarCie10(busqueda.codigo))
                    {
                        txtCieDiagnostico4.Text = busqueda.codigo;
                        txtDiagnostico4.Text = busqueda.resultado;
                        chbPre4.Checked = true;
                    }
                    else
                    {
                        MessageBox.Show("Cie10 ya ha sido agregado.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            else
            {
                if (e.KeyCode == Keys.Delete)
                {
                    txtCieDiagnostico4.Text = "";
                    txtDiagnostico4.Text = "";
                    chbPre4.Checked = false;
                    chbDef4.Checked = false;
                }
            }
        }
        public void limpiarCampos()
        {
            txtMotivo.Text = "";
            txtAntecedentesPersonales.Text = "";
            if (chb5cp1.Checked)
                chb5cp1.Checked = false;
            if (chb5cp10.Checked)
                chb5cp10.Checked = false;
            if (chb5cp2.Checked)
                chb5cp2.Checked = false;
            if (chb5cp3.Checked)
                chb5cp3.Enabled = false;
            if (chb5cp4.Checked)
                chb5cp4.Checked = false;
            if (chb5cp5.Checked)
                chb5cp5.Checked = false;
            if (chb5cp6.Checked)
                chb5cp6.Checked = false;
            if (chb5cp7.Checked)
                chb5cp7.Checked = false;
            if (chb5cp8.Checked)
                chb5cp8.Checked = false;
            if (chb5cp9.Checked)
                chb5cp9.Checked = false;
            if (chbCardiopatia.Checked)
                chbCardiopatia.Checked = false;
            if (chbDiabetes.Checked)
                chbDiabetes.Checked = false;
            if (chbVascular.Checked)
                chbVascular.Checked = false;
            if (chbHiperT.Checked)
                chbHiperT.Checked = false;
            if (chbCancer.Checked)
                chbCancer.Checked = false;
            if (chbTuberculosis.Checked)
                chbTuberculosis.Checked = false;
            if (chbMental.Checked)
                chbMental.Checked = false;
            if (chbInfeccionsa.Checked)
                chbInfeccionsa.Checked = false;
            if (chbMalFormado.Checked)
                chbMalFormado.Checked = false;
            if (chbOtro.Checked)
                chbOtro.Checked = false;
            txtAntecedentesFamiliares.Text = "";
            txtEnfermedadProblema.Text = "";
            txtRevisionActual.Text = "";
            txtTemperatura.Text = "";
            txtPresionArteria1.Text = "";
            txtPresionArteria2.Text = "";
            txtPulso.Text = "";
            txtFrecuenciaRespiratoria.Text = "";
            txtPeso.Text = "";
            txtTalla.Text = "";
            txtExamenFisico.Text = "";
            txtDiagnostico1.Text = "";
            txtDiagnostico2.Text = "";
            txtDiagnostico3.Text = "";
            txtDiagnostico4.Text = "";
            txtCieDiagnostico1.Text = "";
            txtCieDiagnostico2.Text = "";
            txtCieDiagnostico3.Text = "";
            txtCieDiagnostico4.Text = "";

            txtPlanesTratamiento.Text = "";
            txtEvolucion.Text = "";
            txtindicaciones.Text = "";
            txt_horaAltaEmerencia.Text = DateTime.Now.ToString("hh:mm");
        }
        private void Consulta_Load(object sender, EventArgs e)
        {
            HabilitarBotones(true, false, false, false, false, false, false, false, false, false, false);
            List<PERFILES> perfilUsuario = new NegPerfil().RecuperarPerfil(His.Entidades.Clases.Sesion.codUsuario);
            foreach (var item in perfilUsuario)
            {
                List<ACCESO_OPCIONES> accop = NegUtilitarios.ListaAccesoOpcionesPorPerfil(item.ID_PERFIL, 7);
                foreach (var items in accop)
                {
                    if (items.ID_ACCESO == 71110)// se cambia del perfil  29 a opcion 71110// Mario Valencia 14/11/2023 // cambio en seguridades.
                    {
                        mushuñan = true;
                        Int16 AreaUsuario = 1;
                        DataTable codigoAreaAsignada = NegUsuarios.AreaAsignada(Convert.ToInt16(His.Entidades.Clases.Sesion.codUsuario));
                        bool parse = Int16.TryParse(codigoAreaAsignada.Rows[0][0].ToString(), out AreaUsuario);
                        if (parse)
                        {
                            switch (AreaUsuario)
                            {
                                case 2:
                                    //button1.Visible = true;
                                    break;
                                case 3:
                                    button1.Visible = true;
                                    txt_profesionalEmergencia.Text = "";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void toolStripButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            DataTable paciente = new DataTable();
            if (mushuñan)
                emergecia = true;
            Emergencia.frm_AyudaPacientes ayuda = new Emergencia.frm_AyudaPacientes(emergecia);
            ayuda.campoPadre = txthistoria;
            ayuda.campoAtencion = txtatecodigo;
            ayuda.triaje = false;
            ayuda.mushunia = mushuñan;
            if (Sesion.codDepartamento == 1)
                ayuda.sistemas = true;
            ayuda.ShowDialog();
            if (txthistoria.Text != "")
            {
                subsecuente = NegAtenciones.RecuperaAtencionSub(Convert.ToInt64(txtatecodigo.Text));
                DataTable signos = new DataTable();
                if (subsecuente != null)
                {
                    paciente = NegConsultaExterna.RecuperaPaciente(subsecuente.ate_codigo_principal);
                    signos = NegConsultaExterna.RecuperaSignos(subsecuente.ate_codigo_principal);
                    //CONSULTO SI PACIENTE YA TIENE CONSULTA EXTERNA
                    consultaExterna = NegConsultaExterna.PacienteExisteCxE(Convert.ToString(subsecuente.ate_codigo_principal));
                    if (!NegConsultaExterna.PacienteCerradaCxE(subsecuente.ate_codigo_subsecuente))
                    {
                        HabilitarBotones(false, false, true, true, true, true, true, true, false, true, true);

                    }
                    else
                    {
                        HabilitarBotones(false, false, false, true, true, true, true, true, true, false, true);

                    }
                    if (consultaExterna != null)
                        CargarPacienteExiste(subsecuente.ate_codigo_principal);
                    else
                    {
                        HabilitarBotones(false, true, false, false, false, false, false, false, false, false, false);
                        nuevaConsulta = true;
                        P_Central.Enabled = true;
                    }
                }
                else
                {
                    paciente = NegConsultaExterna.RecuperaPaciente(Convert.ToInt64(txtatecodigo.Text));
                    signos = NegConsultaExterna.RecuperaSignos(Convert.ToInt64(txtatecodigo.Text));
                    //CONSULTO SI PACIENTE YA TIENE CONSULTA EXTERNA
                    consultaExterna = NegConsultaExterna.PacienteExisteCxE(txtatecodigo.Text);
                    if (consultaExterna != null)
                    {
                        CargarPacienteExiste();

                        if (!NegConsultaExterna.PacienteCerradaCxE(Convert.ToInt64(txtatecodigo.Text)))
                        {
                            HabilitarBotones(false, false, true, true, true, true, true, true, false, true, true);

                        }
                        else
                        {
                            HabilitarBotones(false, false, false, true, true, true, true, true, true, false, true);

                        }
                    }
                    else
                    {
                        HabilitarBotones(false, true, false, false, false, false, false, false, false, false, false);
                        nuevaConsulta = true;
                        P_Central.Enabled = true;
                    }
                }

                if (signos.Rows.Count > 0)
                {
                    string enteros = signos.Rows[0][3].ToString();
                    string[] final = enteros.Split('.');
                    txtPresionArteria1.Text = final[0];
                    enteros = signos.Rows[0][4].ToString();
                    final = enteros.Split('.');
                    txtPresionArteria2.Text = final[0];
                    //enteros = signos.Rows[0][5].ToString();
                    //final = enteros.Split('.');
                    txtPulso.Text = signos.Rows[0][5].ToString();
                    //enteros = signos.Rows[0][6].ToString();
                    //final = enteros.Split('.');
                    txtFrecuenciaRespiratoria.Text = signos.Rows[0][6].ToString();
                    txtFrecuenciaCardiaca.Text = signos.Rows[0][5].ToString();
                    txtSaturaOxigeno.Text = signos.Rows[0][9].ToString();
                    txtIndice.Text = signos.Rows[0][12].ToString();
                    txtGlicemiaCapilar.Text = signos.Rows[0][14].ToString();
                    txtGlasgow.Text = signos.Rows[0][15].ToString();
                    txtOcular.Text = signos.Rows[0][16].ToString();
                    txtVerval.Text = signos.Rows[0][17].ToString();
                    txtMotora.Text = signos.Rows[0][18].ToString();
                    txtDiametroDer.Text = signos.Rows[0][19].ToString();
                    txtReaccionDer.Text = signos.Rows[0][20].ToString();
                    txtDiametroIz.Text = signos.Rows[0][21].ToString();
                    txtReaccionIz.Text = signos.Rows[0][22].ToString();

                    //enteros = signos.Rows[0][7].ToString();
                    //final = enteros.Split('.');
                    txtTemperatura.Text = signos.Rows[0][7].ToString();
                    final = enteros.Split('.');
                    txtTemperatura.Text = signos.Rows[0][8].ToString();
                    //enteros = signos.Rows[0][10].ToString();
                    //final = enteros.Split('.');
                    txtPeso.Text = signos.Rows[0][10].ToString();
                    enteros = signos.Rows[0][11].ToString();
                    final = enteros.Split('.');
                    txtTalla.Text = signos.Rows[0][11].ToString();
                }
                lblHistoria.Text = txthistoria.Text;
                lblAteCodigo.Text = txtatecodigo.Text;
                lblNombre.Text = paciente.Rows[0][0].ToString();
                lblApellido.Text = paciente.Rows[0][1].ToString();
                lblEdad.Text = paciente.Rows[0][2].ToString();

                if (paciente.Rows[0][3].ToString().Trim() == "M")
                {
                    lblSexo.Text = "Masculino";
                }
                else
                {
                    lblSexo.Text = "Femenino";
                }
                //CargarPaciente(); //aqui se puede ver las n consultas externas del paciente
                P_Central.Visible = true;
                P_Datos.Visible = true;
                //btnBuscarPaciente.Visible = false;
                //btnGuarda.Visible = true;
                //btnNuevo.Visible = true;

            }
        }
        public bool mushuñan = false;
        public void imprimir()
        {
            subsecuente = NegAtenciones.RecuperaAtencionSub(Convert.ToInt64(txtatecodigo.Text));
            if (subsecuente != null)
            {
                consultaExterna = NegConsultaExterna.PacienteExisteCxE(Convert.ToString(subsecuente.ate_codigo_principal));
                sign = NegConsultaExterna.signoscitalesCex(subsecuente.ate_codigo_principal);
                if (sign == null)
                {
                    MessageBox.Show("No se puede imprimir hasta haber  completado la hora de triaje.", "His3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                consultaExterna = NegConsultaExterna.PacienteExisteCxE(txtatecodigo.Text.Trim());
                sign = NegConsultaExterna.signoscitalesCex(Convert.ToInt64(txtatecodigo.Text.Trim()));
                if (sign == null)
                {
                    MessageBox.Show("No se puede imprimir hasta haber  completado la hora de triaje.", "His3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            double tot1 = 0;
            try
            {
                tot1 = sign.Ocular + sign.Verbal + sign.Motora;
            }
            catch (Exception ex)
            {
                //throw;
            }
            SUCURSALES sucursal = new SUCURSALES();
            string logo = "";
            string empresa = "";
            if (mushuñan)
            {
                Int32 ingreso = NegTipoIngreso.RecuperarporAtencion(Convert.ToInt64(txtatecodigo.Text));
                switch (ingreso)
                {
                    case 10:
                        logo = NegUtilitarios.RutaLogo("Mushuñan");
                        empresa = "SANTA CATALINA DE SENA";
                        break;
                    case 12:
                        logo = NegUtilitarios.RutaLogo("BrigadaMedica");
                        empresa = "BRIGADAS MEDICAS";
                        break;
                    default:
                        logo = NegUtilitarios.RutaLogo("General");
                        empresa = Sesion.nomEmpresa;
                        break;
                }
            }
            else
            {
                logo = NegUtilitarios.RutaLogo("General");
                empresa = Sesion.nomEmpresa;
            }
            if (consultaExterna != null)
            {
                if (consultaExterna.Sexo == "Masculino")
                {
                    datos.sexoPaciente = "M";
                }
                else
                    datos.sexoPaciente = "F";

                PACIENTES pacien = new PACIENTES();
                pacien = NegPacientes.recuperarPacientePorAtencion(Convert.ToInt32(txtatecodigo.Text));
                if (NegParametros.ParametroFormularios())
                    datos.historiaClinica = pacien.PAC_IDENTIFICACION;
                else
                    datos.historiaClinica = consultaExterna.Historia;

                NegCertificadoMedico neg = new NegCertificadoMedico();
                //HCU_form002MSP Ds = new HCU_form002MSP();
                His.Formulario.HCU_form002MSP Ds = new His.Formulario.HCU_form002MSP();
                Ds.Tables[0].Rows.Add
                    (new object[]
                    {
                    consultaExterna.Nombre.ToString(),
                    consultaExterna.Apellido.ToString(),
                    datos.sexoPaciente.ToString(),
                    consultaExterna.Edad.ToString(),
                    datos.historiaClinica.ToString().Trim(),
                    consultaExterna.AteCodigo.ToString(),
                    consultaExterna.Motivo.ToString(),
                    consultaExterna.AntecedentesPersonales.ToString(),
                    consultaExterna.Cardiopatia.ToString(),
                    consultaExterna.Diabetes.ToString(),
                    consultaExterna.Vascular.ToString(),
                    consultaExterna.Hipertencion.ToString(),
                    consultaExterna.Cancer.ToString(),
                    consultaExterna.tuberculosis.ToString(),
                    consultaExterna.mental.ToString(),
                    consultaExterna.infecciosa.ToString(),
                    consultaExterna.malformacion.ToString(),
                    consultaExterna.otro.ToString(),
                    consultaExterna.antecedentesFamiliares.ToString(),
                    consultaExterna.enfermedadActual.ToString(),
                    consultaExterna.sentidos.ToString(),
                    consultaExterna.sentidossp.ToString(),
                    consultaExterna.respiratorio.ToString(),
                    consultaExterna.respiratoriosp.ToString(),
                    consultaExterna.cardioVascular.ToString(),
                    consultaExterna.cardioVascularsp.ToString(),
                    consultaExterna.digestivo.ToString(),
                    consultaExterna.digestivosp.ToString(),
                    consultaExterna.genital.ToString(),
                    consultaExterna.genitalsp.ToString(),
                    consultaExterna.urinario.ToString(),
                    consultaExterna.urinariosp.ToString(),
                    consultaExterna.esqueletico.ToString(),
                    consultaExterna.esqueleticosp.ToString(),
                    consultaExterna.endocrino.ToString(),
                    consultaExterna.endocrinosp.ToString(),
                    consultaExterna.linfatico.ToString(),
                    consultaExterna.linfaticosp.ToString(),
                    consultaExterna.nervioso.ToString(),
                    consultaExterna.nerviososp.ToString(),
                    consultaExterna.revisionactual.ToString(),
                    consultaExterna.fechamedicion.ToString(),
                    consultaExterna.temperatura.ToString(),
                    consultaExterna.presion1.ToString(),
                    consultaExterna.presion2.ToString(),
                    consultaExterna.pulso.ToString(),
                    consultaExterna.frecuenciaRespiratoria.ToString(),
                    consultaExterna.peso.ToString(),
                    consultaExterna.talla.ToString(),
                    consultaExterna.cabeza.ToString(),
                    consultaExterna.cabezasp.ToString(),
                    consultaExterna.cuello.ToString(),
                    consultaExterna.cuellosp.ToString(),
                    consultaExterna.torax.ToString(),
                    consultaExterna.toraxsp.ToString(),
                    consultaExterna.abdomen.ToString(),
                    consultaExterna.abdomensp.ToString(),
                    consultaExterna.pelvis.ToString(),
                    consultaExterna.pelvissp.ToString(),
                    consultaExterna.extremidades.ToString(),
                    consultaExterna.extremidadessp.ToString(),
                    consultaExterna.examenFisico.ToString(),
                    consultaExterna.diagnostico1.ToString(),
                    consultaExterna.diagnostico1cie.ToString(),
                    consultaExterna.diagnostico1pre.ToString(),
                    consultaExterna.diagnostico1def.ToString(),
                    consultaExterna.diagnostico2.ToString(),
                    consultaExterna.diagnostico2cie.ToString(),
                    consultaExterna.diagnostico2pre.ToString(),
                    consultaExterna.diagnostico2def.ToString(),
                    consultaExterna.diagnostico3.ToString(),
                    consultaExterna.diagnostico3cie.ToString(),
                    consultaExterna.diagnostico3def.ToString(),
                    consultaExterna.diagnostico3pre.ToString(),
                    consultaExterna.diagnostico4.ToString(),
                    consultaExterna.diagnostico4cie.ToString(),
                    consultaExterna.diagnostico4def.ToString(),
                    consultaExterna.diagnostico4pre.ToString(),
                    consultaExterna.planesTratamiento.ToString(),
                    consultaExterna.evolucion.ToString(),
                    consultaExterna.prescripciones.ToString(),
                    Convert.ToString(dtp_fechaAltaEmerencia.Value),
                    Convert.ToString(DateTime.Now.ToString("hh:mm")),
                    consultaExterna.dr.ToString(),
                    Sesion.codMedico.ToString(),
                    logo,
                    empresa,
                    sign.Ocular,
                    sign.Verbal,
                    sign.Motora,
                    sign.Reaccion_Iz,
                    sign.Reaccion_Der,
                    sign.S_Oxigeno,
                    sign.Glisemia_Capilar,
                    tot1
                    });

                frmReportes x = new frmReportes(1, "ConsultaExterna", Ds);
                x.Show();
            }
            else
                MessageBox.Show("Algo ocurrio al generar el reporte.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnImprimir1_Click(object sender, EventArgs e)
        {
            imprimir();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ATENCIONES validaAtencion = NegAtenciones.RecuperarAtencionID(Convert.ToInt64(txtatecodigo.Text.Trim()));
            if (validaAtencion.ESC_CODIGO == 1)
            {
                HabilitarBotones(false, true, false, false, false, false, false, false, false, false, false);
                P_Central.Enabled = true;
                nuevaConsulta = false;
            }
            else
            {
                HabilitarBotones(false, false, false, true, true, true, true, false, false, false, false);
                MessageBox.Show("Paciente ha sido dado de alta.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Valida())
            {
                if (GuardaFormulario())
                {
                    MessageBox.Show("Información Guardada Con Exito", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    P_Central.Enabled = false;
                    ATENCIONES validaAtencion = NegAtenciones.RecuperarAtencionID(Convert.ToInt64(txtatecodigo.Text.Trim()));
                    if (validaAtencion.ESC_CODIGO == 1)
                        HabilitarBotones(false, false, true, true, true, true, true, true, false, true, false);
                    else
                        HabilitarBotones(false, false, false, true, true, true, false, false, false, false, false);
                }
                else
                    MessageBox.Show("Información No Se Guardo Comuniquese Con Sistemas", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Datos incompletos.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCertificado1_Click(object sender, EventArgs e)
        {
            try
            {
                //frm_CertificadoIESS x = new frm_CertificadoIESS(Convert.ToInt32(txtatecodigo.Text.Trim()), Convert.ToInt32(lblHistoria.Text.Trim()));
                //frm_Certificados x = new frm_Certificados();
                //if (x.abre)
                frm_Certificados x = new frm_Certificados(Convert.ToInt32(txtatecodigo.Text.Trim()), Convert.ToInt32(lblHistoria.Text.Trim()));
                x.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ud. No tiene acceso a generar certificados medicos ya que no esta registrado como un usuario medico", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show(ex.Message);
            }
        }

        private void btnReceta1_Click(object sender, EventArgs e)
        {
            if (NegParametros.ParametroReceta())
            {
                MEDICOS medico = NegMedicos.recuperarMedicoID_Usuario(Sesion.codUsuario);
                var recuperada = NegAtenciones.RecuepraAtencionNumeroAtencion(Convert.ToInt64(txtatecodigo.Text.Trim()));
                if (NegCertificadoMedico.ExisteRecetaMedico(recuperada.ATE_CODIGO, medico.MED_CODIGO))
                {
                    MessageBox.Show("Este medico genero una receta medica para este paciente. SI DESEA MODIFICAR LA RECETA, PRIMERO DEBE ANULARLA EN EL EXPLORADOR DE RECETAS MEDICAS", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                frmRecetaNew x = new frmRecetaNew(txtatecodigo.Text.Trim(), true);
                if (x.noCargar)
                    x.Show();
                else
                    x.Close();
            }
            else
            {
                His.Formulario.frm_RecetaMedica x = new His.Formulario.frm_RecetaMedica();
                x.Show();
            }
        }

        private void btnLaboratorio_Click(object sender, EventArgs e)
        {
            His.Formulario.frm_LaboratorioClinico laboratorio = new His.Formulario.frm_LaboratorioClinico(Convert.ToInt64(txtatecodigo.Text.Trim()), true);
            laboratorio.Show();
        }

        private void btnImagen_Click(object sender, EventArgs e)
        {
            frm_Imagen X = new frm_Imagen(Convert.ToInt32(txtatecodigo.Text.Trim()));
            X.mushuñan = mushuñan;
            X.ShowDialog();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (subsecuente != null)
            {
                if (!NegConsultaExterna.CerrarCxe("Form002", subsecuente.ate_codigo_subsecuente))
                {
                    MessageBox.Show("No se ha podido cerrar formulario. Consulte con sistemas", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    HabilitarBotones(false, false, true, true, true, true, true, false, false, true, false);
                }
                else
                {
                    MessageBox.Show("Formulario cerrado con exito", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HabilitarBotones(false, false, false, true, true, true, true, false, true, false, false);
                }
            }
            else
            {
                if (!NegConsultaExterna.CerrarCxe("Form002", Convert.ToInt64(txtatecodigo.Text.Trim())))
                {
                    MessageBox.Show("No se ha podido cerrar formulario. Consulte con sistemas", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    HabilitarBotones(false, false, true, true, true, true, true, false, false, true, false);
                }
                else
                {
                    MessageBox.Show("Formulario cerrado con exito", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HabilitarBotones(false, false, false, true, true, true, true, false, true, false, false);
                }

            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (Sesion.codDepartamento == 1 || Sesion.codDepartamento == 2 || Sesion.codDepartamento == 5)
            {
                if (!NegConsultaExterna.AbrirCxE(Convert.ToInt64(txtatecodigo.Text.Trim())))
                {
                    MessageBox.Show("No se ha podido abrir formulario. Consulte con sistemas", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                    MessageBox.Show("Formulario abierto con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HabilitarBotones(false, false, true, true, true, true, true, false, false, true, false);
            }
            else
            {
                MessageBox.Show("No se ha podido abrir formulario. No tiene los permisos necesarios", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            HabilitarBotones(true, false, false, false, false, false, false, false, false, false, false);
            P_Central.Visible = false;
            P_Datos.Visible = false;
            limpiarCampos();
        }

        private void txtTemperatura_Leave(object sender, EventArgs e)
        {
            if (txtTemperatura.Text != "")
                if (Convert.ToDecimal(txtTemperatura.Text) > 50)
                {
                    MessageBox.Show("Rango de temperatura incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtTemperatura.Text = "0";
                }
        }

        private void txtPresionArteria1_Leave(object sender, EventArgs e)
        {
            if (txtPresionArteria1.Text != "")
                if (Convert.ToDecimal(txtPresionArteria1.Text) > 300)
                {
                    MessageBox.Show("Rango de presion incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPresionArteria1.Text = "0";
                }
        }

        private void txtPresionArteria2_Leave(object sender, EventArgs e)
        {
            if (txtPresionArteria2.Text != "")
                if (Convert.ToDecimal(txtPresionArteria2.Text) > 300)
                {
                    MessageBox.Show("Rango de presion incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPresionArteria2.Text = "0";
                }
        }

        private void txtPulso_Leave(object sender, EventArgs e)
        {
            if (txtPulso.Text != "")
                if (Convert.ToDecimal(txtPulso.Text) > 300)
                {
                    MessageBox.Show("Rango de pulso incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPulso.Text = "0";
                }
        }

        private void txtFrecuenciaRespiratoria_Leave(object sender, EventArgs e)
        {
            if (txtFrecuenciaRespiratoria.Text != "")
                if (Convert.ToDecimal(txtFrecuenciaRespiratoria.Text) > 300)
                {
                    MessageBox.Show("Rango de frecuencia respiratoria incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtFrecuenciaRespiratoria.Text = "0";
                }
        }

        private void txtPeso_Leave(object sender, EventArgs e)
        {
            if (txtPeso.Text != "")
                if (Convert.ToDecimal(txtPeso.Text) > 700)
                {
                    MessageBox.Show("Rango de peso incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtPeso.Text = "0";
                }
        }

        private void txtTalla_Leave(object sender, EventArgs e)
        {
            if (txtTalla.Text != "")
                if (Convert.ToDecimal(txtTalla.Text) > 3)
                {
                    MessageBox.Show("Rango de talla incorrecto", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtTalla.Text = "0";
                }
        }

        private void txtPresionArteria1_TextChanged(object sender, EventArgs e)
        {
            if (txtPresionArteria1.Text == "" || !NegUtilitarios.ValidaPrecion1(Convert.ToInt16(txtPresionArteria1.Text)))
            {
                txtPresionArteria1.Text = "0";
            }
        }

        private void txtPresionArteria2_TextChanged(object sender, EventArgs e)
        {
            if (txtPresionArteria2.Text == "" || !NegUtilitarios.ValidaPrecion2(Convert.ToDouble(txtPresionArteria2.Text)))
            {
                txtPresionArteria2.Text = "0";
            }
        }
        MEDICOS medicoTratante = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (mushuñan)
            {
                Int32 ingreso = NegTipoIngreso.RecuperarporAtencion(Convert.ToInt64(txtatecodigo.Text));
                switch (ingreso)
                {
                    case 12:
                        List<MEDICOS> medicos = NegMedicos.listaMedicos();
                        medicos = NegMedicos.listaMedicosIncTipoMedico();
                        His.Formulario.frm_AyudaMedicos ayuda = new His.Formulario.frm_AyudaMedicos(medicos, "MEDICOS", "CODIGO");
                        ayuda.ShowDialog();

                        if (ayuda.campoPadre.Text != string.Empty)
                        {
                            medicoTratante = NegMedicos.RecuperaMedicoId(Convert.ToInt32(ayuda.campoPadre.Text.ToString()));
                            txt_profesionalEmergencia.Text = "";
                            txt_profesionalEmergencia.Text = medicoTratante.MED_APELLIDO_PATERNO.Trim() + " " + medicoTratante.MED_APELLIDO_MATERNO.Trim() + " " + medicoTratante.MED_NOMBRE1.Trim() + " " + medicoTratante.MED_NOMBRE2.Trim();
                        }
                        break;
                    case 10:
                        List<MEDICOS> medicos1 = NegMedicos.listaMedicos();
                        medicos1 = NegMedicos.listaMedicosIncTipoMedico();
                        His.Formulario.frm_AyudaMedicos ayuda1 = new His.Formulario.frm_AyudaMedicos(medicos1, "MEDICOS", "CODIGO");
                        ayuda1.ShowDialog();

                        if (ayuda1.campoPadre.Text != string.Empty)
                        {
                            medicoTratante = NegMedicos.RecuperaMedicoId(Convert.ToInt32(ayuda1.campoPadre.Text.ToString()));
                            txt_profesionalEmergencia.Text = "";
                            txt_profesionalEmergencia.Text = medicoTratante.MED_APELLIDO_PATERNO.Trim() + " " + medicoTratante.MED_APELLIDO_MATERNO.Trim() + " " + medicoTratante.MED_NOMBRE1.Trim() + " " + medicoTratante.MED_NOMBRE2.Trim();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnSubsecuente_Click(object sender, EventArgs e)
        {
            //Emergencia.frm_AyudaPacientes ayuda = new Emergencia.frm_AyudaPacientes(true, true); // Cambio para consulta subsecuente
            //ayuda.ShowDialog();
            //if (ayuda.ateCodigo != "")
            //{
            //    frm_Evolucion frm = new frm_Evolucion(true, Convert.ToInt64(ayuda.ateCodigo));
            //    frm.ShowDialog();
            //}
            if (subsecuente != null)
            {
                frm_Evolucion frm = new frm_Evolucion(true, subsecuente.ate_codigo_subsecuente);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No se generado una ATENCIÓN SUBSECUENTE para este paciente en la admisión", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (txtatecodigo.Text != "")
            {
                PACIENTES paciente = NegPacientes.recuperarPacientePorAtencion(Convert.ToInt32(txtatecodigo.Text));
                His.Admision.frm_ExploradorFormularios frm = new Admision.frm_ExploradorFormularios();
                frm.FiltroHC = Convert.ToInt64(paciente.PAC_HISTORIA_CLINICA);
                frm._ayudaSubsecuentes = true;
                frm.ShowDialog();
            }
        }
    }
}
