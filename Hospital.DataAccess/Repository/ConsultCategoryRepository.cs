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
    public class ConsultCategoryRepository : Repository<ConsultCategory>, IConsultCategoryRepository
    {
        private readonly AppDbContext _db;

        public ConsultCategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ConsultCategory model)
        {
            _db.Update(model);
        }
    }
}
