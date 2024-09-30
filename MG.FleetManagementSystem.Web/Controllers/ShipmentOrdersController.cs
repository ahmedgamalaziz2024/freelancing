using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Repositories;
using System.Linq;

namespace MG.FleetManagementSystem.Web.Controllers
{
    public class ShipmentOrdersController : Controller
    {
        private readonly IRepository<ShipmentOrder> _shipmentOrderRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Models.Route> _routeRepository;

        public ShipmentOrdersController(
            IRepository<ShipmentOrder> shipmentOrderRepository, IRepository<Vehicle> vehicleRepository, IRepository<User> userRepository, IRepository<Models.Route> routeRepository)
        {
            _shipmentOrderRepository = shipmentOrderRepository;
            _vehicleRepository = vehicleRepository;
            _userRepository = userRepository;
            _routeRepository = routeRepository;
        }

        public IActionResult Index()
        {
            var shipmentOrders = _shipmentOrderRepository.GetAll();
            return View(shipmentOrders);
        }

        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ShipmentOrder shipmentOrder)
        {
            if (ModelState.IsValid)
            {
                _shipmentOrderRepository.Add(shipmentOrder);
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns();
            return View(shipmentOrder);
        }

        public IActionResult Edit(int id)
        {
            var shipmentOrder = _shipmentOrderRepository.GetById(id);
            if (shipmentOrder == null)
            {
                return NotFound();
            }
            PopulateDropDowns();
            return View(shipmentOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ShipmentOrder shipmentOrder)
        {
            if (id != shipmentOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _shipmentOrderRepository.Update(shipmentOrder);
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDowns();
            return View(shipmentOrder);
        }

        // Implement Details and Delete actions similarly
        public IActionResult Details(int id)
        {
            var shipmentOrder = _shipmentOrderRepository.GetById(id);
            if (shipmentOrder == null)
            {
                return NotFound();
            }

            var customer = _userRepository.GetById(shipmentOrder.CustomerId);
            var vehicle = _vehicleRepository.GetById(shipmentOrder.VehicleId);
            var driver = _userRepository.GetById(shipmentOrder.DriverId);
            var route = _routeRepository.GetById(shipmentOrder.RouteId);

            ViewBag.CustomerName = customer?.FullName;
            ViewBag.VehicleLicensePlate = vehicle?.LicensePlate;
            ViewBag.DriverName = driver?.FullName;
            ViewBag.RouteName = route?.Name;

            return PartialView("_Details", shipmentOrder);
        }
        private void PopulateDropDowns()
        {
            ViewBag.Vehicles = new SelectList(_vehicleRepository.GetAll().Where(v => v.Status.ToLower() == "available"), "Id", "LicensePlate");
            ViewBag.Drivers = new SelectList(_userRepository.GetAll().Where(u => u.Role.ToLower() == "driver"), "Id", "FullName");
            ViewBag.Customers = new SelectList(_userRepository.GetAll().Where(u => u.Role.ToLower() == "customer"), "Id", "FullName");
            ViewBag.Routes = new SelectList(_routeRepository.GetAll(), "Id", "Name");
        }
    }
}