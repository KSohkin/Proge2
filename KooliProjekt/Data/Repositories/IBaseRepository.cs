namespace KooliProjekt.Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task Save(T instance);
        Task Delete(int id);
        Task Delete (T instance);
        Task<PagedResult<T>> List(int page, int pageSize);
    }
}