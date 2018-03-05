using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Queues
{
	public interface IQueue
	{
		void AddMessage(QueueMessageBase message);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "We already provide non-generic")]
		T GetMessage<T>() where T : QueueMessageBase;

		object GetMessage(Type messageType);

		void DeleteMessage(QueueMessageBase message);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "We are providing a version without Generics")]
		T[] GetMultipleMessages<T>(int maxMessages) where T : QueueMessageBase;

		object[] GetMultipleMessages(Type messageType, int maxMessages);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "We already provide non-generic")]
		void Purge<T>() where T : QueueMessageBase;

		void Purge(Type messageType);
	}
}
