using System.ComponentModel.DataAnnotations;

namespace MG.FleetManagementSystem.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = "admin";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "admin";

        [Required]
        public string Role { get; set; } = "Customer";
        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}