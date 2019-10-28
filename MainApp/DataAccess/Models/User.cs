using Ocph.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess.Models
{

    [TableName("user")]
    public class User : BaseNotify
    {

        private string userName;
        private string password;
        private string nama;
        private int idUser;

        [PrimaryKey("IdUser")]
        [DbColumn("idUser")]
        public int IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }

        [DbColumn("nama")]
        public string Nama
        {
            get { return nama; }
            set { SetProperty(ref nama, value); }
        }

        [DbColumn("userName")]
        public string UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        [DbColumn("password")]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }


    }
}
