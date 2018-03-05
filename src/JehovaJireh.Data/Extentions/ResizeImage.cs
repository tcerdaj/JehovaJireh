using System.IO;
using System.Threading.Tasks;
using System.Web;
using ImageResizer;

namespace JehovaJireh.Data.Extentions
{
	public static class ImageExtention
	{
		public static async Task<MemoryStream> ResizeImage(this HttpPostedFileBase image, int width, int height)
		{

			//  string basePath = ImageResizer.Util.PathUtils.RemoveExtension(original);

			byte[] data = new byte[image.ContentLength];
			await image.InputStream.ReadAsync(data, 0, data.Length);

			var inputStream = new MemoryStream(data);
			var memoryStream = new MemoryStream();
			var settings = string.Format("width={0}&height={1}", width, height);

			var i = new ImageJob(inputStream, memoryStream, new Instructions(settings));
			ImageBuilder.Current.Build(i);

			return memoryStream;
		}


	}
}