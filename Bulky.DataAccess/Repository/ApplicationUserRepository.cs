using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyWeb.DataAccess.Data;
using System.Linq.Expressions;

namespace Bulky.DataAccess.Repository
{
    //doing this allows us to only implement the methods we want extra and not everything from Repository + CategoryRepository - only CategoryRepository
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
