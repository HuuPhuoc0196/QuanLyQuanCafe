using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE IdTableFood = " + id + " AND status = 0");
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }

        public void InsertBill(int idTableFood)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC usp_InsertBill @idTableFood", new object[] { idTableFood });
        }

        public int GetMaxBillId()
        {
            return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(Id) FROM Bill");
        }

        public void CheckOut(int idBill, int disCount, float totalPrice)
        {
            string query = "EXEC usp_pay @idBill , @DisCount , @TotalPrice";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] {idBill, disCount, totalPrice});
        }

        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC GetListBillByDate @CheckIn , @CheckOut , @Page ", new object[] { checkIn, checkOut, pageNum });
        }

        public int GetNumBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("EXEC GetNumBillByDate @CheckIn , @CheckOut", new object[] { checkIn, checkOut});
        }
    }
}
