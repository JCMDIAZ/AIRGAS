using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SWYRA.General;

namespace SWYRA
{
    public partial class FrmProductos : Form
    {
        List<Catalogos> listCatalogo = new List<Catalogos>();
        List<Productos> listProductos = new List<Productos>();

        public FrmProductos()
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
            txtClaveProducto.Text = "";
            txtCodigoBarra.Text = "";
            deFechaAlta.DateTime = DateTime.Today;
            chkBaja.Checked = false;
            deFechaBaja.EditValue = null;
            txtDescripcion.Text = "";
            txtNombreCorto.Text = "";
            chkPEPS.Checked = false;
            chkManejaPorLotes.Checked = false;
            cbProveedor.EditValue = null;
            cbFamilia.EditValue = null;
            cbUnidadMedida.EditValue = null;
            seMaximoCompra.Value = 0;
            sePrecioCompra.Value = 0;
            sePrecioVenta.Value = 0;
            chkIVA.Checked = false;
            seIVA.Value = 0;
            seIVA.Enabled = chkIVA.Checked;
            chkIEPS.Checked = false;
            seIEPS.Value = 0;
            seIEPS.Enabled = chkIEPS.Checked;
            txtMarca.Text = "";
            txtNumParte.Text = "";
            txtMaterial.Text = "";
            chkLimpieza.Checked = false;
            chkActivo.Checked = false;
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            listCatalogo = CargaCatalogos();
            listProductos = CargaProductos();
            gcProductos.DataSource = listProductos;
            cbFamilia.Properties.DataSource = listCatalogo.Where(o => o.tipocatalogo == "0001").ToList();
            cbUnidadMedida.Properties.DataSource = listCatalogo.Where(o => o.tipocatalogo == "0002").ToList();
            GetGridProducto();
        }

