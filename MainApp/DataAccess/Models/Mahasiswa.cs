using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess.Models
{

    public delegate void delegateChangeCentroid( bool isCentorid);
    [TableName("mahasiswa")]
    public class Mahasiswa : BaseNotify, ICloneable
    {
        public event delegateChangeCentroid OnChangeCentroid;

        private int idMahasiswa;
        private string npm;
        private int suku;
        private double ipk;
        private double hasilTest;
        private int gelombang;
        private int idProgdi;
        private int idJurusan;
        private double masaStudi;
        private string nama;
        private Jurusan jurusan;

        [PrimaryKey("idMahasiswa")]
        [DbColumn("idMahasiswa")]
        public int IDMahasiswa
        {
            get { return idMahasiswa; }
            set { idMahasiswa = value; }
        }

        [DbColumn("npm")]
        public string NPM
        {
            get { return npm; }
            set { SetProperty(ref npm, value); }
        }

        [DbColumn("nama")]
        public string Nama
        {
            get { return nama; }
            set { SetProperty(ref nama, value); }
        }

        [DbColumn("suku")]
        public int Suku
        {
            get { return suku; }
            set { SetProperty(ref suku, value); }
        }

        [DbColumn("ipk")]
        public double IPK
        {
            get { return ipk; }
            set { SetProperty(ref ipk, value); }
        }

        [DbColumn("hasilTest")]
        public double HasilTest
        {
            get { return hasilTest; }
            set { SetProperty(ref hasilTest, value); HasilTestText = value.ToString(); }
        }
        [DbColumn("masaStudi")]
        public double MasaStudi
        {
            get { return masaStudi; }
            set { SetProperty(ref masaStudi, value); MasaStudiText = value.ToString(); }
        }

        [DbColumn("Gelombang")]
        public int Gelombang
        {
            get { return gelombang; }
            set { SetProperty(ref gelombang, value); }
        }

        [DbColumn("ProgdiId")]
        public int IdProgdi
        {
            get { return idProgdi; }
            set { SetProperty(ref idProgdi, value); }
        }

        [DbColumn("jurusanId")]
        public int IdJurusan
        {
            get { return idJurusan; }
            set { SetProperty(ref idJurusan, value); }
        }

        private int nomor;

        public int Nomor
        {
            get { return nomor; }
            set {SetProperty(ref nomor ,value); }
        }


        private bool isCentroid;

        public bool IsCentroid
        {
            get { return isCentroid; }
            set {
                SetProperty(ref isCentroid ,value);
                ChangeCentroid(value);
            }
        }

        private Gelombang gelombangModel;

        public Gelombang GelombangModel
        {
            get { return gelombangModel; }
            set {SetProperty(ref gelombangModel ,value); }
        }


        private Suku sukuModel;

        public Suku SukuModel
        {
            get { return sukuModel; }
            set { SetProperty(ref sukuModel, value); }
        }



        private void ChangeCentroid(bool value)
        {
            if (OnChangeCentroid != null)
            {
                OnChangeCentroid(value);
            }
        }

        public Jurusan Jurusan
        { get { return jurusan; }
            set { SetProperty(ref jurusan, value); } }

        private int clustering;

        public int Clustering
        {
            get { return clustering; }
            set { SetProperty(ref clustering , value); }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }




        ///Initialisasi
        ///

        private string hasilTestText;

        public string HasilTestText
        {
            get {
                if (HasilTest >= 0)
                    hasilTestText = "";
                hasilTestText = HasilTest <= 3 ? "Kurang" : HasilTest <= 5 ? "Cukup" : HasilTest <= 7 ? "Baik" : HasilTest <= 8 ? "Cukup Baik" : "Sempurna";
                return hasilTestText; }
            set { SetProperty(ref hasilTestText , value); }
        }

        private string masaStudiText;
        private string ipkText;

        public string MasaStudiText
        {
            get
            {
                if (MasaStudi >= 0)
                    masaStudiText = "";
                masaStudiText = MasaStudi <= 4.5 ? "Tepat Waktu" : MasaStudi <= 5.5 ? "Cukup Tepat Waktu" : "Tidak Tepat Waktu";
                return masaStudiText;
            }
            set { SetProperty(ref masaStudiText, value); }
        }


        public string IPKText
        {
            get
            {
                if (IPK >= 0)
                    ipkText = "";
                ipkText = IPK <= 2.75 ? "Memuaskan" : IPK <= 3.5 ? "Sangat Memuaskan" : "Terpuji";
                return ipkText;
            }
            set { SetProperty(ref ipkText, value); }
        }



        public string Angkatan
        {
            get
            {
                if (!string.IsNullOrEmpty(NPM) && NPM.Length>=4)
                {
                    return NPM.Substring(0,4);
                }
                return string.Empty;
            }
        }

        public int FakultasId { get; internal set; }
    }
}
