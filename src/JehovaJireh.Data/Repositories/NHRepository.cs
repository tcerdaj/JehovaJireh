using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Exception;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;
using NHibernate.Linq;

namespace JehovaJireh.Data.Repositories
{
	public class NHRepository<T, IdT> : IRepository<T, IdT>
	{
		private readonly ILogger log;
		private ISession session;
		private readonly ExceptionManager exManager;
        private ISessionFactory sessionFactory;

        public ISession Session
		{
			get
            {
                session = !session.IsOpen ? sessionFactory.OpenSession(): session;
                return session;
            }
		}

		public NHRepository(ISession session, ExceptionManager exManager, ILogger log)
		{
            this.session = session;
			this.exManager = exManager;
			this.log = log;
            this.sessionFactory = NHUnitOfWork.BuildSessionFactory();
        }

		public NHRepository(ISession session)
		{
			this.session = session;
		}

		public NHRepository()
		{

		}

		public IQueryable<T> Query()
		{
			IQueryable<T> data = null;
			Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                data = Session.Query<T>();
                //foreach (var obj in data)
                //    Session.Evict(obj);
            }
            catch (System.Exception ex)
            {
                HandleException(ex);
            }

			return data;
		}


		public void HandleException(System.Exception ex)
		{
			System.Exception exceptionToThrow;
			System.Exception results;
			if (exManager.HandleException(ex, ExceptionPolicies.DataAccess.WebServicePolicy, out exceptionToThrow))
			{
				if (exceptionToThrow == null)
					results =  ex;
				else
					results = exceptionToThrow;
			}
			else
			{
				results =  ex;
			}

			log.Error(ex);
			throw results;
		}

		public T GetById(IdT Id)
		{
			object item = new object();
			Stopwatch timespan = Stopwatch.StartNew();
			//log.GetStarted(Id);

			try
			{
				item = Session.Get<T>(Id);
                Session.Evict(item);
            }
			catch (System.Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
                timespan.Stop();
                //log.GetFinished(Id.ToString(), timespan.Elapsed);
            }
			return (T)item;
		}

		public void Create(T entity)
		{
			//log.SaveInsertStarted(entity);
			
			try
			{
				using (var tx = Session.BeginTransaction())
				{
                    Session.Save(entity);
                    tx.Commit();
                }
			}
			catch (System.Exception ex)
			{
				HandleException(ex);
			}
			finally
			{

                //log.SaveInsertFinished(entity, timespan.Elapsed);
            }
		}

		public void Delete(T entity)
		{
			//log.DeleteStarted(entity);
			try
			{
				using (var tx = Session.BeginTransaction())
				{
                    Session.Delete(entity);
                    tx.Commit();
                }
			}
			catch (System.Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
                //log.DeleteFinished(entity);
            }
		}

		public void DeleteById(IdT Id)
		{
			//log.DeleteStarted(Session.Load<T>(Id));
			Stopwatch timespan = Stopwatch.StartNew();
            var entity = Session.Load<T>(Id);

            try
			{
                using (var tx = Session.BeginTransaction())
				{
                   Session.Delete(entity);
                   tx.Commit();
                };
			}
			catch (System.Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
                timespan.Stop();
				//log.DeleteFinished(entity);
			}
		}

		public void Dispose()
		{
           Session.Dispose();
        }

        public void Update(T entity)
		{
			//log.SaveStarted(entity);
			Stopwatch timespan = Stopwatch.StartNew();
			try
			{
				using (var tx = Session.BeginTransaction())
				{
                    Session.SaveOrUpdate(entity);
					tx.Commit();
                    //log.SaveFinished(entity);
                }
			}
			catch (System.Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
                timespan.Stop();
				//log.SaveFinished(entity);
			}
		}


		public T GetReferenceById(IdT Id)
		{
			object item = new object();

            try
            {
                item = Session.Load<T>(Id);
            }
            catch (System.Exception ex)
            {
                HandleException(ex);
            }

            return (T)item;
		}

        /// <summary>
        /// Flushes the current active NHibernate session.
        /// </summary>
        //public void FlushSession()
        //{
        //    if (Session != null && this.Session.IsOpen)
        //    {
        //        this.Session.Flush();
        //    }
        //}

        /// <summary>
        /// Close the active NHibernate session
        /// </summary>
        //public void CloseSession()
        //{
        //    if (this.Session != null)
        //    {
        //        if (this.Session.IsOpen)
        //        {
        //            this.Session.Close();
        //        }
        //        this.Session.Dispose();
        //    }
        //}

        //public void OpenSession()
        //{
        //    if (this.Session != null)
        //    {
        //        if (!this.Session.IsOpen)
        //        {
        //            this.Session.Connection.Open();
        //        }
        //    }
        //}
    }

   
}
