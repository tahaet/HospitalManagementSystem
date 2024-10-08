﻿using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DataAccess.Repository.IRepository
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        void Update(Expense model);
        Task<bool> UpdatePaymentStatus(int id , string PaymentStatus);
    }
}
