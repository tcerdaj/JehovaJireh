using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.IRepositories
{
	public interface IRepository<T, IdT>
	{
		T GetById(IdT Id);
		T GetReferenceById(IdT Id);
		void Update(T entity);
		void Create(T entity);
		void Delete(T entity);
		IQueryable<T> Query();
        //void CloseSession();
        //void FlushSession();
        //void OpenSession();
    }
}
