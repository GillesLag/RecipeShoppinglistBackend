namespace RecipeShoppingList.Repostories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    public IEnumerable<T> GetAll();
    public T? GetById(int id);
    public void Delete(int id);
    public void Update(T entity);
    public void Add(T entity);
    public void SaveChanges();
}
