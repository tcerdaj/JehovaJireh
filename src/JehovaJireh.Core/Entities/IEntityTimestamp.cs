using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public interface IEntityTimestamp
	{
		User CreatedBy { get; set; }
		User ModifiedBy { get; set; }
		DateTime? CreatedOn { get; set; }
		DateTime? ModifiedOn { get; set; }
	}
}
