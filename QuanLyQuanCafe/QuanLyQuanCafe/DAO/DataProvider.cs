using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private static DataProvider instance; // Ctrl + R + E: phim tắt đóng gói

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider()
        {
        }

        public string cnStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        public DataTable ExecuteQuery(string query, Object[] parameter = null)
        {
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand(query, cn);

            if (parameter != null)
            {
                string[] listPara = query.Split(' ');
                int i = 0;
                foreach (var item in listPara)
                {
                    if(item.Contains('@'))
                    {
                        cmd.Parameters.AddWithValue(item, parameter[i]);
                        i++;
                    }
                }
            }


            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(data);
            return data;
        }

        public int ExecuteNonQuery(string query, Object[] parameter = null)
        {
            int data = 0;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand(query, cn);

            if (parameter != null)
            {
                string[] listPara = query.Split(' ');
                int i = 0;
                foreach (var item in listPara)
                {
                    if (item.Contains('@'))
                    {
                        cmd.Parameters.AddWithValue(item, parameter[i]);
                        i++;
                    }
                }
            }
            data = cmd.ExecuteNonQuery();
            return data;
        }

        public Object ExecuteScalar(string query, Object[] parameter = null)
        {
            Object data = 0;
            SqlConnection cn = new SqlConnection(cnStr);
            SqlCommand cmd = new SqlCommand(query, cn);

            if (parameter != null)
            {
                string[] listPara = query.Split(' ');
                int i = 0;
                foreach (var item in listPara)
                {
                    if (item.Contains('@'))
                    {
                        cmd.Parameters.AddWithValue(item, parameter[i]);
                        i++;
                    }
                }
            }
            data = cmd.ExecuteScalar();
            return data;
        }
    }
}
