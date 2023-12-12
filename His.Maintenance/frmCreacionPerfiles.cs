using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using His.Negocio;
using His.Entidades;
using Infragistics.Win.UltraWinGrid;
using Core.Entidades;

namespace His.Maintenance
{
    public partial class frmCreacionPerfiles : Form
    {
        PERFILES perfil = new PERFILES();
        PERFILES_ACCESOS peracc = new PERFILES_ACCESOS();
        MODULO modulo = new MODULO();
        bool editarAcceso = false;
        bool editarPerfil = false;
        bool editarModulo = false;
        bool editarPerfilSic = false;
        bool editarModuloSic = false;
        bool editarPerfilCg = false;
        bool editarModuloCg = false;
        Int32 per_codigo;
        Int16 id_perfil;
        Int32 id_modulo;
        Int32 id_PerfilSic;
        Int32 id_PerfilCg;
        Int32 id_moduloSic;
        Int32 id_moduloCg;
        bool nuevoPerfil = false;
        bool nuevoModulo = false;
        bool nuevoPerfilSic = false;
        bool nuevoModuloSic = false;
        bool nuevoPerfilCg = false;
        bool nuevoModuloCG = false;
        public frmCreacionPerfiles()
        {
            InitializeComponent();
            cargarCombos();
            listarPerfiles();
            HabilitarNuevo(false, false, false, false);
            habilitarCampos(false, false);
            CargaPerfilesSic();
            CargaPerfilesCg();
        }

        private void frmCreacionPerfiles_Load(object sender, EventArgs e)
        {

        }
        public void cargarCombos()
        {
            cmbModulo.DataSource = NegModulo.RecuperaModulos();
            cmbModulo.DisplayMember = "DESCRIPCION";
            cmbModulo.ValueMember = "ID_MODULO";
            cmbModulo.SelectedIndex = -1;

            cmbAcceso.DataSource = NegAccesoOpciones.ListaAccesoOpciones();
            cmbAcceso.DisplayMember = "DESCRIPCION".Trim();
            cmbAcceso.ValueMember = "ID_ACCESO";
            cmbAcceso.SelectedIndex = -1;
        }
        public void listarPerfiles()
        {
            dsProcedimiento1 = NegMaintenance.cargaPerfilesAcceso();
            ultraGridPerfil.DataSource = dsProcedimiento1.Perfiles;
        }

