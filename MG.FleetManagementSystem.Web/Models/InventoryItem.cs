// Models/InventoryItem.cs
namespace MG.FleetManagementSystem.Web.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // spare_part, consumable
        public int Quantity { get; set; }
        public int SafetyStock { get; set; }
        public decimal UnitCost { get; set; }
    }
}