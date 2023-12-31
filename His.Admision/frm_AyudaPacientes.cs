﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using His.Entidades;
using His.Negocio;
using His.Parametros;
using His.General;
using Recursos;
using Infragistics.Win.UltraWinGrid;

namespace His.Admision
{
    public partial class frm_AyudaPacientes : Form
    {
        List<PACIENTES> consultaPacientes = new List<PACIENTES>();
        public TextBox campoPadre = new TextBox();
        public TextBox campoId = new TextBox();
        public string columnabuscada = "HCL";
        public bool inicio = true;
        private Int16 tipoIngreso;
        private bool _busca = false;
        public bool mushuñan = false;
        public bool preIngreso = false;
        public frm_AyudaPacientes()
        {
            InitializeComponent();
            btnActualizar.Appearance.Image = Archivo.ButtonRefresh;
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(50, "50"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(100, "100"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(1000, "1000"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(10000, "10000"));
            cb_numFilas.DisplayMember = "Value";
            cb_numFilas.ValueMember = "Key";
            cb_numFilas.SelectedIndex = 0;
            inicio = false;
            tipoIngreso = 0;

        }

        public frm_AyudaPacientes(bool busca)
        {
            InitializeComponent();
            _busca = busca;
            btnActualizar.Appearance.Image = Archivo.ButtonRefresh;
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(50, "50"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(100, "100"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(1000, "1000"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(10000, "10000"));
            cb_numFilas.DisplayMember = "Value";
            cb_numFilas.ValueMember = "Key";
            cb_numFilas.SelectedIndex = 0;
            inicio = false;
            BuscaTablaJIRE();
        }

        public frm_AyudaPacientes(string _preIngreso)
        {
            InitializeComponent();
            preIngreso = true;
            grid.DataSource = NegPacientes.BuscaPacienteEmergencia();
        }

        //public void BuscarEmergencia()
        //{
        //    grid.DataSource = NegPacientes.BuscaPacienteEmergencia();
        //}

        public void BuscaTablaJIRE()
        {
            if (txt_busqHist.Text != "" || txt_busqNom.Text != "" || txt_busqCi.Text != "")
                grid.DataSource = NegPacientes.BuscaPacienteJireParametro(txt_busqHist.Text, txt_busqNom.Text, txt_busqCi.Text);
            else
                grid.DataSource = NegPacientes.BuscaPacienteJire();
        }

        public frm_AyudaPacientes(Int16 ParTipoIngreso)
        {
            InitializeComponent();
            btnActualizar.Appearance.Image = Archivo.ButtonRefresh;
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(50, "50"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(100, "100"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(1000, "1000"));
            cb_numFilas.Items.Add(new KeyValuePair<int, string>(10000, "10000"));
            cb_numFilas.DisplayMember = "Value";
            cb_numFilas.ValueMember = "Key";
            cb_numFilas.SelectedIndex = 0;
            inicio = false;
            tipoIngreso = ParTipoIngreso;

        }

        private void Buscar()
        {
            try
            {
                string id = txt_busqCi.Text.ToString();
                string historia = txt_busqHist.Text.ToString();
                string nombre = txt_busqNom.Text.ToString();
                int cantidad = 100;

                if (cb_numFilas.SelectedItem != null)
                {
                    KeyValuePair<int, string> sitem = (KeyValuePair<int, string>)cb_numFilas.SelectedItem;
                    cantidad = sitem.Key;
                }

                //if(tipoIngreso ==0)
                consultaPacientes = NegPacientes.RecuperarPacientesLista(id, historia, nombre, cantidad);
                //else
                //    consultaPacientes = NegPacientes.RecuperarPacientesLista(id, historia, nombre, cantidad,tipoIngreso);

                grid.DataSource = consultaPacientes.Select(
                    p => new
                    {
                        HCL = p.PAC_HISTORIA_CLINICA,
                        NOMBRE = p.PAC_APELLIDO_PATERNO + " " + p.PAC_APELLIDO_MATERNO + " " + p.PAC_NOMBRE1 + " " + p.PAC_NOMBRE2,
                        ID = p.PAC_IDENTIFICACION
                    }
                    ).OrderBy(p => p.NOMBRE).Distinct().ToList();
                grid.DisplayLayout.Bands[0].Columns["NOMBRE"].Width = 350;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void cb_numFilas_SelectedValueChanged(object sender, EventArgs e)
        {
            if (inicio == false)
                Buscar();

        }

        private void txt_busqNom_AfterExitEditMode(object sender, EventArgs e)
        {

        }

        private void txt_busqHist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(GeneralPAR.TeclaTabular) || e.KeyChar == (char)(Keys.Tab))
            {
                e.Handled = true;
                SendKeys.SendWait("{TAB}");
                if (_busca)
                    BuscaTablaJIRE();
                else
                    Buscar();
            }
        }

        private void txt_busqNom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(GeneralPAR.TeclaTabular) || e.KeyChar == (char)(Keys.Tab))
            {
                e.Handled = true;
                SendKeys.SendWait("{TAB}");
                if (_busca)
                    BuscaTablaJIRE();
                else
                    Buscar();
            }
        }

        private void txt_busqCi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(GeneralPAR.TeclaTabular) || e.KeyChar == (char)(Keys.Tab))
            {
                e.Handled = true;
                SendKeys.SendWait("{TAB}");
                if (_busca)
                    BuscaTablaJIRE();
                else
                    Buscar();
            }
        }

        private void grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void grid_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void enviarCodigo()
        {
            try
            {
                campoPadre.Tag = true;
                campoPadre.Text = grid.ActiveRow.Cells[columnabuscada].Value.ToString();
                campoId.Text = grid.ActiveRow.Cells["ID"].Value.ToString();

                frmExploradorControlHC.hcayuda = campoPadre.Text;// Envia el HC del paciente al formulario de Control de HC CheckList
                frmEmergenciaProcedimientos.id = grid.ActiveRow.Cells["ID"].Value.ToString();
                this.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void grid_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void frm_AyudaPacientes_Load(object sender, EventArgs e)
        {
            if (!preIngreso)
            {
                //cargo los ultimos 25 pacientes ingresados
                //if(tipoIngreso ==0)
                consultaPacientes = NegPacientes.RecuperarPacientesLista("", "", "", 50);
                //else
                //    consultaPacientes = NegPacientes.RecuperarPacientesLista("", "", "", 50,tipoIngreso );

                grid.DataSource = consultaPacientes.Select(
                    p => new
                    {
                        HCL = p.PAC_HISTORIA_CLINICA,
                        NOMBRE = p.PAC_APELLIDO_PATERNO + " " + p.PAC_APELLIDO_MATERNO + " " + p.PAC_NOMBRE1 + " " + p.PAC_NOMBRE2,
                        ID = p.PAC_IDENTIFICACION
                    }
                    ).Distinct().ToList();
                grid.DisplayLayout.Bands[0].Columns["NOMBRE"].Width = 350;

                //pongo el foco en el campo
                txt_busqNom.Focus();
            }
            else
            {
                grid.DataSource = NegPacientes.BuscaPacienteEmergencia();
                grid.DisplayLayout.Bands[0].Columns["NOMBRE"].Width = 350;
                //pongo el foco en el campo
                txt_busqNom.Focus();
            }
        }

        private void ultraGrid1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    enviarCodigo();
                }
                else if (e.KeyCode == Keys.End)
                {
                    grid.ActiveCell = grid.Rows[grid.Rows.Count - 1].Cells[grid.ActiveCell.Column.Index];
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Home)
                {
                    grid.ActiveCell = grid.Rows[0].Cells[grid.ActiveCell.Column.Index];
                    e.Handled = true;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ultraGrid1_DoubleClickRow(object sender, Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs e)
        {
            enviarCodigo();
        }

        private void grid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            grid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
            grid.DisplayLayout.Override.RowSizing = RowSizing.AutoFixed;
            grid.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;
        }

        private void frm_AyudaPacientes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void grid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            if (preIngreso)
            {
                UltraGridBand band = this.grid.DisplayLayout.Bands[0];
                foreach (UltraGridRow item in band.GetRowEnumerator(GridRowType.DataRow))
                {
                    ATENCIONES_REINGRESO atRein = null;
                    atRein = NegAtenciones.buscaReIngresoXate_codigo(Convert.ToInt64(item.Cells["ATE_CODIGO"].Value));
                    if (atRein != null)
                    {
                        item.Appearance.BackColor = Color.FromArgb(255, 159, 64);//naranja
                    }

                }
            }
        }
    }
}
