using AppCore.Records.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppCore.DataAccess.EntityFramework.Bases
{
    public abstract class RepoBase<TEntity> : IDisposable where TEntity : RecordBase, new()
    {
        protected readonly DbContext _dbContext;

        protected RepoBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // List<Urun> urunler = _repoBase.Query().ToList();
        // List<Urun> urunler = _repoBase.Query(urun => urun.Kategori).ToList();
        public virtual IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] entitiesToInclude) // read
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            foreach (var entityToInclude in entitiesToInclude)
            {
                query = query.Include(entityToInclude);
            }
            return query;
        }

        // List<Urun> urunler = _repoBase.Query(urun => urun.Adi.Contains("ASUS")).ToList();
        // List<Urun> urunler = _repoBase.Query(urun => urun.StokMiktari >= 10 && urun.StokMiktari <= 20, urun => urun.Kategori);
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] entitiesToInclude)
        {
            return Query(entitiesToInclude).Where(predicate);
        }

        public virtual IQueryable<TRelationalEntity> Query<TRelationalEntity>() where TRelationalEntity : class, new()
        {
            return _dbContext.Set<TRelationalEntity>().AsQueryable();
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().Any(predicate);
        }

        // Urun urun = new Urun() { Adi = "HP", ... };
        // _repoBase.Add(urun);
        // List<Urun> urunler = new List<Urun>()
        // {
        //      new Urun() { Adi = "HP", ... },
        //      new Urun() { Adi = "ASUS", ... }
        // };
        // foreach (Urun urun in urunler)
        // {
        //      _repoBase.Add(urun, false);
        // }
        // _repoBase.Save();
        public virtual void Add(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString();
            _dbContext.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }

        // Urun urun = new Urun() { Id = 1, Adi = "HP", ... }; // 1. yöntem
        // Urun urun = _repoBase.Query().SingleOrDefault(u => u.Id == 1); // 2. yöntem
        // urun.Adi = "HP"; // 2. yöntem
        // _repoBase.Update(urun);
        // List<Urun> urunler = new List<Urun>()
        // {
        //      new Urun() { Id = 1, Adi = "HP", ... },
        //      new Urun() { Id = 2, Adi = "ASUS", ... }
        // };
        // foreach (Urun urun in urunler)
        // {
        //      _repoBase.Update(urun, false);
        // }
        // _repoBase.Save();
        public virtual void Update(TEntity entity, bool save = true)
        {
            _dbContext.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }

        // Urun urun = _repoBase.Query().SingleOrDefault(u => u.Id == 1);
        // Urun urun = _repoBase.Query().Where(u => u.Id == 1).SingleOrDefault();
        // Urun urun = _repoBase.Query(u => u.Id == 1).SingleOrDefault();
        // _repoBase.Delete(urun);
        public virtual void Delete(TEntity entity, bool save = true)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (save)
                Save();
        }

        public virtual void Delete<TRelationalEntity>(Expression<Func<TRelationalEntity, bool>> predicate, bool save = false) where TRelationalEntity : class, new()
        {
            var relationalEntities = Query<TRelationalEntity>().Where(predicate).ToList();
            _dbContext.Set<TRelationalEntity>().RemoveRange(relationalEntities);
            if (save)
                Save();
        }

        // _repoBase.Delete(1);
        public virtual void Delete(int id, bool save = true)
        {
            //var entity = _dbContext.Set<TEntity>().Find(id);
            var entity = _dbContext.Set<TEntity>().SingleOrDefault(e => e.Id == id);
            Delete(entity, save);
        }

        // _repoBase.Delete(u => u.Id == 1);
        // _repoBase.Delete(u => u.ExpirationDate < DateTime.Now);
        public virtual void Delete(Expression<Func<TEntity, bool>> predicate, bool save = true)
        {
            var entities = Query(predicate).ToList();
            foreach (var entity in entities)
            {
                Delete(entity, false);
            }
            if (save)
                Save();
        }

        public virtual int Save()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                // exc üzerinden logalama
                throw exc;
            }
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
