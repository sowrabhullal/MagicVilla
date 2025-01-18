using MagicVillaWebApi.Models;
using System.Linq.Expressions;

namespace MagicVillaWebApi.Repository.IRepository
{
    public interface IVillaRepository: IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}
