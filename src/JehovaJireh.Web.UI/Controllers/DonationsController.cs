using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Logging;
using JehovaJireh.Web.UI.Models;
using Newtonsoft.Json;
using JehovaJireh.Web.UI.Extentions;
using JehovaJireh.Web.UI.App_GlobalResources;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Core.Entities;
using Omu.ValueInjecter;
using JehovaJireh.Data.Mappings;
using MvcSiteMapProvider;
using JehovaJireh.Core.EntitiesDto;
using System.Threading.Tasks;
using System.Net;

namespace JehovaJireh.Web.UI.Controllers
{
    public class DonationsController : BaseController
    {
		// GET: Donation list
		private ILogger log;
		private static IWindsorContainer container;
		private IDonationRepository donationRepository;
		private IUserRepository userRepository;
        private IRequestRepository requestRepository;
        User _user;

        private User CurrentUser
        {
            get
            {
               return _user;
            }

            set { _user = value; }
        }

        public DonationsController()
		{
			container = MvcApplication.BootstrapContainer();
			log = container.Resolve<ILogger>();
			donationRepository = container.Resolve<IDonationRepository>();
			userRepository = container.Resolve<IUserRepository>();
            requestRepository = container.Resolve<IRequestRepository>();
        }

		public ActionResult Index(DonationMessageId? message)
        {
			ViewBag.StatusMessage =
					message == DonationMessageId.CrateDonationSuccess ? Resources.YourDonationHasBenCreated
					: message == DonationMessageId.Error ? Resources.Error
					: "";
           
            CurrentUser = CurrentUser ?? userRepository.GetByUserName(User.Identity.Name);
            ViewBag.UserId = CurrentUser?.Id;

            //TODO: 
            //Create kendo parent child grid.
            //Get webapi call
            //if message is null show all donation 
            //else if message is not null show the current user donations.

            return View();
        }

		[Authorize]
		[HttpGet]
		public ActionResult MakeADonation()
		{
			//TODO: Maybe show the last 5 donations list
			var model = new DonationViewModels();
			model.DonationDetails.Add(new DonationDetailsViewModels());
			return View(model);
		}

		[HttpPost]
		public async System.Threading.Tasks.Task<ActionResult> MakeADonation(DonationViewModels model)
		{
            //Setting donation details from formData
            //TODO:
            //Save to database
            if (!ModelState.IsValid) return View(model);
            List<HttpPostedFile> files = new List<HttpPostedFile>();

            foreach (var f in Request.Files)
            {
                files.Add(f as HttpPostedFile);
            }

			try
			{
                var HeaderImagesJson = Request.Params["headerImages"];
                var donationDetailsJson = Request.Params["details"];

                //Deserialization
                var donationImages = JsonConvert.DeserializeObject<List<FileViewModel>>(HeaderImagesJson);
				var donationDetails = JsonConvert.DeserializeObject<List<DonationDetailsViewModels>>(donationDetailsJson);
                List<string> filesAdded = new List<string>();
                model.DonationDetails = donationDetails;
				var imageService = new ImageService(log);
				ViewBag.StatusMessage = "";
                

                //Amount validation if money is on
                if (model.IsMoney && model.Amount <= 0)
                {
                    ViewBag.StatusMessage = Resources.InvalidAmount;
                    return View(model);
                }

                filesAdded = new List<string>();
                foreach (var line in model.DonationDetails)
				{
					foreach (var file in line.MultiFileData)
					{
						//check if the image already exist.
						var fileExist = filesAdded.Any(x => x == file.Name);
						if (!fileExist)
						{
							//upload image to azure
							var fileToUpload = file.GetFile(Request.Files);
							if (fileToUpload != null)
							{
								var imageUrl = await imageService.CreateUploadedImageAsync(fileToUpload, Guid.NewGuid().ToString(), true, 500, 500);
								line.Images.Add(new ImageViewModel() { Item = line, ImageUrl = imageUrl });
								filesAdded.Add(file.Name);
							}
						}
					}
				}

                //Class Injection
                CurrentUser = userRepository.GetByUserName(User.Identity.Name);
				var donation = new Donation {
					Amount = model.Amount,
					CreatedBy = CurrentUser,
					CreatedOn = DateTime.Now,
					DonatedOn = DateTime.Now,
					Description = model.Description,
					DonationStatus = (Core.Entities.DonationStatus)model.DonationStatus,
					ExpireOn= model.ExpireOn,
					Title = model.Title,
					ModifiedOn = null
				};


                //Donation Images
                filesAdded = new List<string>();
                foreach (var fileWrapper in model.HeaderMultiFileData)
                {
                    //check if the image already exist.
                    var fileExist = filesAdded.Any(x => x == fileWrapper.FileName);
                    if (!fileExist)
                    {
                        var file = fileWrapper as HttpPostedFileBase;
                        var imageUrl = await imageService.CreateUploadedImageAsync(file, Guid.NewGuid().ToString(), true, 500, 500);
                        donation.AddImage(new DonationImage() { Donation = donation, ImageUrl = imageUrl, CreatedBy = CurrentUser, CreatedOn = DateTime.Now, ModifiedOn = null });
                        filesAdded.Add(fileWrapper.FileName);
                    }
                }

                if (model.DonationDetails != null && model.DonationDetails.Count() > 0)
                    donation.DonationDetails = new List<DonationDetails>();
                else
                    donation.DonationDetails = null;

                if (!model.IsMoney)
                {
                    foreach (var item in model.DonationDetails)
                    {


                        var itemLine = new DonationDetails
                        {
                            CreatedBy = CurrentUser,
                            Donation = donation,
                            CreatedOn = DateTime.Now,
                            ItemName = item.ItemName,
                            ItemType = (Core.Entities.DonationType)item.ItemType,
                            Line = item.Index,
                            DonationStatus = (Core.Entities.DonationStatus)item.DonationStatus,
                            ModifiedOn = null
                        };

                        foreach (var image in item.Images)
                        {
                            itemLine.Images.Add(
                                new DonationDetailsImage
                                {
                                    CreatedBy = CurrentUser,
                                    Item = itemLine,
                                    ImageUrl = image.ImageUrl,
                                    CreatedOn = DateTime.Now,
                                    ModifiedOn = null
                                });
                        }

                        donation.DonationDetails.Add(itemLine);
                    }
                }

				//save to database
				donationRepository.Create(donation);
				//Success
				return Json(new { result = "Redirect", url = Url.Action("Index", "Donations", new { Message = DonationMessageId.CrateDonationSuccess })});
			}
			catch (System.Exception ex)
			{
				ViewBag.StatusMessage = ex.Message;
			}

            return Json(new { result = "Error", Message = ViewBag.StatusMessage }); ;
		}

