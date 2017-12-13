using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
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
    public partial class frmAccountProfile : Form
    {
        private Account account;

        public Account Account
        {
            get { return account; }
            set { account = value; ChangeAccount(account); }
        }
        public frmAccountProfile(Account acc)
        {
            InitializeComponent();
            this.Account = acc;
        }

        void ChangeAccount(Account acc)
        {
            txtUser.Text = acc.UserName;
            txtDisPlayName.Text = acc.DisplayName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateAccountInfo()
        {
            string userName = txtUser.Text.Trim();
            string displayName = txtDisPlayName.Text.Trim();
            string passWord = txtPassWord.Text.Trim();
            string newPassWord = txtNewPassWord.Text.Trim();
            string reEnterPassWord = txtReEnterPassWord.Text.Trim();
            if(!newPassWord.Equals(reEnterPassWord))
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, passWord, newPassWord))
                {
                    MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                    MessageBox.Show("Mật khẩu không đúng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }

    public class AccountEvent:EventArgs
    {
        public AccountEvent(Account acc)
        {
            this.Acount = acc;
        }

        private Account acount;

        public Account Acount
        {
            get { return acount; }
            set { acount = value; }
        }


    }
}
