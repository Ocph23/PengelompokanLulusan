using FontAwesome.WPF;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess.Models
{
    [TableName("progdi")]
    public class ProgramStudi : BaseNotify,IItem
    {
        [PrimaryKey("idProgdi")]
        [DbColumn("idProgdi")]
        public int Id
        {
            get { return idProgdi; }
            set { SetProperty(ref idProgdi,value); }
        }

        [DbColumn("FakultasId")]
        public int ParentId
        {
            get { return fakultasId; }
            set { SetProperty(ref fakultasId, value); }
        }


        private string kodeProgdi;

        [DbColumn("kodeProgdi")]
        public string Kode
        {
            get { return kodeProgdi; }
            set { SetProperty(ref kodeProgdi, value); }
        }


        [DbColumn("namaProgdi")]
        public string Name
        {
            get { return namaProgdi; }
            set { SetProperty(ref namaProgdi, value); }
        }

        [DbColumn("jenjang")]
        public string Jenjang
        {
            get { return jenjang; }
            set { SetProperty(ref jenjang, value); }
        }

        public FontAwesomeIcon Icon
        {
            get
            {
                return FontAwesomeIcon.UniversalAccess;
            }
        }

        public int Nomor { get; set; }
        public List<IItem> Children { get; set; } = new List<IItem>();

        public Type type { get { return typeof(ProgramStudi); } }

        private int idProgdi;
        private string namaProgdi;
        private string jenjang;
        private int fakultasId;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
