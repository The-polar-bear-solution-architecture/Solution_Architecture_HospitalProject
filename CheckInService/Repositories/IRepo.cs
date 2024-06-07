using System.Collections.Generic;

namespace CheckInService.Repositories
{
    public interface IRepo<T>
    {
        IEnumerable<T> Get();
        T? Get(int id);
        void Post(T entity);
        void Put(T entity);
    }
}
