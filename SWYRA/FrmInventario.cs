using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static SWYRA.General;

namespace SWYRA
{
    public partial class FrmInventario : Form
    {
        List<Areas> listAreas = new List<Areas>();
        List<Catalogos> listCatalogos = new List<Catalogos>();
        List<Inventario> listInventarios = new List<Inventario>();
        private bool sw = true;

        public FrmInventario()
        {
            InitializeComponent();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void Limpiar()
        {
            sw = false;
            txtID.Text = "";
            cbAlmacen.EditValue = null;
            cbArea.EditValue = null;
            cbProducto.EditValue = null;
            txtLinProd.Text = "";
            cbUniMed.EditValue = null;
            seUniMed.EditValue = 0;
            seStockMin.EditValue = 0;
            seStockMax.EditValue = 0;
            seMaster.EditValue = 0;
            seExistencia.EditValue = 0;
            txtConSerie.Text = "";
            txtUbicacion.Text = "";
            chkActivo.Checked = false;
            deFechaModif.EditValue = null;
            sw = true;
        }

        private void FrmInventario_Load(object sender, EventArgs e)
        {
            cbAlmacen.Properties.DataSource = CargaAlmacen();
            CargaArea();
            var alm = (cbAlmacen.Text != "") ? cbAlmacen.GetColumnValue("Clave").ToString() : "";
            cbArea.Properties.DataSource = listAreas.Where(o => o.almacen == alm).ToList();
            cbProducto.Properties.DataSource = CargaProductos();
            CargaCatalogos();
            cbUniMed.Properties.DataSource = listCatalogos.Where(o => o.tipocatalogo == "0002").ToList();
            CargaInventarios();
            gcInventario.DataSource = listInventarios;
            GetGridInventario();
        }

        private List<Almacen> CargaAlmacen()
        {
            List<Almacen> listAlmacenes = new List<Almacen>();
            try
            {
                var query = "SELECT Clave, Nombre, Abreviatura FROM ALMACENES WHERE Activo = 1";
                listAlmacenes = GetDataTable("DB", query, 1).ToList<Almacen>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listAlmacenes;
        }

        private void CargaArea()
        {
            try
            {
                var query = "SELECT AREAID, NOMBRE, ALMACEN FROM AREAS WHERE Activo = 1";
                listAreas = GetDataTable("DB", query, 2).ToList<Areas>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Productos> CargaProductos()
        {
            List<Productos> listProductos = new List<Productos>();
            try
            {
                var query = "SELECT CVEART, DESCRIPCION, NOMBRECORTO, FAMILIA, UNIDADMEDIDA, CODIGOBARRA, MARCA, " +
                            "NUMERODEPARTE, MATERIAL, c1.NOMBRE strfamilia, c2.NOMBRE strunidadmedida " +
                            "FROM PRODUCTOS p JOIN CATALOGOS c1 ON c1.TIPOCATALOGO = '0001' AND c1.CLAVE = p.FAMILIA " +
                            "JOIN CATALOGOS c2 ON c2.TIPOCATALOGO = '0002' AND c2.CLAVE = p.UNIDADMEDIDA " +
                            "WHERE p.Activo = 1";
                listProductos = GetDataTable("DB", query, 3).ToList<Productos>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listProductos;
        }

        private void CargaCatalogos()
        {
            try
            {
                var query = "SELECT TIPOCATALOGO, CLAVE, NOMBRE, ABREVIATURA, ACTIVO FROM CATALOGOS WHERE ACTIVO = 1";
                listCatalogos = GetDataTable("DB", query, 4).ToList<Catalogos>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargaInventarios()
        {
            try
            {
                var query = "SELECT ID, CVEALM, CVEAREA, i.CVEART, LIN_PROD, CON_SERIE, UNI_MED, " +
                            "UNI_EMP, STOCK_MIN, STOCK_MAX, EXIST, MASTERS, i.UBICACION, " +
                            "FCHMOVIMIENTO, i.ACTIVO, a.Nombre strAlmacen, r.NOMBRE strArea, " +
                            "p.DESCRIPCION strProducto, c1.NOMBRE strUniMed " +
                            "FROM INVENTARIO i JOIN ALMACENES a ON a.Clave = i.CVEALM " +
                            "LEFT JOIN AREAS r ON r.AREAID = i.CVEAREA " +
                            "JOIN PRODUCTOS p ON p.CVEART = i.CVEART " +
                            "JOIN CATALOGOS c1 ON c1.TIPOCATALOGO = '0002' AND c1.CLAVE = i.UNI_MED " +
                            "WHERE i.Activo = 1";
                listInventarios = GetDataTable("DB", query, 5).ToList<Inventario>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbAlmacen_EditValueChanged(object sender, EventArgs e)
        {
            cbArea.Properties.DataSource = listAreas.Where(o => o.almacen == cbAlmacen.GetColumnValue("Clave").ToString()).ToList();
            cbArea.EditValue = null;
        }

        private void cbProducto_EditValueChanged(object sender, EventArgs e)
        {
            if (sw)
            {
                cbUniMed.EditValue = cbProducto.GetColumnValue("unidadmedida");
            }
        }

        private void GetGridInventario()
        {
            if (gridView1.RowCount > 0)
            {
                var id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id");
                CargaDatos(id.ToString());
            }
            else
            {
                Limpiar();
            }
        }

        private void CargaDatos(string id)
        {
            sw = false;
            var inv = listInventarios.First(o => o.id == int.Parse(id));
            txtID.Text = id;
            cbAlmacen.EditValue = inv.cvealm;
            cbArea.EditValue = inv.cvearea;
            cbProducto.EditValue = inv.cveart;
            txtLinProd.Text = inv.lin_prod;
            cbUniMed.EditValue = inv.uni_med;
            seUniMed.EditValue = inv.uni_emp;
            seStockMin.EditValue = inv.stock_min;
            seStockMax.EditValue = inv.stock_max;
            seMaster.EditValue = inv.masters;
            seExistencia.EditValue = inv.exist;
            txtConSerie.Text = inv.con_serie;
            txtUbicacion.Text = inv.ubicacion;
            chkActivo.Checked = inv.activo;
            deFechaModif.EditValue = inv.fchmovimiento;
            sw = true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidaDatos())
            {
                try
                {
                    var query = @"";
                    if (txtID.Text == "")
                    {
                        query =
                            @"INSERT INVENTARIO (CVEALM, CVEAREA, CVEART, LIN_PROD, CON_SERIE, UNI_MED, " +
                            "UNI_EMP, STOCK_MIN, STOCK_MAX, EXIST, MASTERS, UBICACION, FCHMOVIMIENTO, ACTIVO) " +
                            "VALUES ('" + cbAlmacen.EditValue + "', '" + (cbArea.EditValue ?? "") + "', '" +
                            cbProducto.EditValue + "', '" + txtLinProd.Text + "', '" + txtConSerie.Text + "', '" +
                            cbUniMed.EditValue + "', " + seUniMed.EditValue + ", " + seStockMin.EditValue + ", " +
                            seStockMax.EditValue + ", " + seExistencia.EditValue + ", " + seMaster.EditValue + ", '" +
                            txtUbicacion.Text + "', GETDATE(), " + (chkActivo.Checked  ? "1" : "0") + ")";
                    }
                    else
                    {
                        query =
                            @"UPDATE INVENTARIO SET CVEALM = '" + cbAlmacen.EditValue +
                            "', CVEAREA = '" + (cbArea.EditValue ?? "") + "', CVEART = '" + cbProducto.EditValue +
                            "', LIN_PROD = '" + txtLinProd.Text + "', CON_SERIE = '" + txtConSerie.Text +
                            "', UNI_MED = '" + cbUniMed.EditValue + "', UNI_EMP = " + seUniMed.EditValue +
                            ", STOCK_MIN = " + seStockMin.EditValue + ", STOCK_MAX = " + seStockMax.EditValue +
                            ", EXIST = " + seExistencia.EditValue + ", MASTERS = " + seMaster.EditValue +
                            ", UBICACION = '" + txtUbicacion.Text + "', FCHMOVIMIENTO = GETDATE(), " +
                            "ACTIVO = " + (chkActivo.Checked ? "1" : "0") + "WHERE ID = '" + txtID.Text + "'";
                    }
                    var res = GetExecute("DB", query, 6);
                    MessageBox.Show(@"Guardado satisfactoriamente.");
                    CargaInventarios();
                    gcInventario.DataSource = listInventarios;
                    GetGridInventario();
                }
                catch (Exception ms)
                {
                    MessageBox.Show(ms.Message);
                }
            }
        }

        private bool ValidaDatos()
        {
            bool b = true;
            if (cbAlmacen.EditValue == null)
            {
                MessageBox.Show(@"Favor de seleccionar el Almacen");
                cbAlmacen.Focus();
                b = false;
            }
            else if ((cbArea.Properties.DataSource as List<Areas>).Any())
            {
                if (cbArea.EditValue == null)
                {
                    MessageBox.Show(@"Favor de seleccionar el Área del Almacen.");
                    cbArea.Focus();
                    b = false;
                }
            }
            else if (cbProducto.EditValue == null)
            {
                MessageBox.Show(@"Favor de seleccionar el Producto");
                cbProducto.Focus();
                b = false;
            }
            return b;
        }
    }
}
