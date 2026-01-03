using Microsoft.EntityFrameworkCore.Storage;
using SMT.Application.Interfaces;
using SMT.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SMTDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(SMTDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            await _transaction!.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction!.RollbackAsync();
        }
    }
}
