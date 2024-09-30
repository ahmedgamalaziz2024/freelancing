using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.FleetManagementSystem.Web.Services
{
    public interface IMaintenanceService
    {
        List<MaintenanceOrder> GetProactiveMaintenanceOrders();
        MaintenanceOrder CreateReactiveMaintenanceOrder(int vehicleId, string issue);
        List<MaintenanceOrder> GetAllMaintenanceOrders();
        MaintenanceOrder GetMaintenanceOrderById(int id);
        void UpdateMaintenanceOrder(MaintenanceOrder order);
        List<MaintenanceOrder> GetOverdueMaintenanceOrders();
        void CompleteMaintenanceOrder(int orderId, string completionNotes);
        MaintenanceOrder CreateMaintenanceOrder(MaintenanceOrder order);  // Add this line

    }

    public class MaintenanceService : IMaintenanceService
    {
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<MaintenanceOrder> _maintenanceOrderRepository;

        public MaintenanceService(IRepository<Vehicle> vehicleRepository, IRepository<MaintenanceOrder> maintenanceOrderRepository)
        {
            _vehicleRepository = vehicleRepository;
            _maintenanceOrderRepository = maintenanceOrderRepository;
        }

        public List<MaintenanceOrder> GetProactiveMaintenanceOrders()
        {
            var vehicles = _vehicleRepository.GetAll();
            var proactiveOrders = new List<MaintenanceOrder>();

            foreach (var vehicle in vehicles)
            {
                if (vehicle.OdometerReading >= vehicle.NextMaintenanceDueKm ||
                    DateTime.Now >= vehicle.NextMaintenanceDueDate)
                {
                    proactiveOrders.Add(new MaintenanceOrder
                    {
                        VehicleId = vehicle.Id,
                        Type = "Proactive",
                        Status = "Planned",
                        Description = $"Scheduled maintenance for {vehicle.LicensePlate}",
                        StartDate = DateTime.Now.AddDays(7), // Schedule for a week from now
                        EstimatedCompletionDate = DateTime.Now.AddDays(8)
                    });
                }
            }

            return proactiveOrders;
        }

        public MaintenanceOrder CreateReactiveMaintenanceOrder(int vehicleId, string issue)
        {
            var vehicle = _vehicleRepository.GetById(vehicleId);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found", nameof(vehicleId));

            var order = new MaintenanceOrder
            {
                VehicleId = vehicleId,
                Type = "Reactive",
                Status = "Urgent",
                Description = issue,
                StartDate = DateTime.Now,
                EstimatedCompletionDate = DateTime.Now.AddDays(2) // Estimate 2 days for reactive maintenance
            };

            _maintenanceOrderRepository.Add(order);
            return order;
        }

        public List<MaintenanceOrder> GetAllMaintenanceOrders()
        {
            return _maintenanceOrderRepository.GetAll().ToList();
        }

        public MaintenanceOrder GetMaintenanceOrderById(int id)
        {
            return _maintenanceOrderRepository.GetById(id);
        }

        public void UpdateMaintenanceOrder(MaintenanceOrder order)
        {
            var existingOrder = _maintenanceOrderRepository.GetById(order.Id);
            if (existingOrder == null)
            {
                throw new ArgumentException("Maintenance order not found", nameof(order));
            }

            existingOrder.VehicleId = order.VehicleId;
            existingOrder.Type = order.Type;
            existingOrder.Status = order.Status;
            existingOrder.Description = order.Description;
            existingOrder.StartDate = order.StartDate;
            existingOrder.EstimatedCompletionDate = order.EstimatedCompletionDate;
            existingOrder.CompletionDate = order.CompletionDate;
            existingOrder.CompletionNotes = order.CompletionNotes;
            existingOrder.EstimatedCost = order.EstimatedCost;
            existingOrder.ActualCost = order.ActualCost;

            _maintenanceOrderRepository.Update(existingOrder);

            // If the order is completed, update the vehicle status
            if (order.Status.ToLower() == "completed")
            {
                var vehicle = _vehicleRepository.GetById(order.VehicleId);
                if (vehicle != null)
                {
                    vehicle.Status = "Available";
                    vehicle.LastMaintenanceDate = DateTime.Now;
                    vehicle.NextMaintenanceDueDate = DateTime.Now.AddMonths(3);
                    vehicle.NextMaintenanceDueKm = vehicle.OdometerReading + 5000;
                    _vehicleRepository.Update(vehicle);
                }
            }
        }

        public List<MaintenanceOrder> GetOverdueMaintenanceOrders()
        {
            return _maintenanceOrderRepository.GetAll()
                .Where(o => o.Status != "Completed" && o.EstimatedCompletionDate < DateTime.Now)
                .ToList();
        }

        public void CompleteMaintenanceOrder(int orderId, string completionNotes)
        {
            var order = _maintenanceOrderRepository.GetById(orderId);
            if (order == null)
                throw new ArgumentException("Maintenance order not found", nameof(orderId));

            order.Status = "Completed";
            order.CompletionDate = DateTime.Now;
            order.CompletionNotes = completionNotes;

            _maintenanceOrderRepository.Update(order);

            // Update the vehicle's maintenance information
            var vehicle = _vehicleRepository.GetById(order.VehicleId);
            if (vehicle != null)
            {
                vehicle.LastMaintenanceDate = DateTime.Now;
                vehicle.NextMaintenanceDueKm = vehicle.OdometerReading + 5000; // Next maintenance due in 5000 km
                vehicle.NextMaintenanceDueDate = DateTime.Now.AddMonths(3); // Or 3 months, whichever comes first
                _vehicleRepository.Update(vehicle);
            }
        }

        public MaintenanceOrder CreateMaintenanceOrder(MaintenanceOrder order)
        {
            // Ensure the order has a valid vehicle
            var vehicle = _vehicleRepository.GetById(order.VehicleId);
            if (vehicle == null)
            {
                throw new ArgumentException("Invalid Vehicle ID");
            }

            // Set default values if not provided
            if (string.IsNullOrEmpty(order.Status))
            {
                order.Status = "Planned";
            }

            if (order.StartDate == default)
            {
                order.StartDate = DateTime.Now;
            }

            if (order.EstimatedCompletionDate == default)
            {
                order.EstimatedCompletionDate = order.StartDate.AddDays(7); // Default to a week later
            }

            // Add the order to the repository
            _maintenanceOrderRepository.Add(order);

            return order;
        }
    }
}