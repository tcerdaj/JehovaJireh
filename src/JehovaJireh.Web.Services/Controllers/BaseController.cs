using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Data.Mappings;
using JehovaJireh.Logging;
using Omu.ValueInjecter;

namespace JehovaJireh.Web.Services.Controllers
{
    public class BaseController<T,DTO,IdT> : ApiController where T: new() where DTO: new()
    {
		private readonly IRepository<T, IdT> repository;
		private readonly ILogger log;

        public BaseController()
        {
           
        }

		public BaseController(IRepository<T, IdT> repository, ILogger log)
		{
			this.repository = repository;
			this.log = log;
		}

		public IRepository<T, IdT> Repository
		{
			get { return repository; }
		}

        // GET api/baseapi      
        [ActionName("Get")]
        public virtual IEnumerable<DTO> Get()
		{
			IEnumerable<DTO> dto;
			try
			{
				var dta = repository.Query().ToList();
				dto = dta.Select(x => new DTO().InjectFrom<DeepCloneInjection>(x)).Cast<DTO>().ToList();// Mapper.Map<IEnumerable<DTO>>(dta);
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}

			return dto;
		}

		public IEnumerable<DTO> Get(int take, int skip)
		{
			IEnumerable<DTO> dto;
			try
			{
				var dta = repository.Query().Take(take).Skip(skip).ToList();
				dto = dta.Select(x => new DTO().InjectFrom<DeepCloneInjection>(x)).Cast<DTO>().ToList();// Mapper.Map<IEnumerable<DTO>>(dta);

			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}

			return dto;
		}

        //get by id
        [ActionName("Get")]
        public DTO Get(IdT id)
		{
			DTO dto = new DTO();
			try
			{
				var dta = repository.GetById(id);
                if (dta != null)
                    dto = (DTO)dto.InjectFrom<DeepCloneInjection>(dta);
                else
                    dto = default(DTO);
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}

			return dto;
		}

		// POST: api/baseapi
		public virtual IHttpActionResult Post(DTO dto)
		{
			try
			{
				T data = new T();
				data.InjectFrom<DeepCloneInjection>(dto);
				repository.Create(data);
                dto.InjectFrom<DeepCloneInjection>(data);
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}

			return Ok(dto);
		}

        // PUT: api/baseapi
        [ActionName("Put")]
        public IHttpActionResult Put(IdT id, DTO dto)
		{
			try
			{
				T data = repository.GetById(id);
				data.InjectFrom<DeepCloneInjection>(dto);
				repository.Update(data);
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}
			return Ok(dto);
		}

		// DELETE: api/baseapi
		public IHttpActionResult Delete(IdT id)
		{
			try
			{
                var entity = repository.GetById(id);
                repository.Delete(entity);
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}
			return Ok(id);
		}

		// DELETE: api/baseapi
		public IHttpActionResult Delete(DTO dto)
		{
			try
			{
				T data = new T();
				data.InjectFrom<DeepCloneInjection>(dto);
				repository.Delete(data);
			}
			catch (System.Exception ex)
			{
				log.Error(ex);
				throw ex;
			}
			return Ok(dto);
		}
	}
}
