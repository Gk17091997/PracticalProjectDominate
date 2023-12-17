using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.Interface
{
    public interface IUnitofWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task<int> commit();
    }
}
