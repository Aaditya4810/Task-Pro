using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class t_Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int c_empId { get; set; }
        
        [StringLength(50)]
        [Required(ErrorMessage ="Name is required")]
        public string c_empName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Email is required")]
        public string c_empEmail { get; set; }
        
        [StringLength(50)]
         [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
        ErrorMessage = "Password must be at least 6 characters long, contain an uppercase letter, a digit, and a special character.")]
        [Required(ErrorMessage = "Password is required")]
        public string c_empPassword { get; set; }

        public string? c_profileImage { get; set; }
        public IFormFile ProfileImage{get;set;}
    }
}