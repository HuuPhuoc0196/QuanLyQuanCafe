using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class frmAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        public frmAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region Method

        void Load()
        {
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadTableFood();
            AddFoodBinding();
            LoadCategoryByCombobox(cbFoodCatagory);
        }

        void AddFoodBinding()
        {
            txtFoodID.DataBindings.Add("Text", dgvFood.DataSource, "Mã");
            txtFoodName.DataBindings.Add("Text", dgvFood.DataSource, "Tên");
            nmFoodPrice.DataBindings.Add("Value", dgvFood.DataSource, "Giá");
        }

        void LoadCategoryByCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dgvBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadTableFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetTableFood();
            dgvFood.DataSource = foodList;
        }

        #endregion

        #region Event
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadTableFood();
        }
        #endregion

        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            if (dgvFood.SelectedCells.Count > 0)
            {
                int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["Mã loại"].Value;
                Category category = CategoryDAO.Instance.GetCategoryByID(id);
                cbFoodCatagory.SelectedIndex = category.ID - 1;
            }
        }

        
    }
}
