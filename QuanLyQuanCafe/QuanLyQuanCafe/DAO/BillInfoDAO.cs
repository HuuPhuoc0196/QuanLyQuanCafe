using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }
        private BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE Id = " + id);
            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count, int tableID)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC usp_InsertBillInfo @idBill , @idFood , @count , @tableID", new object[] { idBill, idFood, count, tableID});
        }

        public void DeleteBillInfoByFoodID(int id)
        {
            string query = "DELETE FROM BillInfo WHERE IdFood = @idFood";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id });
        }
    }
}
