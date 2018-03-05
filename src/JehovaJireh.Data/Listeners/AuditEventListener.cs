using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JehovaJireh.Core.Entities;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Persister.Entity;

namespace JehovaJireh.Data.Listeners
{
	public class AuditEventListener : IPreUpdateEventListener, IPreInsertEventListener
	{
		private static readonly string ModifiedOnPropertyName = GetPropertyName<IEntityTimestamp>(val => val.ModifiedOn),
									   createdOnPropertyName = GetPropertyName<IEntityTimestamp>(val => val.CreatedOn);

		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			var audit = @event.Entity as IEntityTimestamp;
			if (audit == null)
				return false;

			var currentDate = DateTime.UtcNow;
			audit.ModifiedOn = currentDate;
			SetState(@event.Persister, @event.State, ModifiedOnPropertyName, currentDate);

			return false;
		}

		public bool OnPreInsert(PreInsertEvent @event)
		{
			var audit = @event.Entity as IEntityTimestamp;
			if (audit == null)
				return false;

			var currentDate = DateTime.UtcNow;
			audit.CreatedOn = audit.ModifiedOn = currentDate;
			SetState(@event.Persister, @event.State, ModifiedOnPropertyName, currentDate);
			SetState(@event.Persister, @event.State, createdOnPropertyName, currentDate);
			
			return false;
		}

		private void SetState(IEntityPersister persister, object[] state, string propertyName, object value)
		{
			var index = Array.IndexOf(persister.PropertyNames, propertyName);
			if (index == -1)
				return;
			state[index] = value;
		}

		private static string GetPropertyName<TType>(Expression<Func<TType, object>> expression)
		{
			return ExpressionProcessor.FindPropertyExpression(expression.Body);
		}
	}
}
