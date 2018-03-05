using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Web.UI.Helpers;
using JehovaJireh.Web.UI.Models;

namespace JehovaJireh.Web.UI.Extentions
{
	public static  class FIleExptentions
	{
		public static async System.Threading.Tasks.Task<string> GenerateCloudUrlAsync(this FileViewModel model, ImageService service)
		{
			string imageIrl = null;

			if (model != null && service != null)
			{
				try
				{
					string strPhoto = (model.Name);
					string filePath = Path.GetFullPath(model.Name);
					var test = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
					FileStream fs = new FileStream(strPhoto, FileMode.Open, FileAccess.Read);
					Byte[] imgByte = new byte[fs.Length];

					fs.Read(imgByte, 0, System.Convert.ToInt32(fs.Length));
					Bitmap originalBMP = new Bitmap(fs);
					MemoryFile file = new MemoryFile(fs, model.Type, model.Name);
					fs.Close();
					imageIrl = await service.CreateUploadedImageAsync(file, Guid.NewGuid().ToString(), true, 200, 200);
				}
				catch (System.Exception ex)
				{
					throw ex;
				}
			}

			return imageIrl;
		}

		public static HttpPostedFileBase GetFile(this FileViewModel model, HttpFileCollectionBase files)
		{
			for (var i=0; i<files.Count; i++)
			{
				HttpPostedFileBase file = files[i];
				if (file.FileName == model.Name)
					return file;
			}

			return null;
		}
	}
}