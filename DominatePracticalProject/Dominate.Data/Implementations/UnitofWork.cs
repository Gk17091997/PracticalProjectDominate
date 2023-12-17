using Dominate.Data.Context;
using Dominate.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.Implementations
{
    public class UnitofWork : IUnitofWork
    {
        private readonly UserDbContext dbcontext;
        private Dictionary<Type, object> repositories;

        public UnitofWork(UserDbContext context)
        {
            dbcontext = context;
        }
        public async Task<int> commit()
        {
          await  dbcontext.SaveChangesAsync();
            return 1;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            repositories ??= new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(dbcontext);
            }
            return (IRepository<TEntity>)repositories[type];
        }
    }
}
