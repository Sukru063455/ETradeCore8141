using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public abstract class StoreRepoBase : RepoBase<Store>
    {
        public StoreRepoBase(ETradeContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<Store> Query(params Expression<Func<Store, object>>[] entitiesToInclude)
        {
            return base.Query(entitiesToInclude).Where(q => !q.IsDeleted);
        }

        public override void Delete(Store entity, bool save = true)
        {
            entity.IsDeleted = true;
            base.Update(entity, save);
        }
    }

    public class StoreRepo : StoreRepoBase
    {
        public StoreRepo(ETradeContext dbContext) : base(dbContext)
        {
        }
    }
}
