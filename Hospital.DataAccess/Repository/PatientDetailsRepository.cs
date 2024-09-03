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
    public class PatientDetailsRepository : Repository<PatientDetails>, IPatientDetailsRepository
    {
        private readonly AppDbContext _db;

        public PatientDetailsRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(PatientDetails model)
        {
            _db.Update(model);
        }
    }
}
