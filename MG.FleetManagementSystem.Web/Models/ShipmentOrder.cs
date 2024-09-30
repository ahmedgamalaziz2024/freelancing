using System;

namespace MG.FleetManagementSystem.Web.Models
{
    public class ShipmentOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public int RouteId { get; set; }
        public string Status { get; set; } // planned, confirmed, executed, in_transit, awaiting_return, completed
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ActualStartOdometer { get; set; }
        public int ActualEndOdometer { get; set; }
        public decimal Price { get; set; }
    }
}