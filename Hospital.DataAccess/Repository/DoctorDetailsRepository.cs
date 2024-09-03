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
    public class DoctorDetailsRepository : Repository<DoctorDetails>, IDoctorDetailsRepository
    {
        private readonly AppDbContext _db;

        public DoctorDetailsRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(DoctorDetails model)
        {
            _db.Update(model);
        }
    }
}
