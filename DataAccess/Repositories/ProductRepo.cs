using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public abstract class ProductRepoBase : RepoBase<Product>
    {
        protected ProductRepoBase(ETradeContext dbContext) : base(dbContext)
        {
        }
    }

    public class ProductRepo : ProductRepoBase
    {
        public ProductRepo(ETradeContext dbContext) : base(dbContext)
        {
        }
    }
}
