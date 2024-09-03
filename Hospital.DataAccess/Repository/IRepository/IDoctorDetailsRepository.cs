using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DataAccess.Repository.IRepository
{
    public interface IDoctorDetailsRepository : IRepository<DoctorDetails>
    {
        void Update(DoctorDetails model);
    }
}
