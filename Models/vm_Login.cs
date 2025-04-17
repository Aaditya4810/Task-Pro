using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class vm_Login
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string c_empEmail { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Password is required.")]
        public string c_empPassword { get; set; }
    }
}