        private List<Catalogos> CargaCatalogos()
        {
            List<Catalogos> listPaso = new List<Catalogos>();
            try
            {
                var query = "SELECT TIPOCATALOGO, CLAVE, NOMBRE, ABREVIATURA, ACTIVO FROM CATALOGOS WHERE ACTIVO = 1 ORDER BY TIPOCATALOGO, CLAVE";
                listPaso = GetDataTable("DB", query, 1).ToList<Catalogos>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listPaso;
        }

        private List<Productos> CargaProductos()
        {
            List<Productos> listPaso = new List<Productos>();
            try
            {
                var query = "SELECT CVEART, PEPS, DESCRIPCION, NOMBRECORTO, MANEJAPORLOTES, CVEPROVEEDOR, FAMILIA, UNIDADMEDIDA, " +
                            "FECHAALTA, BAJA, FECHABAJA, PRECIOCOMPRA, PRECIOVENTA, IVA, INCLUYEIVA, IEPS, INCLUYEIEPS, MAXIMOCOMPRA, " +
                            "CODIGOBARRA, MARCA, NUMERODEPARTE, LIMPIEZA, MATERIAL, p.Activo, '' strproveedor, c1.NOMBRE strfamilia, c2.NOMBRE strunidadmedida " +
                            "FROM PRODUCTOS p LEFT JOIN CATALOGOS c1 on c1.TIPOCATALOGO = '0001' and c1.CLAVE = FAMILIA and c1.ACTIVO = 1 " +
                            "LEFT JOIN CATALOGOS c2 on c2.TIPOCATALOGO = '0002' and c2.CLAVE = UNIDADMEDIDA and c2.ACTIVO = 1 " +
                            "ORDER BY CVEART";
                listPaso = GetDataTable("DB", query, 2).ToList<Productos>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return listPaso;
        }

        private void chkBaja_CheckedChanged(object sender, EventArgs e)
        {
            if (deFechaBaja.Text == "" || (deFechaBaja.DateTime == DateTime.Today))
            {
                deFechaBaja.EditValue = (chkBaja.Checked) ? DateTime.Today : (DateTime?) null;
                chkActivo.Checked = !chkBaja.Checked;
                chkActivo.Enabled = !chkBaja.Checked;
            }
        }

        private void chkIVA_CheckedChanged(object sender, EventArgs e)
        {
            seIVA.EditValue = (chkIVA.Checked) ? 16 : 0;
            seIVA.Enabled = chkIVA.Checked;
        }

        private void chkIEPS_CheckedChanged(object sender, EventArgs e)
        {
            seIEPS.EditValue = (chkIEPS.Checked) ? 3 : 0;
            seIEPS.Enabled = chkIEPS.Checked;
        }

        private void GetGridProducto()
        {
            if (gridView1.RowCount > 0)
            {
                var productoid = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "cveart");
                CargaDatos(productoid.ToString());
            }
            else
            {
                Limpiar();
            }
        }

        private void CargaDatos(string productoID)
        {
            var prod = listProductos.First(o => o.cveart == productoID);
            txtClaveProducto.Text = prod.cveart;
            txtCodigoBarra.Text = prod.codigobarra;
            deFechaAlta.DateTime = prod.fechaalta;
            chkBaja.Checked = prod.baja;
            deFechaBaja.EditValue = prod.fechabaja;
            txtDescripcion.Text = prod.descripcion;
            txtNombreCorto.Text = prod.nombrecorto;
            chkPEPS.Checked = prod.peps;
            chkManejaPorLotes.Checked = prod.manejaporlotes;
            cbProveedor.EditValue = prod.cveproveedor;
            cbFamilia.EditValue = prod.familia;
            cbUnidadMedida.EditValue = prod.unidadmedida;
            seMaximoCompra.EditValue = prod.maximocompra;
            sePrecioCompra.EditValue = prod.preciocompra;
            sePrecioVenta.EditValue = prod.precioventa;
            chkIVA.Checked = prod.incluyeiva;
            seIVA.EditValue = prod.iva;
            seIVA.Enabled = chkIVA.Checked;
            chkIEPS.Checked = prod.incluyeieps;
            seIEPS.EditValue = prod.ieps;
            seIEPS.Enabled = chkIEPS.Checked;
            txtMarca.Text = prod.marca;
            txtNumParte.Text = prod.numerodeparte;
            txtMaterial.Text = prod.material;
            chkLimpieza.Checked = prod.limpieza;
            chkActivo.Checked = prod.activo;
        }

        class Variables
        {
            public string Id { get; set; }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (ValidaDatos())
            {
                try
                {
                    var query = @"";
                    if (txtClaveProducto.Text == "")
                    {
                        Variables var1 = GetDataTable("DB", @"select cast((isnull(max(cast(CVEART as int)),0) + 1) as varchar(5)) Id from PRODUCTOS", 17).ToData<Variables>();
                        txtClaveProducto.Text = var1.Id;
                        query =
                            @"INSERT PRODUCTOS (CVEART, PEPS, DESCRIPCION, NOMBRECORTO, MANEJAPORLOTES, CVEPROVEEDOR, FAMILIA, UNIDADMEDIDA, " +
                            "FECHAALTA, BAJA, FECHABAJA, PRECIOCOMPRA, PRECIOVENTA, IVA, INCLUYEIVA, IEPS, INCLUYEIEPS, MAXIMOCOMPRA, " +
                            "CODIGOBARRA, MARCA, NUMERODEPARTE, LIMPIEZA, MATERIAL, Activo) " +
                            "VALUES (RIGHT('0000" + var1.Id + "',4), " + ((chkPEPS.Checked) ? "1" : "0") + ", '" + txtDescripcion.Text + 
                            "', '" + txtNombreCorto.Text + "', " + ((chkManejaPorLotes.Checked) ? "1" : "0") + ", '0000', '" + 
                            cbFamilia.GetColumnValue("clave") + "', '" + cbUnidadMedida.GetColumnValue("clave") + "', " +deFechaAlta.DateTime.ToStrSql() +  ", " + ((chkBaja.Checked) ? "1" : "0") + ", " + 
                            deFechaBaja.DateTime.ToStrSql() + ", " + sePrecioCompra.Value + ", " + sePrecioVenta.Value + ", " +
                            seIVA.Value + ", " + ((chkIVA.Checked) ? "1" : "0") + ", " + seIEPS.Value + ", " + ((chkIEPS.Checked) ? "1" : "0") + ", " +
                            seMaximoCompra.Value + ", '" + txtCodigoBarra.Text + "', '" + txtMarca.Text + "', '" + txtNumParte.Text + "', " +
                            ((chkLimpieza.Checked) ? "1" : "0") + ", '" + txtMaterial.Text + "', " + ((chkActivo.Checked) ? "1" : "0") + ")";
                    }
                    else
                    {
                        query =
                            @"UPDATE PRODUCTOS SET PEPS = " + ((chkPEPS.Checked) ? "1" : "0") + ", DESCRIPCION = '" +
                            txtDescripcion.Text + "', NOMBRECORTO = '" + txtNombreCorto.Text + "', MANEJAPORLOTES = " +
                            ((chkManejaPorLotes.Checked) ? "1" : "0") + ", FAMILIA = '" + cbFamilia.GetColumnValue("Clave") + "', UNIDADMEDIDA = '" +
                            cbUnidadMedida.GetColumnValue("Clave") + "', FECHAALTA = " + deFechaAlta.DateTime.ToStrSql() + ", BAJA = " +
                            ((chkBaja.Checked) ? "1" : "0") + ", FECHABAJA = " + deFechaBaja.DateTime.ToStrSql() + ", PRECIOCOMPRA = " +
                            sePrecioCompra.Value + ", PRECIOVENTA = " + sePrecioVenta.Value + ", IVA = " + seIVA.Value + ", INCLUYEIVA = " +
                            ((chkIVA.Checked) ? "1" : "0") + ", IEPS = " + seIEPS.Value + ", INCLUYEIEPS = " + ((chkIEPS.Checked) ? "1" : "0") +
                            ", MAXIMOCOMPRA = " + seMaximoCompra.Value + ", CODIGOBARRA = '" + txtCodigoBarra.Text + "', MARCA = '" +
                            txtMarca.Text + "', NUMERODEPARTE = '" + txtNumParte.Text + "', LIMPIEZA = " +
                            ((chkLimpieza.Checked) ? "1" : "0") + ", MATERIAL = '" + txtMaterial.Text + "', Activo = " + ((chkActivo.Checked) ? "1" : "0") +
                            "WHERE CVEART = '" + txtClaveProducto.Text + "'";
                    } 
                    var res = GetExecute("DB", query, 3);
                    MessageBox.Show(@"Guardado satisfactoriamente.");
                    listProductos = CargaProductos();
                    gcProductos.DataSource = listProductos;
                    GetGridProducto();
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
            if (txtDescripcion.Text == "")
            {
                MessageBox.Show(@"Favor de agregar la descripción del producto.");
                txtDescripcion.Focus();
                b = false;
            }
            else if (txtNombreCorto.Text == "")
            {
                MessageBox.Show(@"Favor de asignar el nombre corto del producto.");
                txtNombreCorto.Focus();
                b = false;
            }
            return b;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetGridProducto();
        }
    }
}
