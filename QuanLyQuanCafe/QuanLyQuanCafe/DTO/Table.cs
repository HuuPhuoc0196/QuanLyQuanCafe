using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Table
    {
        private int iD;

        private string name;

        private string status;
        private object item;

        public Table(int id, string name, string status) 
        {
            this.ID = id;
            this.Name = name;
            this.Status = status;
        }

        public Table(DataRow row)
        {
            this.ID = (int)row["Id"];
            this.Name = row["Name"].ToString();
            this.Status = row["Status"].ToString();
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }


    }
}
