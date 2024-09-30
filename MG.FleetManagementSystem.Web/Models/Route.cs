// Models/Route.cs
namespace MG.FleetManagementSystem.Web.Models
{
    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public double Distance { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public decimal Price { get; set; }
        public int DriverId { get; set; }
    }
}