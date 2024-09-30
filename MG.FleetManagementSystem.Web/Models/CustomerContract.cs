// Models/CustomerContract.cs
using System;
using System.Collections.Generic;

namespace MG.FleetManagementSystem.Web.Models
{
    public class CustomerContract
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Terms { get; set; }
        public List<PriceListItem> PriceList { get; set; }
    }

    public class PriceListItem
    {
        public int RouteId { get; set; }
        public decimal Price { get; set; }
    }
}