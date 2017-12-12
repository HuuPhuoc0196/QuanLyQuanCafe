using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        public  Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int status, int disCount = 0)
        {
            this.ID = id;
            this.DataCheckIn = dateCheckIn;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.DisCount = disCount;
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["Id"];
            this.DataCheckIn = (DateTime?)row["DateCheckIn"]; 
            var dateCheckOutTemp = row["DateCheckOut"];
            if(dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp; 
            this.Status = (int)row["status"];
            if (row["DisCount"].ToString() != "")
                this.DisCount = (int)row["DisCount"];
        }
        private int iD;

        public int ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private DateTime? dateCheckIn;

        public DateTime? DataCheckIn
        {
            get { return dateCheckIn; }
            set { dateCheckIn = value; }
        }

        private DateTime? dateCheckOut;

        public DateTime? DateCheckOut
        {
            get { return dateCheckOut; }
            set { dateCheckOut = value; }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        private int disCount;

        public int DisCount
        {
            get { return disCount; }
            set { disCount = value; }
        }
    }
}
