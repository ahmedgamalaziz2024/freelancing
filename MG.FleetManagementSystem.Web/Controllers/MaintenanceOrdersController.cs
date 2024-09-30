using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Services;
using MG.FleetManagementSystem.Web.Attributes;
using System.Linq;
using MG.FleetManagementSystem.Web.Repositories;

namespace MG.FleetManagementSystem.Web.Controllers
{
    [Authorize]
    public class MaintenanceOrdersController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public MaintenanceOrdersController(IMaintenanceService maintenanceService, IRepository<Vehicle> vehicleRepository)
        {
            _maintenanceService = maintenanceService;
            _vehicleRepository = vehicleRepository;
        }

        [AuthorizeRoles("admin", "manager")]
        public IActionResult Index()
        {
            var orders = _maintenanceService.GetAllMaintenanceOrders();
            return View(orders);
        }

        [AuthorizeRoles("admin", "manager")]
        public IActionResult CreateProactive()
        {
            var proactiveOrders = _maintenanceService.GetProactiveMaintenanceOrders();
            return View(proactiveOrders);
        }

        [HttpPost]
        [AuthorizeRoles("admin", "manager")]
        public IActionResult ConfirmProactive(List<int> selectedVehicleIds)
        {
            var proactiveOrders = _maintenanceService.GetProactiveMaintenanceOrders()
                .Where(o => selectedVehicleIds.Contains(o.VehicleId))
                .ToList();

            foreach (var order in proactiveOrders)
            {
                _maintenanceService.CreateMaintenanceOrder(order);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        [AuthorizeRoles("admin", "manager")]
        public IActionResult CreateReactive()
        {
            ViewBag.Vehicles = new SelectList(_vehicleRepository.GetAll(), "Id", "LicensePlate");
            return View();
        }

        [HttpPost]

        [AuthorizeRoles("admin", "manager")]
        public IActionResult CreateReactive(MaintenanceOrder order)
        {
            if (ModelState.IsValid)
            {
                order.Type = "Reactive";
                order.Status = "Planned";
                _maintenanceService.CreateMaintenanceOrder(order);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Vehicles = new SelectList(_vehicleRepository.GetAll(), "Id", "LicensePlate");
            return View(order);
        }

        [AuthorizeRoles("admin", "manager")]
        public IActionResult Details(int id)
        {
            var order = _maintenanceService.GetMaintenanceOrderById(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpGet]
        [AuthorizeRoles("admin", "manager", "Mechanic")]
        public IActionResult Edit(int id)
        {
            var order = _maintenanceService.GetMaintenanceOrderById(id);
            if (order == null)
                return NotFound();

            ViewBag.Vehicles = new SelectList(_vehicleRepository.GetAll(), "Id", "LicensePlate");
            return View(order);
        }

        [HttpPost]
        [AuthorizeRoles("admin", "manager", "Mechanic")]
        public IActionResult Edit(MaintenanceOrder order)
        {
            if (ModelState.IsValid)
            {
                _maintenanceService.UpdateMaintenanceOrder(order);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Vehicles = new SelectList(_vehicleRepository.GetAll(), "Id", "LicensePlate");
            return View(order);
        }

        [AuthorizeRoles("admin", "manager")]
        public IActionResult Overdue()
        {
            var overdueOrders = _maintenanceService.GetOverdueMaintenanceOrders();
            return View(overdueOrders);
        }

        [HttpGet]
        [AuthorizeRoles("admin", "manager", "Mechanic")]
        public IActionResult Complete(int id)
        {
            var order = _maintenanceService.GetMaintenanceOrderById(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

       
        [HttpPost]
        [AuthorizeRoles("admin", "manager", "Mechanic")]
        public IActionResult Complete(int id, string completionNotes, decimal actualCost)
        {
            var order = _maintenanceService.GetMaintenanceOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = "Completed";
            order.CompletionDate = DateTime.Now;
            order.CompletionNotes = completionNotes;
            order.ActualCost = actualCost;

            _maintenanceService.UpdateMaintenanceOrder(order);

            // Update vehicle status and schedule next maintenance
            var vehicle = _vehicleRepository.GetById(order.VehicleId);
            if (vehicle != null)
            {
                vehicle.Status = "Available";
                vehicle.LastMaintenanceDate = DateTime.Now;
                vehicle.NextMaintenanceDueDate = DateTime.Now.AddMonths(3); // Schedule next maintenance in 3 months
                vehicle.NextMaintenanceDueKm = vehicle.OdometerReading + 5000; // Schedule next maintenance after 5000 km
                _vehicleRepository.Update(vehicle);
            }

            return RedirectToAction(nameof(Index));
        }
         


        private void PopulateDropDowns()
        {
            ViewBag.Vehicles = new SelectList(_vehicleRepository.GetAll(), "Id", "LicensePlate");
        }
    }
}