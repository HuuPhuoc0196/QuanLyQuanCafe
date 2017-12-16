using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    class AccountType
    {
        public AccountType(int id, string name, int type)
        {
            this.ID = id;
            this.Name = name;
            this.Type = Type;
        }

        public AccountType(DataRow row)
        {
            this.ID = (int)row["Id"];
            this.Name = row["Name"].ToString();
            this.Type = (int)row["Type"];
        }

        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
