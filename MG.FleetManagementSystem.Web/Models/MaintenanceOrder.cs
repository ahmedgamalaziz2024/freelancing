// Models/MaintenanceOrder.cs
using System;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;


namespace MG.FleetManagementSystem.Web.Models
{
    public class MaintenanceOrder
    {
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; } = 1;

        [Required]
        public string Type { get; set; } = "Proactive"; // "Proactive" or "Reactive"

        [Required]
        public string Status { get; set; } = "Planned";// "Planned", "In Progress", "Completed", "Overdue"

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EstimatedCompletionDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        public string CompletionNotes { get; set; } = "";

        public decimal EstimatedCost { get; set; }

        public decimal? ActualCost { get; set; }

        // Navigation property
        public Vehicle Vehicle { get; set; } = new Vehicle();
    }

    public class EmployeeHours
    {
        public int EmployeeId { get; set; }
        public double Hours { get; set; }
    }

    public class UsedPart
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
    }

}