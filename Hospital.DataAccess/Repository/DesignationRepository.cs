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
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
    {
        private readonly AppDbContext _db;

        public DesignationRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Designation model)
        {
            _db.Update(model);
        }
    }
}
