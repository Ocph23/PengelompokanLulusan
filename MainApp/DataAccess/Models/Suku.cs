using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess.Models
{
   public class Suku:BaseNotify
    {

        private string nama;
        private int nilai;

        public string Nama
        {
            get { return nama; }
            set { SetProperty(ref nama ,value); }
        }

        public int Nilai
        {
            get { return nilai; }
            set { SetProperty(ref nilai, value); }
        }


    }


    public class Gelombang : BaseNotify
    {

        private string nama;
        private int nilai;

        public string Nama
        {
            get { return nama; }
            set { SetProperty(ref nama, value); }
        }

        public int Nilai
        {
            get { return nilai; }
            set { SetProperty(ref nilai, value); }
        }


    }
}
