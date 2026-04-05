using System.Threading;
using System.Threading.Tasks;

namespace Animatch.Services
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
