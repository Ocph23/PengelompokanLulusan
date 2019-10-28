using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainApp.DataAccess.Models;
using Ocph.DAL;
using Ocph.DAL.Repository;

namespace MainApp
{
    public class DbContext : Ocph.DAL.Provider.MySql.MySqlDbConnection
    {
        private string constring = "Server=localhost;uid=root;database=kmean;password=";
        public DbContext()
        {
            this.ConnectionString = constring;
        }

        public IRepository<Jurusan> Jurusan { get { return new Repository<Jurusan>(this); } }

        public IRepository<Fakultas> Fakultas { get { return new Repository<Fakultas>(this); } }
        public IRepository<ProgramStudi> Progdi { get { return new Repository<ProgramStudi>(this); } }

        public IRepository<User> User { get { return new Repository<User>(this); } }

        public IRepository<Mahasiswa> Mahasiswa{ get { return new Repository<Mahasiswa>(this); } }

    }
}
