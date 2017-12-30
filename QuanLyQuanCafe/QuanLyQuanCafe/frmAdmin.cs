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
        BindingSource AccountList = new BindingSource();
        BindingSource foodCategory = new BindingSource();

        public Account loginAccount;
        public frmAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region Method

        void Load()
        {
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtPageNum.Text));
            LoadTableFood();
            AddFoodBinding();
            LoadCategoryByCombobox(cbFoodCatagory);
            LoadListAccount();
            LoadListAccountType();
            AddAccountBinding();
            LoadFoodCategory();
            AddFoodCategoryBinding();
        }

        void AddFoodBinding()
        {
            txtFoodID.DataBindings.Add("Text", dgvFood.DataSource, "Mã", true, DataSourceUpdateMode.Never);
            txtFoodName.DataBindings.Add("Text", dgvFood.DataSource, "Tên", true, DataSourceUpdateMode.Never);
            nmFoodPrice.DataBindings.Add("Value", dgvFood.DataSource, "Giá", true, DataSourceUpdateMode.Never);
        }

        void AddFoodCategoryBinding()
        {
            txtCategoryID.DataBindings.Add("Text", dgvCategory.DataSource, "Mã", true, DataSourceUpdateMode.Never);
            txtCategoryName.DataBindings.Add("Text", dgvCategory.DataSource, "Tên", true, DataSourceUpdateMode.Never);
        }

        void LoadCategoryByCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            dgvBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut, pageNum);
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
        void LoadFoodCategory()
        {
            foodCategory.DataSource = CategoryDAO.Instance.GetFoodCategory();
            dgvCategory.DataSource = foodCategory;
        }
        void LoadListAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.GetListAccount();
            dgvAccount.DataSource = AccountList;
        }

        void AddAccountBinding()
        {
            txtUser.DataBindings.Add("Text", dgvAccount.DataSource, "Tài khoản", true, DataSourceUpdateMode.Never);
            txtDisPlayName.DataBindings.Add("Text", dgvAccount.DataSource, "Tên hiển thị", true, DataSourceUpdateMode.Never);

        }

        void LoadListAccountType()
        {
            cbAccountType.DataSource = AccountTypeDAO.Instance.GetListAccountType();
            cbAccountType.DisplayMember = "Type";
        }
        #endregion

        #region Event
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtPageNum.Text));
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
                    string nameFoodCategory = (string)dgvFood.SelectedCells[0].OwningRow.Cells["Danh Mục"].Value;
                    id = FoodDAO.Instance.GetIDCategoryByNameFood(nameFoodCategory).IdCaterogy;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbFoodCatagory.SelectedIndex = category.ID - 1;
                }
                catch
                {
                    return;
                }
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
        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            if (dgvAccount.SelectedCells.Count > 0)
            {
                try
                {
                    cbAccountType.SelectedIndex = (int)dgvAccount.SelectedCells[0].OwningRow.Cells["Loại tài khoản"].Value;
                }
                catch
                {
                    return;
                }
            }
        }
        
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUser.Text;
            string displayName = txtDisPlayName.Text;
            int type = (cbAccountType.SelectedItem as AccountType).Type;

            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadListAccount();
            }
            else
                MessageBox.Show("Thêm tài khoản không thành công vì dữ liệu có cột trống hoặc trùng dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUser.Text;
            if(userName == loginAccount.UserName)
            {
                MessageBox.Show("Bạn không thể xóa tài khoản bạn đang dùng. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (MessageBox.Show(string.Format("Bạn có chắc muốn xóa {0} ra khỏi danh sách Tài khoản không ?", userName), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                if (AccountDAO.Instance.DeleteAccount(userName))
                {
                    MessageBox.Show("Xóa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    LoadListAccount();
                }
                else
                    MessageBox.Show("Xóa tài khoản không thành công. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            string userName = txtUser.Text;
            string displayName = txtDisPlayName.Text;
            int type = (cbAccountType.SelectedItem as AccountType).Type;

            if (AccountDAO.Instance.UpdateAccountNotPass(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadListAccount();
            }
            else
                MessageBox.Show("Sửa tài khoản không thành công. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txtUser.Text;

            if (AccountDAO.Instance.ResetPass(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadListAccount();
            }
            else
                MessageBox.Show("Đặt lại mật khẩu không thành công. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnFirst_Click(object sender, EventArgs e)
        {
            txtPageNum.Text = "1";
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 20;
            if (sumRecord % 20 != 0)
                lastPage++;
            txtPageNum.Text = lastPage.ToString();
        }

        private void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txtPageNum.Text));
        }

        private void btnPreviours_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageNum.Text);
            if (page > 1)
                page--;
            txtPageNum.Text = page.ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txtPageNum.Text);

            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            int lastPage = sumRecord / 20;
            if (sumRecord % 20 != 0)
                lastPage++;

            if (page < lastPage)
                page++;
            txtPageNum.Text = page.ToString();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            frmReport f = new frmReport(dtpkFromDate.Value, dtpkToDate.Value);
            f.ShowDialog();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txtCategoryName.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadFoodCategory();
                LoadCategoryByCombobox(cbFoodCatagory);
            }
            else
                MessageBox.Show("Thêm không thành công vì dữ liệu đã tồn tại Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(string.Format("Bạn có chắc muốn xóa danh mục {0} ra khỏi danh sách không ?", txtCategoryName.Text), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                int idCategory = Convert.ToInt32(txtCategoryID.Text);

                if (CategoryDAO.Instance.DeleteFoodCategory(idCategory))
                {
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    LoadFoodCategory();
                    LoadCategoryByCombobox(cbFoodCatagory);
                }
                else
                    MessageBox.Show("Xóa không thành công. \nBạn phải xóa bên thức ăn trước \nErron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtCategoryID.Text);
            string name = txtCategoryName.Text; 

            if (CategoryDAO.Instance.UpdateFoodCategory(id, name))
            {
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                LoadFoodCategory();
                LoadCategoryByCombobox(cbFoodCatagory);
            }
            else
                MessageBox.Show("Sửa không thành công. Erron", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadFoodCategory();
        }
        #endregion

       

    }
}
