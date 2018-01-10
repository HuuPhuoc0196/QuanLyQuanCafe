using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 100;
        public static int TableHight = 100;
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();
            string query = "EXEC usp_GetTableList";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public void SwitchTable(int idTable1, int idTable2)
        {
            DataProvider.Instance.ExecuteQuery("EXEC usp_SwitchTable @idTable1 , @idTable2", new object[] { idTable1, idTable2 });
        }

        public bool InsertTable(string name, string status)
        {
            string query = "EXEC usp_InsertTable @name , @status ";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, status });

            return result > 0;
        }

        public DataTable GetTable()
        {
            string query = "SELECT Id as [Mã bàn] , Name as [Tên bàn], status as [Trạng thái] FROM TableFood";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }


        public bool UpdateTalbe(int id, string name, string status)
        {
            string query = "UPDATE TableFood Set Name = @name , status = @status WHERE Id = @id";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, status, id });

            return result > 0;
        }

        public bool DeleteTable(int id)
        {

            string query = "EXEC usp_DeleteTable @id";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id });

            return result > 0;
        }
    }
}
