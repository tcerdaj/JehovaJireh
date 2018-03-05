using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Data.Listeners;
using NHibernate;
using NHibernate.Event;

namespace JehovaJireh.Data
{
	public class NHUnitOfWork: IUnitOfWork
	{
		private readonly ISession _session;
		private ITransaction _transaction;
		public NHUnitOfWork(ISession session)
		{
			_session = session;
		}
		public void Dispose()
		{
			if (_transaction != null)
				_transaction.Rollback();
		}

		public void Commit()
		{
			if (_transaction == null)
				throw new InvalidOperationException("UnitOfWork has already been saved.");

			if (!_transaction.IsActive)
			{
				throw new InvalidOperationException("Oops! We don't have an active transaction");
			}

			_transaction.Commit();
		}

		public void Rollback()
		{
			if (_transaction.IsActive)
			{
				_transaction.Rollback();
			}
		}

		public void Begin()
		{
			_transaction = _session.BeginTransaction();
		}
        
        protected static IPersistenceConfigurer SetupDatabase()
        {
            var connectinString = JehovaJireh.Configuration.CloudConfiguration.GetConnectionString("SQLConnectionString");
            return MsSqlConfiguration.MsSql2008
                .UseOuterJoin()
                .ConnectionString(x => x.Is(connectinString))
                .ShowSql();
        }
        public static ISessionFactory BuildSessionFactory()
        {
            NHibernate.Cfg.Configuration cfg = Fluently.Configure()
                .Database(SetupDatabase)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHUnitOfWork>())
                .ExposeConfiguration(c =>
                {
                    c.SetProperty(@"nhibernate-logger", @"JehovaJireh.Infrastructure.Logging.NLogNHibernateFactory, JehovaJireh.Infrastructure"); //TODO: WHY DOESNT THIS WORK?
                })
                .BuildConfiguration();
            cfg.EventListeners.PreUpdateEventListeners =
                new IPreUpdateEventListener[] { new AuditEventListener() };
            cfg.EventListeners.PreInsertEventListeners =
                new IPreInsertEventListener[] { new AuditEventListener() };
            return cfg.BuildSessionFactory();
        }

    }
}
