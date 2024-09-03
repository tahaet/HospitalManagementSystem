﻿using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DataAccess.Repository.IRepository
{

    public interface IVendorRepository : IRepository<Vendor>
    {
        void Update(Vendor model);
    }
}
