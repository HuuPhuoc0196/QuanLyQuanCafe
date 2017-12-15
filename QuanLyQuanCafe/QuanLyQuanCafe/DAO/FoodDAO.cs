using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetFoodByCaterogyID(int id)
        {
            List<Food> listFoodByCaterogyID = new List<Food>();
            string query = "SELECT * FROM dbo.Food WHERE IdCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFoodByCaterogyID.Add(food);
            }
            return listFoodByCaterogyID;
        }

        public DataTable GetTableFood()
        {
            string query = "SELECT f.Id AS [Mã], f.NAME AS [Tên], f.price AS [Giá], fc.Name AS [Danh Mục] FROM Food AS f, FoodCategory AS fc WHERE f.IdCategory = fc.Id";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        public bool InsertFood(string name, int idCategory, float price)
        {
            string query = "EXEC usp_InsertFood @Name , @IdCategory , @price";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, idCategory, price });

            return result > 0;
        }

        public bool UpdateFood(int id, string name, int idCategory, float price)
        {
            string query = "UPDATE Food SET Name = @Name , IdCategory = @idCategory , price = @price WHERE Id = @id";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, idCategory, price, id });

            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood);

            string query = "DELETE FROM Food WHERE Id = @id";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { idFood });

            return result > 0;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = new List<Food>();
            DataTable data = DataProvider.Instance.ExecuteQuery(string.Format("SELECT * FROM Food WHERE dbo.fuConvertToUnsign1(NAME) LIKE dbo.fuConvertToUnsign1(N'%{0}%')", name));
            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }

            return listFood;
        }
    }
}
