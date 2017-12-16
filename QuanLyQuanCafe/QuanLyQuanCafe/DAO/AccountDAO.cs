using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string user, string pass)
        {
            // mã hóa mật khẩu ---- thư viện MD5
            string hasPass = EnCode(pass);
            //string query = "SELECT * FROM Account WHERE UserName = @user AND PassWord = @pass";
            string query = "EXEC usp_Login @UserName , @PassWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[]{user, hasPass});
            return result.Rows.Count > 0;
        }

        public string EnCode(string pass)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(pass);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hasPass = "";
            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            return hasPass;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Account WHERE UserName = @userName", new object[] {userName});
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool UpdateAccount(string userName, string displayName, string passWord, string newPassWord)
        {
            string hasPass = EnCode(passWord);
            string hasNewPass = EnCode(newPassWord);
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC usp_UpdateAccount @userName , @DisplayName , @PassWord , @NewPassWord", new object[] { userName, displayName, hasPass, hasNewPass });
            return result > 0;
        }

        public DataTable GetListAccount()
        {
            string query = "SELECT UserName as [Tài khoản], DisplayName as [Tên hiển thị], Type as [Loại tài khoản] FROM Account ";
            return DataProvider.Instance.ExecuteQuery(query);
        }


        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = "EXEC usp_InsertAccout @UserName , @DisplayName , @Type";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, displayName, type });

            return result > 0;
        }

        public bool UpdateAccountNotPass(string userName, string displayName, int type)
        {
            string query = "EXEC usp_UpdateAccountNotPass @UserName , @DisplayName , @Type";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, displayName, type });

            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {
            string query = string.Format("DELETE FROM Account WHERE UserName = N'{0}'", userName);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPass(string userName)
        {
            string query = string.Format("UPDATE Account SET [PassWord] = '2251022057731868917119086224872421513662' WHERE UserName = N'{0}'", userName);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
