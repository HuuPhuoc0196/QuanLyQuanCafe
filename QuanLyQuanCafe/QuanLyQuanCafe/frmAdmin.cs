﻿using System;
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
            txtFoodID.DataBindings.Add("Text", dgvFood.DataSource, "Mã", true, DataSourceUpdateMode.Never);
            txtFoodName.DataBindings.Add("Text", dgvFood.DataSource, "Tên", true, DataSourceUpdateMode.Never);
            nmFoodPrice.DataBindings.Add("Value", dgvFood.DataSource, "Giá", true, DataSourceUpdateMode.Never);
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

        private void txtFoodID_TextChanged(object sender, EventArgs e)
        {
            if (dgvFood.SelectedCells.Count > 0)
            {
                int id = 0;
                try
                {
                    id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["Mã loại"].Value;
                }
                catch
                {
                    return;
                }
                Category category = CategoryDAO.Instance.GetCategoryByID(id);
                cbFoodCatagory.SelectedIndex = category.ID - 1;
            }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtFoodName.Text;
            int idCategory = (cbFoodCatagory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, idCategory, price))
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadTableFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
                MessageBox.Show("Thêm không thành công vì dữ liệu đã tồn tại Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtFoodID.Text);
            string name = txtFoodName.Text;
            int idCategory = (cbFoodCatagory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.UpdateFood(id, name, idCategory, price))
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadTableFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
                MessageBox.Show("Sửa không thành công. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Bạn có chắc muốn xóa {0} ra khỏi danh sách thức ăn không ?", txtFoodName.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                int idFood = Convert.ToInt32(txtFoodID.Text);

                if (FoodDAO.Instance.DeleteFood(idFood))
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    LoadTableFood();
                    if (deleteFood != null)
                        deleteFood(this, new EventArgs());
                }
                else
                    MessageBox.Show("Xóa không thành công. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            string name = txtSearchFoodName.Text;
            foodList.DataSource = FoodDAO.Instance.SearchFoodByName(name);
        }

        #endregion

    }
}
