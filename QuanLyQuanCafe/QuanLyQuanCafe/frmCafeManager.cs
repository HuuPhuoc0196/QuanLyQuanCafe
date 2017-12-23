using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class frmCafeManager : Form
    {
        private Account account;

        public Account Account
        {
            get { return account; }
            set { account = value; ChangeAccount(account.Type); }
        }
        public frmCafeManager(Account acc)
        {
            InitializeComponent();
            this.Account = acc;
            loadTable();
            LoadCaterogy();
            LoadComboboxTable(cbSwitchTable);
        }
        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + account.DisplayName + ")";
        }
        void loadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHight };
                btn.Text = item.Name + Environment.NewLine + item.Status;  //  Environment.NewLine  = /n
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<QuanLyQuanCafe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            CultureInfo culture = new CultureInfo("vi-VN");
            foreach (QuanLyQuanCafe.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Price.ToString("c", culture));
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString("c", culture));
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            txtTotalPrice.Text = totalPrice.ToString("c", culture);
        }

        void LoadCaterogy()
        {
            cbCategory.DataSource = CategoryDAO.Instance.GetListCategory();
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCaterogyID(int id)
        {
            cbFood.DataSource = FoodDAO.Instance.GetFoodByCaterogyID(id);
            cbFood.DisplayMember = "Name";
        }

        void LoadComboboxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion

        #region Event
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;   // lưu table vào lsvBill
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCaNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountProfile f = new frmAccountProfile(account);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acount.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin f = new frmAdmin();
            f.loginAccount = account;
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCaterogyID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCaterogyID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            loadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCaterogyID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCaterogyID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn cần thêm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int idFood = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;
            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxBillId(), idFood, count, table.ID);
            }
            else
            {
                int countBill = lsvBill.Items.Count;
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count, table.ID);
            }
            loadTable();
            ShowBill(table.ID);
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = -1;
            try
            {
                idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn chưa chọn bàn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            int disCount = Convert.ToInt32(nmDisCount.Value);
            float totalPrice = float.Parse(txtTotalPrice.Text.Split(',')[0]);
            float finalTotalPrice = totalPrice * ((float)(100 - disCount) / 100);
            CultureInfo culture = new CultureInfo("vi-VN");
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho bàn {0}? \nTổng tiền bạn phải trả sau khi đã giảm giá {1}% là {2}", table.Name, disCount, finalTotalPrice.ToString("c", culture)), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, disCount, totalPrice);
                    loadTable();
                    ShowBill(idBill);
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            Table table1 = (lsvBill.Tag as Table);
            Table table2 = (cbSwitchTable.SelectedItem as Table);
            if (MessageBox.Show(string.Format("Bạn có chắc muốn chuyển từ {0} sang {1} không ?",table1.Name, table2.Name), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                int idTable1 = table1.ID;
                int idTable2 = table2.ID;
                TableDAO.Instance.SwitchTable(idTable1, idTable2);
                loadTable();
            }
        }

        #endregion

    }
}
