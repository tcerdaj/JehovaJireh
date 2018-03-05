using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Core.Entities
{
	public enum Gender
	{
		Male =0 ,
		Female = 1
	}

	public enum DonationStatus
	{
		Created = 0,
		PartialRequested = 1,
		Requested = 2,
		Canceled = 3,
		Scheduled = 4,
		Delivery = 5,
		Matched = 6,
        PartialScheduled = 7
    }

	public enum DonationType
	{
		Vehicles = 0,
		Clothing = 1,
		Books = 2,
		Furniture = 3,
		ShoesAndAccessories = 4,
		HouseholdItems = 5,
		Linens = 6,
		SmallElectricalItems = 7,
		PianosAndOrgans = 8,
		ElectronicItems = 9,
		HouseholdAppliances = 10,
		Mattresses = 11,
		Tools = 12,
		Garden = 13,
		SchoolSupplies = 14,
		Money = 15,
		Other = 16
	}
}
