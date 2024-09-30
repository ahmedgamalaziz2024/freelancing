using Microsoft.AspNetCore.Mvc;
using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using MG.FleetManagementSystem.Web.Services;

namespace MG.FleetManagementSystem.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<ShipmentOrder> _shipmentOrderRepository;
        private readonly IRepository<MaintenanceOrder> _maintenanceOrderRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IRepository<Models.Route> _routeRepository;

        public HomeController(
            IRepository<Vehicle> vehicleRepository,
            IRepository<ShipmentOrder> shipmentOrderRepository,
            IRepository<MaintenanceOrder> maintenanceOrderRepository,
            IRepository<User> userRepository,
            IMaintenanceService maintenanceService,
            IRepository<Models.Route> routeRepository)
        {
            _vehicleRepository = vehicleRepository;
            _shipmentOrderRepository = shipmentOrderRepository;
            _maintenanceOrderRepository = maintenanceOrderRepository;
            _userRepository = userRepository;
            _maintenanceService = maintenanceService;
            _routeRepository = routeRepository;
        }

        public IActionResult Index()
        {
            var vehicles = _vehicleRepository.GetAll().ToList();
            var shipmentOrders = _shipmentOrderRepository.GetAll().ToList();
            var maintenanceOrders = _maintenanceOrderRepository.GetAll().ToList();

            ViewBag.TotalVehicles = vehicles.Count;
            ViewBag.AvailableVehicles = vehicles.Count(v => v.Status.ToLower() == "available");
            ViewBag.VehiclesInMaintenance = vehicles.Count(v => v.Status.ToLower() == "maintenance");
            ViewBag.ActiveShipments = shipmentOrders.Count(s => s.Status.ToLower() == "in_transit");
            ViewBag.CompletedShipments = shipmentOrders.Count(s => s.Status.ToLower() == "completed");
            ViewBag.PendingMaintenance = maintenanceOrders.Count(m => m.Status.ToLower() == "planned");
            ViewBag.OngoingMaintenance = maintenanceOrders.Count(m => m.Status.ToLower() == "in progress");
            ViewBag.CustomersCount = _userRepository.GetAll().Count(u => u.Role.ToLower() == "customer");
            ViewBag.DriversCount = _userRepository.GetAll().Count(u => u.Role.ToLower() == "driver");
            ViewBag.TotalRevenue = shipmentOrders.Where(s => s.Status.ToLower() == "completed").Sum(s => s.Price);
            ViewBag.AverageShipmentPrice = shipmentOrders.Any() ? shipmentOrders.Average(s => s.Price) : 0;
            ViewBag.OverdueMaintenanceCount = _maintenanceService.GetOverdueMaintenanceOrders().Count();

            return View();
        }

        public IActionResult VehicleLocations()
        {
            var vehicles = _vehicleRepository.GetAll().ToList();

            foreach (var vehicle in vehicles)
            {
                var route = _routeRepository.GetById(vehicle.RouteId);
                var driver = _userRepository.GetById(route.DriverId);
                vehicle.PopulateMapInfo(route, driver);
            }

            return View(vehicles);
        }

        public IActionResult RouteDirections()
        {
            var route = _routeRepository.GetAll();
            if (route == null)
            {
                return NotFound();
            }

            //// Find a shipment order using this route that is in transit
            //var routes = _routeRepository.GetAll();

            //ViewBag.RouteDirections = routes;

            return View(route);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}