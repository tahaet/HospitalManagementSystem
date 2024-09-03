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
    public class TestResultRepository : Repository<TestResult>, ITestResultRepository
    {
        private readonly AppDbContext _db;

        public TestResultRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TestResult model)
        {
            _db.Update(model);
        }
    }

}
