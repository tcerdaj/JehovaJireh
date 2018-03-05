using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using JehovaJireh.Exception;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace JehovaJireh.Infrastructure.Installers
{
	public class ServicesInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Component.For<JehovaJireh.Logging.ILogger>()
				 .UsingFactoryMethod(_ => new Logging.NLogLogger(Logging.NLogLogger.GetConfiguration()))
				 );
			//container.Register(
			//	Component.For<IEmail>()
			//	 .UsingFactoryMethod(_ => new Email())
			//	 );
			container.Register(
				Component.For<ExceptionManager>()
				.UsingFactoryMethod(_ => ExceptionPolicies.AssigPolicyToExManager(container.Resolve<ILogger>()))
				);

		}
	}
}
