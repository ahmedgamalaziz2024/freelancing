// Repositories/VehicleRepository.cs
using MG.FleetManagementSystem.Web.Models;

namespace MG.FleetManagementSystem.Web.Repositories
{
    public class RouteRepository : InMemoryRepository<Models.Route>
    {
        public RouteRepository()
        {
            // Add some initial data
            Add(new Models.Route { Id = 1, DriverId = 1, Name = "Cairo-Alex" , StartLocation = "Cairo", EndLocation = "Alexandria", Distance = 220, EstimatedTime = new TimeSpan(4, 30, 00), Price = 15000, });
            Add(new Models.Route { Id = 2, DriverId = 2, Name = "Aswan-Suhag", StartLocation = "Aswan", EndLocation = "Sohag", Distance = 520, EstimatedTime = new TimeSpan(13, 30, 00), Price = 34000, });
            Add(new Models.Route { Id = 3, DriverId = 3, Name = "Alex-Cairo", StartLocation = "Alexandria", EndLocation = "Cairo", Distance = 220, EstimatedTime = new TimeSpan(4, 30, 00), Price = 22000, });

        }
    }
}