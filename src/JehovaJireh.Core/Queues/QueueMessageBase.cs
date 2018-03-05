using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JehovaJireh.Core.Queues
{
	[DataContract]
	public abstract class QueueMessageBase
	{
		private object context;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "We would never want this to be serialized")]
		public object GetContext()
		{
			return this.context;
		}

		public void SetContext(object value)
		{
			this.context = value;
		}
	}
}
