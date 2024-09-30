using MG.FleetManagementSystem.Web.Models;
using MG.FleetManagementSystem.Web.Repositories;

public class UserRepository : InMemoryRepository<User>
{
    public UserRepository()
    {// Add some initial data
        Add(new User { FullName = "Mohamed Sayed", Role = "driver", Username = "mohamed.sayed", Password = "password" });
        Add(new User { FullName = "Ahmed Jameel", Role = "driver", Username = "ahmed.jameel", Password = "password" });
        Add(new User { FullName = "Ahmed Goda", Role = "driver", Username = "ahmed.goda", Password = "password" });
        Add(new User { FullName = "CISO Real Estate", Role = "customer", Username = "ciso.realestate", Password = "password" });
        Add(new User { FullName = "GAMGOOM Pharma", Role = "customer", Username = "gamgoom.pharma", Password = "password" });
        Add(new User { FullName = "Sherif Abdallah", Role = "admin", Username = "sherif.abdallah", Password = "password" });
    }
}