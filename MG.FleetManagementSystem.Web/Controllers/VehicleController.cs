using Microsoft.AspNetCore.Mvc;
using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using MG.FleetManagementSystem.Web.Attributes;

namespace MG.FleetManagementSystem.Web.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehiclesController(IRepository<Vehicle> vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // GET: Vehicles

        public IActionResult Index()
        {
            var vehicles = _vehicleRepository.GetAll();
            return View(vehicles);
        }

        [AuthorizeRoles("admin", "manager")]
        public IActionResult Details(int id)
        {
            try
            {
                var vehicle = _vehicleRepository.GetById(id);
                if (vehicle == null)
                {
                    return Content("Vehicle not found");
                }
                return PartialView("Details", vehicle);
            }
            catch (Exception ex)
            {
                return Content($"An error occurred: {ex.Message}");
            }
        }

        // GET: Vehicles/Create

        [AuthorizeRoles("admin", "manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("admin", "manager")]
        public IActionResult Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _vehicleRepository.Add(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        [AuthorizeRoles("admin", "manager")]
        public IActionResult Edit(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("admin", "manager")]
        public IActionResult Edit(int id, Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _vehicleRepository.Update(vehicle);
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        [AuthorizeRoles("admin", "manager")]
        public IActionResult Delete(int id)
        {
            var vehicle = _vehicleRepository.GetById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("admin", "manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            _vehicleRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}