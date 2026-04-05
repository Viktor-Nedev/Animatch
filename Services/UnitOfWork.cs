using Animatch.Data;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AnimalManagerDbContext context;

        public UnitOfWork(AnimalManagerDbContext context)
        {
            this.context = context;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
