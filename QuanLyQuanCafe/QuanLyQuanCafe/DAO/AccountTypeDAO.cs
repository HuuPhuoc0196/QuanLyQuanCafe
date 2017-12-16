using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class AccountTypeDAO
    {
        private static AccountTypeDAO instance;

        internal static AccountTypeDAO Instance
        {
            get { if (instance == null) instance = new AccountTypeDAO(); return AccountTypeDAO.instance; }
            private set { AccountTypeDAO.instance = value; }
        }

        public List<AccountType> GetListAccountType()
        {
            string query = "SELECT * FROM AccountType";

            List<AccountType> listAccountType = new List<AccountType>();
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                AccountType accountType = new AccountType(item);
                listAccountType.Add(accountType);
            }

            return listAccountType;
        }
    }
}
