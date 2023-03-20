using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public interface ICategoryService : IService<CategoryModel>
    {
        Task<List<CategoryModel>> GetListAsync();
    }

    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepoBase _categoryRepo;

        public CategoryService(CategoryRepoBase categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public Result Add(CategoryModel model)
        {
            if (_categoryRepo.Exists(c => c.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("Category with same name exists!");
            Category entity = new Category()
            {
                Description = model.Description?.Trim(),
                Name = model.Name.Trim()
            };
            _categoryRepo.Add(entity);
            return new SuccessResult("Category added successfully.");
        }

        public Result Delete(int id)
        {
            //CategoryModel existingCategory = Query().SingleOrDefault(c => c.Id == id);
            //if (existingCategory == null) 
            //{
            //    return new ErrorResult("Category not found!");
            //}
            //if (existingCategory.ProductCountDisplay > 0)
            //{
            //    return new ErrorResult("Category cannot be deleted because category contains products!");
            //}
            //_categoryRepo.Delete(id);
            //return new SuccessResult("Category deleted successfully.");
            Category existingCategory = _categoryRepo.Query(c => c.Products).SingleOrDefault(c => c.Id == id);
            if (existingCategory != null && existingCategory.Products.Count > 0)
                return new ErrorResult("Category cannot be deleted because category contains products!");
            _categoryRepo.Delete(id);
            return new SuccessResult("Category deleted successfully.");
        }

        public void Dispose()
        {
            _categoryRepo.Dispose();
        }

        public async Task<List<CategoryModel>> GetListAsync()
        {
            List<CategoryModel> categories = await Query().ToListAsync();
            return categories;
        }

        public IQueryable<CategoryModel> Query()
        {
            return _categoryRepo.Query(c => c.Products).OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Description = c.Description,
                Guid = c.Guid,
                Id = c.Id,
                Name = c.Name,
                
                ProductCountDisplay = c.Products.Count
            });
        }

        public Result Update(CategoryModel model)
        {
            if (_categoryRepo.Exists(c => c.Name.ToLower() == model.Name.ToLower().Trim() && c.Id != model.Id))
                return new ErrorResult("Category with same name exists!");
            Category entity = new Category()
            {
                Id = model.Id,
                Description = model.Description?.Trim(),
                Name = model.Name.Trim()
            };
            _categoryRepo.Update(entity);
            return new SuccessResult("Category updated successfully.");
        }
    }
}
