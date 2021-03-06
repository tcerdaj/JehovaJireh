﻿using JehovaJireh.Core.Entities;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Data.Repositories
{
    public class DonationDetailsRepository: NHRepository<DonationDetails,Guid>, IDonationDetailsRepository
    {
        public DonationDetailsRepository(ISession session, ExceptionManager exManager, ILogger log)
            :base(session, exManager, log)
        {
                
        }
    }
}
