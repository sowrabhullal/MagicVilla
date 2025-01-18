using MagicVillaWebApi.Data;
using MagicVillaWebApi.Migrations;
using MagicVillaWebApi.Models;
using MagicVillaWebApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVillaWebApi.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        ApplicationDbContext _db;

        public VillaRepository(ApplicationDbContext db):base(db) {
            _db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.villas.Update(entity);
            await SaveAsync();
            return entity; 
        }
    }
}