        [HttpPut]
        public ActionResult Update(int id, DonationDto model)
        {
            try
            {
                Console.WriteLine("Inside Update");
                if (model != null )
                {
                    var dta = donationRepository.Query().FirstOrDefault(x => x.Id == id);
                    dta.InjectFrom<DeepCloneInjection>(model);
                    var selected = dta.DonationDetails.Where(x => x.DonationStatus == Core.Entities.DonationStatus.Requested).Count();
                    var isRequested = model.DonationDetails != null || (selected + 1) == dta.DonationDetails.Count() ;
                    dta.DonationStatus = isRequested ? Core.Entities.DonationStatus.Requested : Core.Entities.DonationStatus.PartialRequested;

                    if (dta.DonationStatus == Core.Entities.DonationStatus.PartialRequested)
                    {
                        var itemId = model.DonationDetails.FirstOrDefault().Id;
                        var line = dta.DonationDetails.FirstOrDefault(x => x.Id == itemId);
                        line.RequestedBy = CurrentUser;
                        line.ModifiedBy = CurrentUser;
                        line.ModifiedOn = DateTime.Now;
                        line.DonationStatus = Core.Entities.DonationStatus.Requested;
                        dta.DonationDetails.Where(x => x.Id == itemId).Select(u => u = line);
                    }
                    donationRepository.Update(dta);
                    Response.StatusCode = (int)HttpStatusCode.OK;
                }
            }
            catch (System.Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { status = "error", data = ex.Message });
            }

            return Json(new { status = "ok", data = model });
        }

        public ActionResult RequestList()
        {
            ViewBag.UserId = CurrentUser?.Id;
            return View();
        }

		[Authorize]
		public ActionResult MakeNewRequest()
		{
			return View(new RequestViewModels());
		}

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> MakeNewRequest(RequestViewModels model)
		{
            if (!ModelState.IsValid) return View(model);

            try
            {
                
                //upload image to azure
                var imageService = new ImageService(log);
                var fileToUpload = model.ImageUpload;
                if (fileToUpload != null)
                {
                    var imageUrl = await imageService.CreateUploadedImageAsync(fileToUpload, Guid.NewGuid().ToString(), true, 500, 500);
                    model.ImageUrl = imageUrl;
                }

                Request data = new Request();
                CurrentUser = userRepository.GetByUserName(User.Identity.Name);
                data.InjectFrom<DeepCloneInjection>(model);
                data.CreatedBy = CurrentUser;
                data.CreatedOn = DateTime.Now;
                requestRepository.Create(data);
                return RedirectToAction("RequestList");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("error", ex.Message);
            }

            return View(model);
		}

		[Authorize]
		public ActionResult SchedulerDonation()
		{
            CurrentUser = CurrentUser ?? userRepository.GetByUserName(User.Identity.Name);
            ViewBag.UserId = CurrentUser?.Id;
            return View();
		}

        [Authorize]
        [HttpPost]
        public Task<DonationDto> RequestedBy(int donationId)
        {
            var dta = donationRepository.GetById(donationId);
            DonationDto dto = null;

            try
            {
                if (dta != null)
                {
                    dta.RequestedBy = CurrentUser;
                    dta.ModifiedBy = CurrentUser;
                    dta.ModifiedOn = DateTime.Now;
                    donationRepository.Update(dta);
                    dto = (DonationDto)new DonationDto().InjectFrom<DeepCloneInjection>(dta);
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return Task.FromResult<DonationDto>(dto);
        }

        #region Helpers	
        public enum DonationMessageId
		{
			CrateDonationSuccess,
			Error,
		}

       
		#endregion
	}
}