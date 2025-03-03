using Microsoft.EntityFrameworkCore;
using RecipeShoppingList.Data;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    ///TODO Make everything Async
    protected readonly RecipeContext _context;
    public GenericRepository(RecipeContext context)
    {
        _context = context;
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(int id)
    {
        var entity = _context.Set<T>().Find(id);

        if (entity != null)
        {
            _context.Set<T>().Remove(entity);
        }
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public virtual T? GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
