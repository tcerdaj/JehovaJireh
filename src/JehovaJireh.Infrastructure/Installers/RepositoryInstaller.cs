using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Data.Repositories;

namespace JehovaJireh.Infrastructure.Installers
{
	public class RepositoryInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
						        Component.For<IUserRepository>()
								.ImplementedBy<UserRepository>(),
								Component.For<IDonationRepository>()
								.ImplementedBy<DonationRepository>(),
                                Component.For<IRequestRepository>()
                                .ImplementedBy<RequestRepository>(),
                                Component.For<ISchedulerRepository>()
                                .ImplementedBy<SchedulerRepository>(),
                                Component.For<IDonationDetailsRepository>()
                                .ImplementedBy<DonationDetailsRepository>()
                            );
		}
	}
}

