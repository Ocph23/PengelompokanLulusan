using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess
{
    public interface IItem: ICloneable
    {
        string Kode { get; set; }

        string Name { get; set; }

        int Id { get; set; }

        int ParentId { get; set; }

        Type type { get; }

        List<IItem> Children { get; set; } 

        int Nomor { get; set; }

        FontAwesomeIcon Icon { get; }
        string Jenjang { get; set; }
    }
}
