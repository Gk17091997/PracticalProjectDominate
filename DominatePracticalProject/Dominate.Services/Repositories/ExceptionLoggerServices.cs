using Dominate.Data.Interface;
using Dominate.Data.Model;
using Dominate.Services.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Services.Repositories
{
    public class ExceptionLoggerServices : IExceptionLoggerServices
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IRepository<Exceptions> _repository;

        public ExceptionLoggerServices(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
            _repository = _unitofWork.GetRepository<Exceptions>();
        }
        public void InsertErrorLog(Exceptions exceptions)
        {
            _repository.Add(exceptions);
            _unitofWork.commit();
        }
    }
}
