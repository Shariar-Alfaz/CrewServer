using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterFaces
{
    public interface ICommonRepository<T>
    {
        Task<List<T?>> GetAll();
        Task<T?> Save(T entity);
        Task<bool> Delete(int id);
        Task<T> Update (T entity);

    }
}
