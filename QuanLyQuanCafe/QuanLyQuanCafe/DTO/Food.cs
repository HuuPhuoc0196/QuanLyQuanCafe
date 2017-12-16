using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Food
    {
        public Food(int iD, string name, int idCaterogy, float price)
        {
            this.ID = iD;
            this.Name = name;
            this.IdCaterogy = idCaterogy;
            this.Price = price;
        }

        public Food(DataRow row)
        {
            this.ID = (int)row["ID"];
            this.Name = row["Name"].ToString();
            this.IdCaterogy = (int)row["IdCategory"];
            this.Price = (float)Convert.ToDouble(row["Price"].ToString());
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

        private int idCaterogy;

        public int IdCaterogy
        {
            get { return idCaterogy; }
            set { idCaterogy = value; }
        }

        private float price;

        public float Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
