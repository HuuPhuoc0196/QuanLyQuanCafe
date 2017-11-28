using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            //string query = "SELECT * FROM Account WHERE UserName = @user AND PassWord = @pass";
            string query = "EXEC usp_Login @UserName , @PassWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[]{user, pass});
            return result.Rows.Count > 0;
        }
    }
}
