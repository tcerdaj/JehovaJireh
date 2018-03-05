using JehovaJireh.Core.Entities;
using JehovaJireh.Core.EntitiesDto;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Data.Mappings;
using JehovaJireh.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Omu.ValueInjecter;
using NHibernate;
using NHibernate.Linq;

namespace JehovaJireh.Web.Services.Controllers
{
    public class SchedulerController : BaseController<Scheduler, SchedulerDto, int>
    {
        ISchedulerRepository repository;
        IDonationDetailsRepository donationDetailsRepository;
        ILogger log;
        ISession session;
        IDonationRepository donationRepository;

        public SchedulerController(ISchedulerRepository repository, ILogger log, ISession session, IDonationDetailsRepository donationDetailsRepository, IDonationRepository donationRepository)
			:base(repository, log)
        {
            this.log = log;
            this.session = session;
            this.repository = repository;
            this.donationRepository = donationRepository;
            this.donationDetailsRepository = donationDetailsRepository;
        }

        [ActionName("CreatedBy")]
        [HttpGet]
        public IEnumerable<SchedulerDto> CreatedBy(int id)
        {
            IEnumerable<SchedulerDto> dto;
            try
            {
                var dta = repository.Query().Where(x => x.CreatedBy.Id == id )
                                     .ToList();
                dto = dta.Select(x => new SchedulerDto()
                                .InjectFrom<DeepCloneInjection>(x))
                                .Cast<SchedulerDto>()
                                .ToList();
            }
            catch (System.Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

            return dto;
        }

        public override IHttpActionResult Post(SchedulerDto dto)
        {
            try
            {
                Scheduler data = new Scheduler();
                data.InjectFrom<DeepCloneInjection>(dto);

                if (dto.Item != null && dto.Item.Id == Guid.Empty)
                    data.Item = null;

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




    }
}
