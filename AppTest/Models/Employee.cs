using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AppTest.Models 
{
    public class Employee : IdentityUser
    { 
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public DateTime Birthday { get; set; }
        public string? Address { get; set; }

        [Required]
        [ForeignKey("department")]
        public int IdDepartment { get; set; }
        public Department? department { get; set; }

        public string? Img { get; set; }

        public DateTime? Create_At { get; set; } = DateTime.Now;
    }
}
