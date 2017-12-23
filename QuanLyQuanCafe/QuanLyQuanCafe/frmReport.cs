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
    public partial class frmReport : Form
    {
        private DateTime checkIn;
        private DateTime checkOut;
        public frmReport(DateTime checkin, DateTime checkout)
        {
            InitializeComponent();
            this.checkIn = checkin;
            this.checkOut = checkout;
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'QuanLyQuanCafeDataSet.GetListBillByDateForReport' table. You can move, or remove it, as needed.
            this.GetListBillByDateForReportTableAdapter.Fill(this.QuanLyQuanCafeDataSet.GetListBillByDateForReport, checkIn, checkOut);
            this.reportViewer1.RefreshReport();
        }
    }
}
