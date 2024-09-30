// Repositories/VehicleRepository.cs
using MG.FleetManagementSystem.Web.Models;

namespace MG.FleetManagementSystem.Web.Repositories
{
    public class VehicleRepository : InMemoryRepository<Vehicle>
    {
        public VehicleRepository()
        {
            
            // Add some initial data
            Add(new Vehicle { Type = "Truck", RouteId = 1, LicensePlate = "م ق ص 897", OdometerReading = 98000, CurrentLatitude = 30.43, CurrentLongitude = 31.56, Id = 1, LicenseExpiryDate = DateTime.Now.AddMonths(10), Status = "available", Weight = 4500, Volume = 2.45});
            Add(new Vehicle { Type = "Truck", RouteId = 2, LicensePlate = "ن ن خ 898", OdometerReading = 123000, CurrentLatitude = 30.73, CurrentLongitude = 32.56, Id = 2, LicenseExpiryDate = DateTime.Now.AddMonths(15), Status = "available", Weight = 2500, Volume = 2.45 });
            Add(new Vehicle { Type = "Sedan", RouteId = 3, LicensePlate = "ح ج م 342", OdometerReading = 320000, CurrentLatitude = 30.93, CurrentLongitude = 33.56, Id = 3, LicenseExpiryDate = DateTime.Now.AddMonths(33), Status = "available", Weight = 1500, Volume = 2.45 });
        }
    }
}