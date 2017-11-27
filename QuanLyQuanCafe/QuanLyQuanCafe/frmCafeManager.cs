using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class frmCafeManager : Form
    {
        public frmCafeManager()
        {
            InitializeComponent();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCaNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountProfile f = new frmAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin f = new frmAdmin();
            f.ShowDialog();
        }

    }
}
