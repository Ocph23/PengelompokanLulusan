using FontAwesome.WPF;
using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess.Models
{
    [TableName("Fakultas")]
    public class Fakultas : BaseNotify,  IItem
    {
        private int idFakultas;
        private string namaFakultas;
        private string kodeFakultas;

        [PrimaryKey("IdFakultas")]
        [DbColumn("IdFakultas")]
        public int Id
        {
            get { return idFakultas; }
            set { idFakultas = value; }
        }

        [DbColumn("NamaFakultas")]
        public string Name
        {
            get { return namaFakultas; }
            set { SetProperty(ref namaFakultas, value); }
        }


        [DbColumn("KodeFakultas")]
        public string Kode
        {
            get { return kodeFakultas; }
            set { SetProperty(ref kodeFakultas, value); }
        }

        public int Nomor { get; set; }
        public int ParentId { get; set; }
        public Type type { get { return typeof(Fakultas); } }
        public List<IItem> Children { get; set; } = new List<IItem>();

        public FontAwesomeIcon Icon
        {
            get
            {
                return FontAwesomeIcon.University;
            }
        }

        public string Jenjang { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
