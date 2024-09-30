using MG.FleetManagementSystem.Web.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MG.FleetManagementSystem.Web.Models
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vehicle type is required")]
        [StringLength(50, ErrorMessage = "Vehicle type cannot be longer than 50 characters")]
        public string Type { get; set; } = "Truck";

        [Required(ErrorMessage = "License plate is required")]
        [StringLength(20, ErrorMessage = "License plate cannot be longer than 20 characters")]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; } = "123";

        [Required(ErrorMessage = "Odometer reading is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Odometer reading must be a positive number")]
        [Display(Name = "Odometer Reading")]
        public int OdometerReading { get; set; }

        [Required(ErrorMessage = "License expiry date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "License Expiry Date")]
        public DateTime LicenseExpiryDate { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Weight must be a positive number")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Volume is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Volume must be a positive number")]
        public double Volume { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "available";

        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public int RouteId { set; get; }
        public string MapInfo { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Last Maintenance Date")]
        public DateTime? LastMaintenanceDate { get; set; }

        [Display(Name = "Next Maintenance Due Km")]
        public int NextMaintenanceDueKm { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Next Maintenance Due Date")]
        public DateTime NextMaintenanceDueDate { get; set; }
        public void PopulateMapInfo(Route route, User driver)
        {
            StringBuilder mapInfo = new StringBuilder();
            mapInfo.AppendLine("Vehicle: " + this.LicensePlate + " --- ");
            mapInfo.AppendLine("Route: " + route?.Name + " --- ");
            mapInfo.AppendLine("Driver: " + driver?.FullName + " --- ");
            this.MapInfo = mapInfo.ToString();
        }
    }
}