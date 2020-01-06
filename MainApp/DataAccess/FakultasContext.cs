using MainApp.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.DataAccess
{
    public class FakultasContext
    {
        internal List<Fakultas> GetFakultas()
        {
            using (var db = new DbContext())
            {
                var fakultases = db.Fakultas.Select().ToList();
                foreach(var fakultas in fakultases)
                {
                    fakultas.Children = db.Progdi.Where(x => x.ParentId == fakultas.Id).ToList<IItem>();
                }
                return fakultases;

            }
        }

        internal void DeleteFaculty(IItem selectedItem)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var deleted = db.Fakultas.Delete(x => x.Id == selectedItem.Id);
                    if(!deleted)
                    {
                        throw new SystemException("Data Tidak Berhasil Dihapus");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        internal void DeleteProgdi(IItem selectedItem)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var deleted = db.Progdi.Delete(x => x.Id == selectedItem.Id);
                    if (!deleted)
                    {
                        throw new SystemException("Data Tidak Berhasil Dihapus");
                    }
                }
            }
            catch (Exception ex)
            {
               
                throw ex;

            }
        }

        internal void EditFaculty(IItem selectedItem)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var model = new Fakultas { Id = selectedItem.Id, Kode = selectedItem.Kode, Name = selectedItem.Name };
                    var updated = db.Fakultas.Update(x => new { x.Kode, x.Name }, model, x => x.Id == selectedItem.Id);
                    if (!updated)
                    {
                        throw new SystemException("Data Tidak Berhasil Diubah");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void EditProgdi(IItem selectedItem)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var model = new ProgramStudi { Id = selectedItem.Id, Kode = selectedItem.Kode, Name = selectedItem.Name };
                    var updated = db.Progdi.Update(x => new { x.Kode,x.Name,x.Jenjang},model,x=>x.Id == selectedItem.Id);
                    if (!updated)
                    {
                        throw new SystemException("Data Tidak Berhasil Diubah");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
