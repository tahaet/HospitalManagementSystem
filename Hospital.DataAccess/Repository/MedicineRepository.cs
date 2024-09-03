using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DataAccess.Repository
{
    public class MedicineRepository : Repository<Medicine>, IMedicineRepository
    {
        private readonly AppDbContext _db;

        public MedicineRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Medicine model)
        {
            _db.Update(model);
        }
    }
}
