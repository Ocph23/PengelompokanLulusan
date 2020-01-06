using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess.Models
{
    [TableName("jurusan")]
   public class Jurusan:BaseNotify,ICloneable
    {

        private int idJurusan;
        private string namaJurusan;
        private string kodeJurusan;
        private int progdiId;

        [PrimaryKey("idJurusan")]
        [DbColumn("idJurusan")]
        public int Id
        {
            get { return idJurusan; }
            set { idJurusan = value; }
        }

       /* [DbColumn("ProgdiId")]
        public int ParentId
        {
            get { return progdiId; }
            set { SetProperty(ref progdiId, value); }
        }*/


        [DbColumn("namaJurusan")]
        public string Name
        {
            get { return namaJurusan; }
            set { SetProperty(ref  namaJurusan, value); }
        }


        [DbColumn("kodeJurusan")]
        public string Kode
        {
            get { return kodeJurusan; }
            set { SetProperty(ref kodeJurusan,value); }
        }

        public int Nomor { get; set; }
        public int Initial { get;  set; }


        public Type tye { get { return typeof(Jurusan); } }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
