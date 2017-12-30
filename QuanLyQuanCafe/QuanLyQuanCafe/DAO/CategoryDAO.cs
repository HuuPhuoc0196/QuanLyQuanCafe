using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class CategoryDAO
    {
        private static CategoryDAO instance;

        internal static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> listCategory = new List<Category>();
            string query = "SELECT * FROM dbo.FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Category cate = new Category(item);
                listCategory.Add(cate);
            }
            return listCategory;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;
            string query = "SELECT * FROM FoodCategory WHERE Id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }
            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = "EXEC usp_InsertCategory @name ";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] {name });

            return result > 0;
        }

        public DataTable GetFoodCategory()
        {
            string query = "SELECT Id AS [Mã], NAME AS [Tên] FROM FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return data;
        }

        public bool UpdateFoodCategory(int id, string name)
        {
            string query = "UPDATE FoodCategory SET Name = @Name WHERE Id = @id";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name, id });

            return result > 0;
        }

        public bool DeleteFoodCategory(int idFoodCategory)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFoodCategory);

            string query = "EXEC usp_DeleteFoodCategory @idCategory ";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { idFoodCategory });

            return result > 0;
        }

    }
}
