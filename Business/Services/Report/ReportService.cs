using Business.Models.Report;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace Business.Services.Report
{
    public interface IReportService
    {
        List<ReportItemModel> GetReport(ReportFilterModel filter, bool useInnerJoin = false);
    }

    /*
    select * from products left outer join categories
    on products.categoryId = categories.Id
    left outer join ProductStores on productstores.ProductId = Products.Id
    left outer join Stores on productstores.storeId = stores.Id
    */
    public class ReportService : IReportService
    {
        private readonly ProductRepoBase _repo;

        public ReportService(ProductRepoBase repo)
        {
            _repo = repo;
        }

        public List<ReportItemModel> GetReport(ReportFilterModel filter, bool useInnerJoin = false)
        {
            var productQuery = _repo.Query();
            var categoryQuery = _repo.Query<Category>();
            var storeQuery = _repo.Query<Store>();
            var productStoreQuery = _repo.Query<ProductStore>();

            IQueryable<ReportItemModel> query = null;

            if (useInnerJoin)
            {
                query = from p in productQuery
                        join c in categoryQuery
                        on p.CategoryId equals c.Id
                        join ps in productStoreQuery
                        on p.Id equals ps.ProductId
                        join s in storeQuery
                        on ps.StoreId equals s.Id
                        //where p.Id == 5 || (p.StockAmount >= 10 && p.StockAmount <= 20)
                        //orderby s.Name descending, c.Name, p.Name descending
                        select new ReportItemModel()
                        {
                            CategoryDescription = c.Description,
                            CategoryName = c.Name,
                            ExpirationDate = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            StockAmount = p.StockAmount + " piece(s)",
                            StoreName = s.Name,
                            UnitPrice = p.UnitPrice.ToString("C2"),

                            CategoryId = c.Id,
                            ExpirationDateValue = p.ExpirationDate,
                            StoreId = s.Id
                        };
            }
            else // left outer join
            {
                query = from p in productQuery
                        join c in categoryQuery
                        on p.CategoryId equals c.Id into categoryJoin
                        from category in categoryJoin.DefaultIfEmpty()
                        join ps in productStoreQuery
                        on p.Id equals ps.ProductId into productStoreJoin
                        from productStore in productStoreJoin.DefaultIfEmpty()
                        join s in storeQuery
                        on productStore.StoreId equals s.Id into storeJoin
                        from store in storeJoin.DefaultIfEmpty()
                        select new ReportItemModel()
                        {
                            CategoryDescription = category.Description,
                            CategoryName = category.Name,
                            ExpirationDate = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            StockAmount = $"{p.StockAmount} piece(s)",
                            StoreName = store.Name,
                            UnitPrice = p.UnitPrice.ToString("C2"),

                            CategoryId = category.Id,
                            ExpirationDateValue = p.ExpirationDate,
                            StoreId = store.Id
                        };
            }

            query = query.OrderBy(q => q.StoreName).ThenBy(q => q.CategoryName).ThenBy(q => q.ProductName);

            if (filter is not null)
            {
                if (filter.CategoryId.HasValue)
                    query = query.Where(q => q.CategoryId == filter.CategoryId);
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                    query = query.Where(q => q.ProductName.ToLower().Contains(filter.ProductName.ToLower().Trim()));
                if (filter.ExpirationDateBegin.HasValue)
                    query = query.Where(q => q.ExpirationDateValue >= filter.ExpirationDateBegin);
                if (filter.ExpirationDateEnd.HasValue)
                    query = query.Where(q => q.ExpirationDateValue <= filter.ExpirationDateEnd);
                if (filter.StoreIds is not null && filter.StoreIds.Count > 0)
                    query = query.Where(q => filter.StoreIds.Contains(q.StoreId ?? 0));
            }
            return query.ToList();
        }
    }
}
