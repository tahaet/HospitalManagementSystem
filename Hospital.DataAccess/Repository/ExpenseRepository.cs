using Hospital.DataAccess.Data;
using Hospital.DataAccess.Repository.IRepository;
using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DataAccess.Repository
{
    public class ExpenseRepository : Repository<Expense>, IExpenseRepository
    {
        private readonly AppDbContext _db;

        public ExpenseRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Expense model)
        {
            _db.Update(model);
        }

        public async Task<bool> UpdatePaymentStatus(int id, string PaymentStatus)
        {
            var expense = await _db.Expenses.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
            if (expense != null)
            {
                if(!string.IsNullOrEmpty(PaymentStatus))
                {
                    expense.PaymentStatus = PaymentStatus;
                    return true;
                }
            }
                
            return false;
        }
    }
}