        private void nuevoAccesoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelarHis();
            UltraGridRow Fila = ultraGridModulo.ActiveRow;
            if (ultraGridModulo.Selected.Rows.Count == 1)
            {
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + Fila.Cells["MODULO"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    editarModulo = true;
                    //ListarProductos();
                    HabilitarBotones(false, false, true);
                    HabilitarNuevo(true, true, false, false);
                    habilitarCampos(true, false);
                    txtPerfil.Text = Fila.Cells["MODULO"].Value.ToString();
                    id_modulo = Convert.ToInt16(Fila.Cells["ID"].Value.ToString());
                    ultrgHis.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un modulo para continuar...", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        public void HabilitarBotonesSic(bool nuevo, bool editar, bool cancelar)
        {
            btnNuevoSic.Enabled = nuevo;
            btnEditarSic.Enabled = editar;
            btnCancelarSic.Enabled = cancelar;
        }
        public void HabilitarBotonesCg(bool nuevo, bool editar, bool cancelar)
        {
            btnNuevoCg.Enabled = nuevo;
            btnEditarCg.Enabled = editar;
            btnCancelarCg.Enabled = cancelar;
        }
        public void HabilitarBotones(bool nuevo, bool editar, bool cancelar)
        {
            btnNuevo.Enabled = nuevo;
            btnEditar.Enabled = editar;
            btnCancelar.Enabled = cancelar;
        }
        public void HabilitarNuevo(bool nuevoProce, bool eliminarProce, bool añadir, bool eliminarProdu)
        {
            btnGuardarPer.Enabled = nuevoProce;
            btnEliminar.Enabled = eliminarProdu;
            btnEliminarPer.Enabled = eliminarProce;
            btnAñadir.Enabled = añadir;
        }
        public void habilitarCampos(bool _txtperfil, bool _cmbAcceso)
        {
            txtPerfil.Enabled = _txtperfil;
            cmbAcceso.Enabled = _cmbAcceso;
            cmbModulo.Enabled = _cmbAcceso;
        }
        public void LimpiarCampos()
        {
            txtPerfil.Text = "";
            cmbAcceso.SelectedIndex = -1;
            HabilitarBotones(true, true, true);
        }
        public void crearPerfil()
        {
            perfil = new PERFILES();
            id_perfil = NegMaintenance.maxPerfil();
            perfil.ID_PERFIL = id_perfil;
            perfil.DESCRIPCION = txtPerfil.Text;
            if (NegMaintenance.creaPerfil(perfil))
            {
                MessageBox.Show("Datos guardados con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPerfil.Text = "";
            }
            else
            {
                MessageBox.Show("Algo ocurrio al guardar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                HabilitarNuevo(true, false, false, false);
                return;
            }
        }
        public void editaPerfil()
        {
            if (NegMaintenance.editarPerfil(id_perfil, txtPerfil.Text))
            {
                MessageBox.Show("Datos editados con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editarPerfil = false;
            }
            else
            {
                MessageBox.Show("Algo ocurrio al editar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public void agregarAcceso()
        {
            PERFILES perfil = new PERFILES();
            perfil = NegPerfil.RecuperaPerfil(id_perfil);
            ACCESO_OPCIONES acceso = new ACCESO_OPCIONES();
            acceso = NegAccesoOpciones.RecuperaAccesosOpciones((Int32)cmbAcceso.Value);

            peracc = new PERFILES_ACCESOS();
            peracc.PERFILESReference.EntityKey = perfil.EntityKey;
            peracc.ID_PERFIL = id_perfil;
            peracc.ACCESO_OPCIONESReference.EntityKey = acceso.EntityKey;
            peracc.ID_ACCESO = (Int32)cmbAcceso.Value;
            if (NegMaintenance.agregarAcceso(peracc))
            {
                MessageBox.Show("acceso añadido con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Algo ocurrio al guardar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        public void eliminarAcceso(Int64 id_perfil, Int64 id_acceso)
        {
            if (NegMaintenance.eliminarAcceso(id_perfil, id_acceso))
            {
                MessageBox.Show("acceso eliminado con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HabilitarBotones(true, true, false);
                HabilitarNuevo(false, false, false, false);
                LimpiarCampos();
                listarPerfiles();
            }
            else
            {
                MessageBox.Show("Algo ocurrio al eliminar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #region Eliminar
        private void ultraGridProcedimiento_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point mousepoint = new Point(e.X, e.Y);
                contextMenuStrip1.Show(ultraGridPerfil, mousepoint);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (!btnNuevo.Enabled)
            {
                if (MessageBox.Show("¿Está seguro de cancelar?", "HIS3000", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    LimpiarCampos();
                    HabilitarBotones(true, true, false);
                    HabilitarNuevo(false, false, false, false);
                    habilitarCampos(false, false);
                    editarPerfil = false;
                }
            }
            else
            {
                LimpiarCampos();
                HabilitarBotones(true, true, false);
                HabilitarNuevo(false, false, false, false);
                habilitarCampos(false, false);
                editarPerfil = false;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (ultraGridPerfil.Selected.Rows.Count == 1)
            {
                UltraGridRow fila = ultraGridPerfil.ActiveRow;
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + fila.Cells["Perfil"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HabilitarNuevo(true, true, false, false);
                    editarPerfil = true;
                    HabilitarBotones(false, false, true);
                    habilitarCampos(true, false);
                    id_perfil = Convert.ToInt16(fila.Cells["Codigo"].Value.ToString());
                    txtPerfil.Text = fila.Cells["Perfil"].Value.ToString();
                    txtPerfil.Focus();
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un Perfil.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            editarPerfil = false;
            HabilitarBotones(false, false, true);
            HabilitarNuevo(true, false, false, false);
            habilitarCampos(true, false);
        }

        private void ultraGridProcedimiento_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridPerfil.DisplayLayout.Bands[0];

            //ultraGridProcedimiento.DisplayLayout.ViewStyle = ViewStyle.SingleBand;
            //grid.DisplayLayout.Override.Allow

            ultraGridPerfil.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            //grid.DisplayLayout.GroupByBox.Prompt = "Arrastrar la columna que desea agrupar";
            ultraGridPerfil.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridPerfil.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridPerfil.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridPerfil.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridPerfil.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;

            //Caracteristicas de Filtro en la grilla
            ultraGridPerfil.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            ultraGridPerfil.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            ultraGridPerfil.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.WithOperand;
            ultraGridPerfil.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.Row;
            ultraGridPerfil.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FilterRow;
            //
            ultraGridPerfil.DisplayLayout.UseFixedHeaders = true;

            //Dimension los registros
            ultraGridPerfil.DisplayLayout.Bands[0].Columns[0].Width = 60;
            ultraGridPerfil.DisplayLayout.Bands[0].Columns["Perfil"].Width = 700;
        }

        private void btnGuardarPer_Click(object sender, EventArgs e)
        {
            if (editarPerfil)
            {
                editaPerfil();
                LimpiarCampos();
                listarPerfiles();
                HabilitarNuevo(false, false, false, false);
                habilitarCampos(false, false);
                HabilitarBotones(true, false, false);
            }
            else
            {
                crearPerfil();
                HabilitarNuevo(false, false, true, false);
                habilitarCampos(false, true);
            }
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            PERFILES_ACCESOS perdilacceso = NegMaintenance.buscaPerfilesacceso(id_perfil, Convert.ToInt64(cmbAcceso.Value));
            if (perdilacceso == null)
            {
                agregarAcceso();
                HabilitarNuevo(false, false, true, false);
                listarPerfiles();
            }
            else
                MessageBox.Show("el acceso ya fue agregado al perfil", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            eliminarAcceso(id_perfil, Convert.ToInt64(cmbAcceso.Value));
        }

        private void ultraGridProcedimiento_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridPerfil.Rows)
            {
                if (item.Cells["Codigo"].Value.ToString() == e.Cell.Row.Cells["Codigo"].Value.ToString())
                {
                    try
                    {
                        ultraGridModulo.DataSource = NegModulo.ListaModulo();
                        //perfil = NegMaintenance.buscaPerfiles(Convert.ToInt16(e.Cell.Row.Cells["Codigo"].Value.ToString()));
                        //cmbAcceso.Value = e.Cell.Row.Cells["ID_Acceso"].Value.ToString();
                        //txtPerfil.Text = perfil.DESCRIPCION.ToString();
                        id_perfil = Convert.ToInt16(e.Cell.Row.Cells["Codigo"].Value.ToString());
                        ultraGridAccesos.DataSource = null;
                        //HabilitarBotones(false, false, true);
                        //HabilitarNuevo(false, false, false, true);
                        //break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Seleccione un Acceso", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //throw;
                    }
                }
            }
        }
        private void btnEliminarPer_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult resultado;

                resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resultado == DialogResult.Yes)
                {
                    List<PERFILES_ACCESOS> acOriginal = NegPerfilesAcceso.ListaPerfilesAccesos().Where(p => p.ID_PERFIL == id_perfil).ToList().ClonarEntidad();
                    NegPerfilesAcceso.EliminaListaPerfilesAccesos(acOriginal, acOriginal);
                    List<USUARIOS_PERFILES> upOriginal = NegUsuarios.ListaUsuarioPerfiles().Where(p => p.ID_PERFIL == id_perfil).ToList().ClonarEntidad();
                    NegUsuarios.EliminaUsuarioPerfiles(upOriginal, upOriginal);
                    if (NegMaintenance.eliminarPerfil(id_perfil))
                    {
                        MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        HabilitarBotones(true, true, false);
                        HabilitarNuevo(false, false, false, false);
                        habilitarCampos(false, false);
                    }
                    else
                        MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    listarPerfiles();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void cmbModulo_ValueChanged(object sender, EventArgs e)
        {
            if (cmbModulo.SelectedIndex > 0)
            {
                cmbAcceso.DataSource = NegMaintenance.listaAccesoOpciones(Convert.ToInt64(cmbModulo.Value));
                cmbAcceso.DisplayMember = "DESCRIPCION".Trim();
                cmbAcceso.ValueMember = "ID_ACCESO";
                cmbAcceso.SelectedIndex = -1;
            }
        }
        private void ultraGridAccesos_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                bool estado;
                if (bool.TryParse(e.Cell.Value.ToString(), out estado))
                {
                    foreach (var item in ultraGridAccesos.Rows)
                    {
                        if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                        {
                            if (!Convert.ToBoolean(item.Cells["TIENE_ACCESO"].Value.ToString()))
                            {
                                PERFILES perfil = new PERFILES();
                                perfil = NegPerfil.RecuperaPerfil(id_perfil);
                                ACCESO_OPCIONES acceso = new ACCESO_OPCIONES();
                                acceso = NegAccesoOpciones.RecuperaAccesosOpciones(Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString()));

                                peracc = new PERFILES_ACCESOS();
                                peracc.PERFILESReference.EntityKey = perfil.EntityKey;
                                peracc.ID_PERFIL = id_perfil;
                                peracc.ACCESO_OPCIONESReference.EntityKey = acceso.EntityKey;
                                peracc.ID_ACCESO = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                                if (!NegMaintenance.agregarAcceso(peracc))
                                {
                                    MessageBox.Show("No se ha podido crear el acceso ", "His-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                ultraGridAccesos.DataSource = NegAccesoOpciones.ListarAccesoOpcionesXmodulo(id_modulo, id_perfil);
                                listarPerfiles();
                            }
                            else
                            {
                                List<PERFILES_ACCESOS> acOriginal = NegPerfilesAcceso.ListaPerfilesAccesos().Where(p => p.ID_PERFIL == id_perfil && p.ACCESO_OPCIONES.ID_ACCESO == Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString())).ToList().ClonarEntidad();
                                NegPerfilesAcceso.EliminaListaPerfilesAccesos(acOriginal, acOriginal);
                                ultraGridAccesos.DataSource = NegAccesoOpciones.ListarAccesoOpcionesXmodulo(id_modulo, id_perfil);
                                listarPerfiles();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }

        }
        #endregion

        #region Perfiles His-300

        private void nuevoPerfilToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ultrgHis.Enabled = true;
            editarPerfil = false;
            nuevoPerfil = true;
            HabilitarBotones(false, false, true);
            HabilitarNuevo(true, false, false, false);
            habilitarCampos(true, false);
        }

        private void nuevoModuloToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ultrgHis.Enabled = true;
            nuevoModulo = true;
            HabilitarBotones(false, false, true);
            HabilitarNuevo(true, false, false, false);
            habilitarCampos(true, false);
        }
        private void btnGuardarPer_Click_1(object sender, EventArgs e)
        {
            if (nuevoPerfil)
            {
                crearPerfil();
                listarPerfiles();
                limpiarGrid();
            }
            else if (editarPerfil)
            {
                editaPerfil();
                listarPerfiles();
                limpiarGrid();
            }
            else if (nuevoModulo)
            {
                modulo = new MODULO();
                id_modulo = NegModulo.maxModulo();
                modulo.ID_MODULO = (short)id_modulo;
                modulo.DESCRIPCION = txtPerfil.Text;
                modulo.ESTADO = true;
                if (NegModulo.CrearModulo(modulo))
                {
                    MessageBox.Show("Datos guardados con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPerfil.Text = "";
                    nuevoModulo = false;
                    ultraGridModulo.DataSource = NegModulo.ListaModulo();
                    ultraGridAccesos.DataSource = null;
                }
                else
                {
                    MessageBox.Show("Algo ocurrio al guardar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (editarModulo)
            {
                if (NegModulo.EditarModulo(id_modulo, txtPerfil.Text))
                {
                    MessageBox.Show("Datos guardados con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPerfil.Text = "";
                    nuevoModulo = false;
                    ultraGridModulo.DataSource = NegModulo.ListaModulo();
                    ultraGridAccesos.DataSource = null;
                }
                else
                {
                    MessageBox.Show("Algo ocurrio al guardar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            CancelarHis();
        }
        private void ultraGridPerfil_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridPerfil.Rows)
            {
                if (item.Cells["Codigo"].Value.ToString() == e.Cell.Row.Cells["Codigo"].Value.ToString())
                {
                    try
                    {
                        ultraGridModulo.DataSource = NegModulo.ListaModulo();
                        //perfil = NegMaintenance.buscaPerfiles(Convert.ToInt16(e.Cell.Row.Cells["Codigo"].Value.ToString()));
                        //cmbAcceso.Value = e.Cell.Row.Cells["ID_Acceso"].Value.ToString();
                        //txtPerfil.Text = perfil.DESCRIPCION.ToString();
                        id_perfil = Convert.ToInt16(e.Cell.Row.Cells["Codigo"].Value.ToString());
                        ultraGridAccesos.DataSource = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Seleccione un Acceso", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //throw;
                    }
                }
            }
        }

        private void ultraGridModulo_DoubleClickCell_1(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridModulo.Rows)
            {
                if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                {
                    try
                    {
                        ultraGridAccesos.DataSource = NegAccesoOpciones.ListarAccesoOpcionesXmodulo(Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), id_perfil);
                        id_modulo = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //throw;
                    }
                }
            }
        }

        private void ultraGridAccesos_CellChange_1(object sender, CellEventArgs e)
        {
            try
            {
                bool estado;
                if (bool.TryParse(e.Cell.Value.ToString(), out estado))
                {
                    foreach (var item in ultraGridAccesos.Rows)
                    {
                        if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                        {
                            if (!Convert.ToBoolean(item.Cells["TIENE_ACCESO"].Value.ToString()))
                            {
                                PERFILES perfil = new PERFILES();
                                perfil = NegPerfil.RecuperaPerfil(id_perfil);
                                ACCESO_OPCIONES acceso = new ACCESO_OPCIONES();
                                acceso = NegAccesoOpciones.RecuperaAccesosOpciones(Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString()));

                                peracc = new PERFILES_ACCESOS();
                                peracc.PERFILESReference.EntityKey = perfil.EntityKey;
                                peracc.ID_PERFIL = id_perfil;
                                peracc.ACCESO_OPCIONESReference.EntityKey = acceso.EntityKey;
                                peracc.ID_ACCESO = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                                if (!NegMaintenance.agregarAcceso(peracc))
                                {
                                    MessageBox.Show("No se ha podido crear el acceso ", "His-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                                ultraGridAccesos.DataSource = NegAccesoOpciones.ListarAccesoOpcionesXmodulo(id_modulo, id_perfil);
                                listarPerfiles();
                            }
                            else
                            {
                                List<PERFILES_ACCESOS> acOriginal = NegPerfilesAcceso.ListaPerfilesAccesos().Where(p => p.ID_PERFIL == id_perfil && p.ACCESO_OPCIONES.ID_ACCESO == Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString())).ToList().ClonarEntidad();
                                NegPerfilesAcceso.EliminaListaPerfilesAccesos(acOriginal, acOriginal);
                                ultraGridAccesos.DataSource = NegAccesoOpciones.ListarAccesoOpcionesXmodulo(id_modulo, id_perfil);
                                listarPerfiles();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        private void btnEliminarPer_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado;
                if (editarModulo)
                {
                    resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        List<PERFILES_ACCESOS> acOriginal = NegPerfilesAcceso.ListaPerfilesAccesosXmodulo(id_modulo).ClonarEntidad();
                        NegPerfilesAcceso.EliminaListaPerfilesAccesos(acOriginal, acOriginal);

                        List<ACCESO_OPCIONES> accopc = NegAccesoOpciones.RecuperaAccesosOpcionesXmodulo(id_modulo).ClonarEntidad();
                        NegAccesoOpciones.EliminarAccesoOpciones1(accopc);
                        if (NegModulo.EliminarModulo(id_modulo))
                        {
                            MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            HabilitarBotones(true, true, true);
                            HabilitarNuevo(false, false, false, false);
                            habilitarCampos(false, false);
                            ultraGridAccesos.DataSource = null;
                        }
                        else
                            MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ultraGridModulo.DataSource = NegModulo.ListaModulo();
                    }
                }
                else if (editarPerfil)
                {
                    resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        List<PERFILES_ACCESOS> acOriginal = NegPerfilesAcceso.ListaPerfilesAccesos().Where(p => p.ID_PERFIL == id_perfil).ToList().ClonarEntidad();
                        NegPerfilesAcceso.EliminaListaPerfilesAccesos(acOriginal, acOriginal);
                        List<USUARIOS_PERFILES> upOriginal = NegUsuarios.ListaUsuarioPerfiles().Where(p => p.ID_PERFIL == id_perfil).ToList().ClonarEntidad();
                        NegUsuarios.EliminaUsuarioPerfiles(upOriginal, upOriginal);
                        if (NegMaintenance.eliminarPerfil(id_perfil))
                        {
                            MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            HabilitarBotones(true, true, true);
                            HabilitarNuevo(false, false, false, false);
                            habilitarCampos(false, false);
                            ultraGridAccesos.DataSource = null;
                            ultraGridModulo.DataSource = null;

                        }
                        else
                            MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        listarPerfiles();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void CancelarHis()
        {
            HabilitarBotones(true, true, true);
            txtPerfil.Text = "";
            ultrgHis.Enabled = false;
            nuevoModulo = false;
            editarModulo = false;
            nuevoPerfil = false;
            editarPerfil = false;
        }
        public void CancelarSic()
        {
            HabilitarBotonesSic(true, true, true);
            txtNuevoSic.Text = "";
            ultrgSic.Enabled = false;
            nuevoModuloSic = false;
            editarModuloSic = false;
            nuevoPerfilSic = false;
            editarPerfilSic = false;
        }
        public void CancelarCg()
        {
            HabilitarBotonesCg(true, true, true);
            txtNuevoCg.Text = "";
            ultrgCg.Enabled = false;
            nuevoModuloCG = false;
            editarModuloCg = false;
            nuevoPerfilCg = false;
            editarPerfilCg = false;
        }
        public void limpiarGrid()
        {
            ultraGridModulo.DataSource = null;
            ultraGridAccesos.DataSource = null;
        }
        public void limpiarGridSic()
        {
            ultraGridModuloSic.DataSource = null;
            ultraGridAccesosSic.DataSource = null;
        }
        public void limpiarGridCg()
        {
            ultraGridModuloCg.DataSource = null;
            ultraGridAccesosCg.DataSource = null;
        }
        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            CancelarHis();
        }
        private void ultraGridModulo_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point mousepoint = new Point(e.X, e.Y);
                contextMenuStrip1.Show(ultraGridModulo, mousepoint);
            }
        }

        #endregion
        #region Perfiles Sic-300
        public void CargaPerfilesSic()
        {
            ultraGridPerfilesSic.DataSource = NegUsuarios.PerfilesSic();
        }
        public void CargaModuloSic()
        {
            ultraGridModuloSic.DataSource = NegUsuarios.ModuloSic();
        }
        private void ultraGridPerfilesSic_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridPerfilesSic.Rows)
            {
                if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                {
                    try
                    {
                        ultraGridAccesosSic.DataSource = null;
                        ultraGridModuloSic.DataSource = NegUsuarios.ModuloSic();
                        id_PerfilSic = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //throw;
                    }
                }
            }
        }

        private void ultraGridModuloSic_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridModuloSic.Rows)
            {
                if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                {
                    try
                    {
                        ultraGridAccesosSic.DataSource = null;
                        ultraGridAccesosSic.DataSource = NegUsuarios.BuscaPerfilesSic(Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), id_PerfilSic);
                        id_moduloSic = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //throw;
                    }
                }
            }
        }
        private void nuevoPerfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoPerfilSic = true;
            nuevoModuloSic = false;
            lbNuevoSic.Text = "Nombre del Perfil*:";
            txtNuevoSic.Enabled = true;
            ultrgSic.Enabled = true;
            btnEliminarSic.Enabled = false;
        }

        private void nuevoAccesoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nuevoModuloSic = true;
            nuevoPerfilSic = false;
            lbNuevoSic.Text = "Nombre del Modulo*:";
            txtNuevoSic.Enabled = true;
            ultrgSic.Enabled = true;
            btnEliminarSic.Enabled = false;
        }

        private void btnNuevoSic_Click(object sender, EventArgs e)
        {
            if (txtNuevoSic.Text != null || txtNuevoSic.Text != "")
            {
                if (nuevoPerfilSic)
                {
                    crearPerfilSic();
                    CargaPerfilesSic();
                    limpiarGridSic();
                }
                else if (editarPerfilSic)
                {
                    editaPerfilSic();
                    CargaPerfilesSic();
                    limpiarGridSic();
                }
                else if (nuevoModuloSic)
                {
                    if (NegMaintenance.crearModuloSic(txtNuevoSic.Text))
                    {
                        MessageBox.Show("Datos guardados con exito.", "Sic3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtPerfil.Text = "";
                        nuevoModulo = false;
                        CargaModuloSic();
                        ultraGridAccesos.DataSource = null;
                    }
                    else
                    {
                        MessageBox.Show("Algo ocurrio al guardar.", "Sic3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (editarModuloSic)
                {
                    if (NegMaintenance.editarModuloSic(id_moduloSic, txtNuevoSic.Text))
                    {
                        MessageBox.Show("Datos guardados con exito.", "Sic3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNuevoSic.Text = "";
                        nuevoModuloSic = false;
                        CargaModuloSic();
                        ultraGridAccesosSic.DataSource = null;
                    }
                    else
                    {
                        MessageBox.Show("Algo ocurrio al guardar.", "Sic3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                CancelarSic();
            }
        }

        private void ultraGridAccesosSic_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                bool estado;
                if (bool.TryParse(e.Cell.Value.ToString(), out estado))
                {
                    foreach (var item in ultraGridAccesosSic.Rows)
                    {
                        if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                        {
                            if (!Convert.ToBoolean(item.Cells["TIENE_ACCESO"].Value.ToString()))
                            {

                                if (!NegUsuarios.CrearPerfilSic(id_PerfilSic, Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), "S", " "))
                                {
                                    //MessageBox.Show("No se ha podido crear el acceso ", "Sic-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                ultraGridAccesosSic.DataSource = NegUsuarios.BuscaPerfilesSic(id_moduloSic, id_PerfilSic);
                            }
                            else
                            {
                                if (!NegUsuarios.CrearPerfilSic(id_PerfilSic, Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), "N", " "))
                                {
                                    MessageBox.Show("No se ha podido crear el acceso ", "Sic-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                ultraGridAccesosSic.DataSource = NegUsuarios.BuscaPerfilesSic(id_moduloSic, id_PerfilSic);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        private void ultraGridModuloSic_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point mousepoint = new Point(e.X, e.Y);
                contextMenuStrip2.Show(ultraGridModuloSic, mousepoint);
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CancelarSic();
            UltraGridRow Fila = ultraGridModuloSic.ActiveRow;
            if (ultraGridModuloSic.Selected.Rows.Count == 1)
            {
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + Fila.Cells["MODULO"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    editarModuloSic = true;
                    txtNuevoSic.Enabled = true;
                    HabilitarBotonesSic(false, false, true);
                    txtNuevoSic.Text = Fila.Cells["MODULO"].Value.ToString();
                    id_moduloSic = Convert.ToInt16(Fila.Cells["ID"].Value.ToString());
                    ultrgSic.Enabled = true;
                    btnEliminarSic.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un modulo para continuar...", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void btnCancelarSic_Click(object sender, EventArgs e)
        {
            CancelarSic();
        }
        private void btnEditarSic_Click(object sender, EventArgs e)
        {
            CancelarSic();
            if (ultraGridPerfilesSic.Selected.Rows.Count == 1)
            {
                UltraGridRow fila = ultraGridPerfilesSic.ActiveRow;
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + fila.Cells["Perfil"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    editarPerfilSic = true;
                    HabilitarBotonesSic(false, false, true);
                    id_PerfilSic = Convert.ToInt16(fila.Cells["ID"].Value.ToString());
                    txtNuevoSic.Text = fila.Cells["Perfil"].Value.ToString();
                    txtNuevoSic.Enabled = true;
                    txtNuevoSic.Focus();
                    ultrgSic.Enabled = true;
                    btnEliminarSic.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un Perfil.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        public void crearPerfilSic()
        {
            if (NegMaintenance.crearPerfilSic(txtNuevoSic.Text))
            {
                MessageBox.Show("Datos guardados con exito.", "Sic3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNuevoSic.Text = "";
            }
            else
            {
                MessageBox.Show("Algo ocurrio al guardar.", "Sic3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CancelarSic();
                return;
            }
        }
        public void editaPerfilSic()
        {
            if (NegMaintenance.editarPerfilSic(id_PerfilSic, txtNuevoSic.Text))
            {
                MessageBox.Show("Datos editados con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editarPerfil = false;
            }
            else
            {
                MessageBox.Show("Algo ocurrio al editar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void btnEliminarSic_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado;
                if (editarModuloSic)
                {
                    resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        if (NegMaintenance.eliminarModuloSic(id_moduloSic))
                        {
                            MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CancelarSic();
                            ultraGridAccesosSic.DataSource = null;
                        }
                        else
                            MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CargaModuloSic();
                        CancelarSic();
                    }
                }
                else if (editarPerfilSic)
                {
                    resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                       
                        if (NegMaintenance.eliminarPerfilSic(id_PerfilSic))
                        {
                            MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HabilitarBotonesSic(true, true, true);
                            CancelarSic();
                            ultraGridAccesosSic.DataSource = null;
                            ultraGridModuloSic.DataSource = null;

                        }
                        else
                            MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CargaPerfilesSic();
                        CancelarSic();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
        #region Perfiles Cg-3000
        public void CargaPerfilesCg()
        {
            ultraGridPerfilesCg.DataSource = NegUsuarios.PerfilesCg();
        }
        public void CargaModuloCg()
        {
            ultraGridModuloCg.DataSource = NegUsuarios.ModuloCG();
        }
        private void ultraGridPerfilesCg_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridPerfilesCg.Rows)
            {
                if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                {
                    try
                    {
                        ultraGridAccesosCg.DataSource = null;
                        ultraGridModuloCg.DataSource = NegUsuarios.ModuloCG();
                        id_PerfilCg = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //throw;
                    }
                }
            }
        }

        private void ultraGridModuloCg_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            foreach (UltraGridRow item in ultraGridModuloCg.Rows)
            {
                if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                {
                    try
                    {
                        ultraGridAccesosCg.DataSource = null;
                        ultraGridAccesosCg.DataSource = NegUsuarios.BuscaPerfilesCg(Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), id_PerfilCg);
                        id_moduloCg = Convert.ToInt32(e.Cell.Row.Cells["ID"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        //throw;
                    }
                }
            }
        }

        private void ultraGridAccesosCg_ClickCell(object sender, ClickCellEventArgs e)
        {
            try
            {
                bool estado;
                if (bool.TryParse(e.Cell.Value.ToString(), out estado))
                {
                    foreach (var item in ultraGridAccesosCg.Rows)
                    {
                        if (item.Cells["ID"].Value.ToString() == e.Cell.Row.Cells["ID"].Value.ToString())
                        {
                            if (!Convert.ToBoolean(item.Cells["TIENE_ACCESO"].Value.ToString()))
                            {

                                if (!NegUsuarios.CrearPerfilCg(id_PerfilCg, Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), "S", " "))
                                {
                                    MessageBox.Show("No se ha podido crear el acceso ", "Cg-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                ultraGridAccesosCg.DataSource = NegUsuarios.BuscaPerfilesCg(id_moduloCg, id_PerfilCg);
                            }
                            else
                            {
                                if (!NegUsuarios.CrearPerfilCg(id_PerfilCg, Convert.ToInt64(e.Cell.Row.Cells["ID"].Value.ToString()), "N", " "))
                                {
                                    MessageBox.Show("No se ha podido crear el acceso ", "Cg-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                ultraGridAccesosCg.DataSource = NegUsuarios.BuscaPerfilesCg(id_moduloCg, id_PerfilCg);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
        private void btnNuevoCG_Click(object sender, EventArgs e)
        {
            if (txtNuevoCg.Text != null || txtNuevoCg.Text != "")
            {
                if (nuevoPerfilCg)
                {
                    crearPerfilCg();
                    CargaPerfilesCg();
                    limpiarGridCg();
                }
                else if (editarPerfilCg)
                {
                    editaPerfilCg();
                    CargaPerfilesCg();
                    limpiarGridCg();
                }
                 else if (nuevoModuloCG)
                {
                    if (!NegUsuarios.crearModuloCg(txtNuevoCg.Text))
                    {
                        MessageBox.Show("No se ha podido crear el perfil ", "Cg3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Modulo creado correctamente", "Cg3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ultrgCg.Enabled = false;
                        txtNuevoCg.Text = "";
                        CargaModuloCg();
                        nuevoModuloCG = false;
                    }
                }
                else if (editarModuloCg)
                {
                    if (NegUsuarios.editarModuloCg(id_moduloCg, txtNuevoCg.Text))
                    {
                        MessageBox.Show("Datos guardados con exito.", "Cg3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNuevoCg.Text = "";
                        nuevoModuloCG = false;
                        CargaModuloCg();
                        ultraGridAccesosCg.DataSource = null;
                    }
                    else
                    {
                        MessageBox.Show("Algo ocurrio al guardar.", "Cg3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                CancelarCg();
            }

            


        }
        private void nuevoPerfilToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nuevoPerfilCg = true;
            nuevoModuloCG = false;
            lbNuevoCg.Text = "Nombre del Perfil*:";
            txtNuevoCg.Enabled = true;
            ultrgCg.Enabled = true;
            btnEliminarCG.Enabled = false;
        }

        private void nuevoModuloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nuevoModuloCG = true;
            nuevoPerfilCg = false;
            lbNuevoCg.Text = "Nombre del Modulo*:";
            txtNuevoCg.Enabled = true;
            ultrgCg.Enabled = true;
            btnEliminarCG.Enabled = false;
        }
        private void btnEditarCg_Click(object sender, EventArgs e)
        {
            CancelarCg();
            if (ultraGridPerfilesCg.Selected.Rows.Count == 1)
            {
                UltraGridRow fila = ultraGridPerfilesCg.ActiveRow;
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + fila.Cells["Perfil"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    editarPerfilCg = true;
                    HabilitarBotonesCg(false, false, true);
                    id_PerfilCg = Convert.ToInt16(fila.Cells["ID"].Value.ToString());
                    txtNuevoCg.Text = fila.Cells["Perfil"].Value.ToString();
                    txtNuevoCg.Enabled = true;
                    txtNuevoCg.Focus();
                    ultrgCg.Enabled = true;
                    btnEliminarCG.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un Perfil.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnCancelarCg_Click(object sender, EventArgs e)
        {
            CancelarCg();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CancelarCg();
            UltraGridRow Fila = ultraGridModuloCg.ActiveRow;
            if (ultraGridModuloCg.Selected.Rows.Count == 1)
            {
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + Fila.Cells["MODULO"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    editarModuloCg = true;
                    txtNuevoCg.Enabled = true;
                    HabilitarBotonesCg(false, false, true);
                    txtNuevoCg.Text = Fila.Cells["MODULO"].Value.ToString();
                    id_moduloCg = Convert.ToInt16(Fila.Cells["ID"].Value.ToString());
                    ultrgCg.Enabled = true;
                    btnEliminarCG.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un modulo para continuar...", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ultraGridModuloCg_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point mousepoint = new Point(e.X, e.Y);
                contextMenuStrip3.Show(ultraGridModuloCg, mousepoint);
            }
        }

        private void btnEliminarCG_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado;
                if (editarModuloCg)
                {
                    resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {
                        if (NegUsuarios.eliminarModuloCg(id_moduloCg))
                        {
                            MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CancelarCg();
                            ultraGridAccesosCg.DataSource = null;
                        }
                        else
                            MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CargaModuloCg();
                        CancelarCg();
                    }
                }
                else if (editarPerfilCg)
                {
                    resultado = MessageBox.Show("Desea eliminar los Datos?", " ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (resultado == DialogResult.Yes)
                    {

                        if (NegUsuarios.eliminarPerfilCg(id_PerfilCg))
                        {
                            MessageBox.Show("Datos Eliminados Correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            HabilitarBotonesCg(true, true, true);
                            CancelarCg();
                            ultraGridAccesosCg.DataSource = null;
                            ultraGridModuloCg.DataSource = null;

                        }
                        else
                            MessageBox.Show("Operación Invalida", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CargaPerfilesCg();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void crearPerfilCg()
        {
            if (!NegUsuarios.crearPerfilesCg(txtNuevoCg.Text))
            {
                MessageBox.Show("No se ha podido crear el perfil ", "Sic-3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CancelarCg();
            }
            else
            {
                MessageBox.Show("Perfil creado correctamente", "Sic-3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CancelarCg();
            }
        }
        public void editaPerfilCg()
        {
            if (NegUsuarios.editarPerfilCg(id_PerfilCg, txtNuevoCg.Text))
            {
                MessageBox.Show("Datos editados con exito.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Information);
                editarPerfil = false;
            }
            else
            {
                MessageBox.Show("Algo ocurrio al editar.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ultraGridPerfil_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridPerfil.DisplayLayout.Bands[0];

            //ultraGridProcedimiento.DisplayLayout.ViewStyle = ViewStyle.SingleBand;
            //grid.DisplayLayout.Override.Allow

            ultraGridPerfil.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            //grid.DisplayLayout.GroupByBox.Prompt = "Arrastrar la columna que desea agrupar";
            ultraGridPerfil.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridPerfil.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridPerfil.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridPerfil.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridPerfil.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;

            //Caracteristicas de Filtro en la grilla
            ultraGridPerfil.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            ultraGridPerfil.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            ultraGridPerfil.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.WithOperand;
            ultraGridPerfil.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.Row;
            ultraGridPerfil.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FilterRow;
            //
            ultraGridPerfil.DisplayLayout.UseFixedHeaders = true;

            //Dimension los registros
            ultraGridPerfil.DisplayLayout.Bands[0].Columns[0].Width = 60;
            ultraGridPerfil.DisplayLayout.Bands[0].Columns["Perfil"].Width = 700;
            ultraGridPerfil.DisplayLayout.Bands[0].Columns["Cantidad"].Hidden = true;
        }

        private void ultraGridModulo_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridModulo.DisplayLayout.Bands[0];

            ultraGridModulo.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            //grid.DisplayLayout.GroupByBox.Prompt = "Arrastrar la columna que desea agrupar";
            ultraGridModulo.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridModulo.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridModulo.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridModulo.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridModulo.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;

            e.Layout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
            ultraGridModulo.DisplayLayout.Bands[0].Columns["TODO"].Hidden = true;
        }

        private void ultraGridAccesos_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
        }

        private void ultraGridPerfilesSic_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridPerfilesSic.DisplayLayout.Bands[0];

            //ultraGridProcedimiento.DisplayLayout.ViewStyle = ViewStyle.SingleBand;
            //grid.DisplayLayout.Override.Allow

            ultraGridPerfilesSic.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            //grid.DisplayLayout.GroupByBox.Prompt = "Arrastrar la columna que desea agrupar";
            ultraGridPerfilesSic.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridPerfilesSic.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridPerfilesSic.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridPerfilesSic.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridPerfilesSic.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;

            //Caracteristicas de Filtro en la grilla
            ultraGridPerfilesSic.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            ultraGridPerfilesSic.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            ultraGridPerfilesSic.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.WithOperand;
            ultraGridPerfilesSic.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.Row;
            ultraGridPerfilesSic.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FilterRow;
            //
            ultraGridPerfilesSic.DisplayLayout.UseFixedHeaders = true;

            //Dimension los registros
            ultraGridPerfilesSic.DisplayLayout.Bands[0].Columns[0].Width = 60;
            ultraGridPerfilesSic.DisplayLayout.Bands[0].Columns["Perfil"].Width = 700;
        }

        private void ultraGridModuloSic_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridModuloSic.DisplayLayout.Bands[0];

            ultraGridModuloSic.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            ultraGridModuloSic.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridModuloSic.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridModuloSic.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridModuloSic.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridModuloSic.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;

            e.Layout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
        }

        private void ultraGridAccesosSic_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
        }

        private void ultraGridPerfilesCg_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridPerfilesCg.DisplayLayout.Bands[0];

            //ultraGridProcedimiento.DisplayLayout.ViewStyle = ViewStyle.SingleBand;
            //grid.DisplayLayout.Override.Allow

            ultraGridPerfilesCg.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            //grid.DisplayLayout.GroupByBox.Prompt = "Arrastrar la columna que desea agrupar";
            ultraGridPerfilesCg.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridPerfilesCg.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridPerfilesCg.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridPerfilesCg.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridPerfilesCg.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;

            //Caracteristicas de Filtro en la grilla
            ultraGridPerfilesCg.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            ultraGridPerfilesCg.DisplayLayout.Override.FilterEvaluationTrigger = Infragistics.Win.UltraWinGrid.FilterEvaluationTrigger.OnCellValueChange;
            ultraGridPerfilesCg.DisplayLayout.Override.FilterOperatorLocation = Infragistics.Win.UltraWinGrid.FilterOperatorLocation.WithOperand;
            ultraGridPerfilesCg.DisplayLayout.Override.FilterClearButtonLocation = Infragistics.Win.UltraWinGrid.FilterClearButtonLocation.Row;
            ultraGridPerfilesCg.DisplayLayout.Override.SpecialRowSeparator = Infragistics.Win.UltraWinGrid.SpecialRowSeparator.FilterRow;
            //
            ultraGridPerfilesCg.DisplayLayout.UseFixedHeaders = true;

            //Dimension los registros
            ultraGridPerfilesCg.DisplayLayout.Bands[0].Columns[0].Width = 60;
            ultraGridPerfilesCg.DisplayLayout.Bands[0].Columns["Perfil"].Width = 700;
        }

        private void ultraGridModuloCg_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            UltraGridBand bandUno = ultraGridModuloCg.DisplayLayout.Bands[0];

            ultraGridModuloCg.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            ultraGridModuloCg.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            ultraGridModuloCg.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            bandUno.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            bandUno.Override.CellClickAction = CellClickAction.RowSelect;
            bandUno.Override.CellDisplayStyle = CellDisplayStyle.PlainText;

            ultraGridModuloCg.DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn;
            ultraGridModuloCg.DisplayLayout.Override.RowSizing = RowSizing.AutoFree;
            ultraGridModuloCg.DisplayLayout.Override.RowSizingArea = RowSizingArea.EntireRow;
            e.Layout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
        }

        private void ultraGridAccesosCg_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.PerformAutoResizeColumns(true, PerformAutoSizeType.AllRowsInBand);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            CancelarHis();
            if (ultraGridPerfil.Selected.Rows.Count == 1)
            {
                UltraGridRow fila = ultraGridPerfil.ActiveRow;
                if (MessageBox.Show("¿Está seguro de editar ó eliminar: \n\r"
                    + fila.Cells["Perfil"].Value.ToString() + "?", "HIS3000",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    HabilitarNuevo(true, true, false, false);
                    editarPerfil = true;
                    HabilitarBotones(false, false, true);
                    habilitarCampos(true, false);
                    id_perfil = Convert.ToInt16(fila.Cells["Codigo"].Value.ToString());
                    txtPerfil.Text = fila.Cells["Perfil"].Value.ToString();
                    txtPerfil.Focus();
                    ultrgHis.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Debe elegir un Perfil.", "HIS3000", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
