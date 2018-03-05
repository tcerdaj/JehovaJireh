using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JehovaJireh.Configuration;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using JehovaJireh.Data.Extentions;
using System.Drawing;

namespace JehovaJireh.Data.Repositories
{
	public class ImageService : IImageService
	{
		public CloudBlobClient cloudBlobClient;
		public CloudBlobContainer cloudBlobContainer;
		public CloudStorageAccount cloudStorageAccount;
		public ILogger log;

		public ImageService(ILogger log)
		{
			try
			{
				this.log = log;
				this.cloudStorageAccount = CloudConfiguration.GetStorageAccount("StorageConnectionString");
				this.cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
				this.cloudBlobContainer = cloudBlobClient.GetContainerReference("images");
				this.cloudBlobContainer.SetPermissions(
					new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }
				);
				this.cloudBlobContainer.CreateIfNotExistsAsync();
			}
			catch (System.Exception ex)
			{
				if (this.log != null)
					this.log.Error("An error occurred in  ImageService(ILogger log) constructor.", ex);
			}
		}

		public async Task CreateIfNotExist()
		{
			if (await cloudBlobContainer.CreateIfNotExistsAsync())
			{
				await cloudBlobContainer.SetPermissionsAsync(
					new BlobContainerPermissions
					{
						PublicAccess = BlobContainerPublicAccessType.Blob
					}
					);
			}
		}
		public async Task<string> CreateUploadedImageAsync(HttpPostedFileBase file, string fileName, bool resizeImage = false, int width = 0, int height = 0)
		{
			if (file == null)
				return null;

			string imageFullPath = null;
			try
			{
				var _fileName = file.FileName;
				var imageName = fileName + Path.GetExtension(_fileName);
                CloudBlockBlob originalcloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(_fileName);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
				cloudBlockBlob.Properties.ContentType = file.ContentType;

				if (resizeImage)
				{
					Bitmap bitmap = new Bitmap(file.InputStream);

					int oldWidth = bitmap.Width;
					int oldHeight = bitmap.Height;

					GraphicsUnit units = System.Drawing.GraphicsUnit.Pixel;
					RectangleF r = bitmap.GetBounds(ref units);
					Size newSize = new Size();

					float expectedWidth = r.Width;
					float expectedHeight = r.Height;
					float dimesion = r.Width / r.Height;

					if (width < r.Width)
					{
						expectedWidth = width;
						expectedHeight = expectedWidth / dimesion;
					}
					else if (height < r.Height)
					{
						expectedHeight = height;
						expectedWidth = dimesion * expectedHeight;
					}
					if (expectedWidth > width)
					{
						expectedWidth = width;
						expectedHeight = expectedHeight / expectedWidth;
					}
					//else if (nPozadovanaVyska > height)
					//{
					//	expectedHeight = height;
					//	expectedWidth = dimesion * expectedHeight;
					//}
					newSize.Width = (int)Math.Round(expectedWidth);
					newSize.Height = (int)Math.Round(expectedHeight);

					Bitmap b = new Bitmap(bitmap, newSize);

					Image img = (Image)b;
					byte[] data = ImageToByte(img);

					cloudBlockBlob.UploadFromByteArray(data, 0, data.Length);
                    await originalcloudBlockBlob.DeleteIfExistsAsync(); //remove old if exist.
                }
				else
				{
					await cloudBlockBlob.UploadFromStreamAsync(file.InputStream);
				}

				imageFullPath = cloudBlockBlob.Uri.ToString();
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("An error ocurred in CreateUploadedImageAsync method.", ex);
			}
			finally
			{
				Console.WriteLine("CreateUploadedImageAsync Finish.");
			}

			return imageFullPath;
		}

		/// <summary>
		/// Image to byte
		/// </summary>
		/// <param name="img">Image</param>
		/// <returns>byte array</returns>
		public static byte[] ImageToByte(Image img)
		{
			byte[] byteArray = new byte[0];
			using (MemoryStream stream = new MemoryStream())
			{
				img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				stream.Close();

				byteArray = stream.ToArray();
			}
			return byteArray;
		}
	}
}
