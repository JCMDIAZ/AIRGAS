using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SWYRA_Movil
{
    public partial class FrmActivacion : Form
    {
        public FrmActivacion()
        {
            InitializeComponent();
        }

        private void FrmActivacion_Load(object sender, EventArgs e)
        {
            txtClave.Text = Program.GetDeviceID(true);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}