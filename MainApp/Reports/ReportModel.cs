using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Reports
{
    public class ReportModel
    {

        public string NPM { get; set; }
        public string Nama { get; set; }
        public string Suku { get; set; }
        public double IPK { get; set; }
        public double HasilTest { get; set; }
        public double MasaStudi { get; set; }

        public string Gelombang { get; set; }

        public int Nomor { get; set; }

        public bool IsCentroid { get; set; }
      

        public string Jurusan { get; set; }
     
        public string Clustering { get; set; }

        public int Jumlah { get; set; }


    }

    public class StatistikModel
    {
        public string Suku { get; set; }
        public double IPK { get; set; }
        public double HasilTest { get; set; }
        public double MasaStudi { get; set; }
        public string Gelombang { get; set; }
        public bool IsCentroid { get; set; }

        public string Jurusan { get; set; }
        public int Clustering { get; set; }
        public int Jumlah { get; set; }


    }


    public class StatistikModelReport
    {

       public string Atribut { get; set; }
       public string Nama { get; set; }
        public int Jumlah { get; set; }
        public int Cluster { get; internal set; }
    }
}
