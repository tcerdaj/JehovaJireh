using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JehovaJireh.Core.IRepositories
{
	public interface IImageService
	{
		Task<string> CreateUploadedImageAsync(HttpPostedFileBase file, string fileName, bool resizeImage = false, int width = 0, int height = 0);
	}
}
