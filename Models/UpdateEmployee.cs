using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class t_UpdateEmployee
    {
        public int c_empId { get; set; }
        
        [StringLength(50)]
        [Required(ErrorMessage ="Name is required")]
        public string? c_empName { get; set; }
        public string? c_profileImage { get; set; }
        public IFormFile? ProfileImage{get;set;}
    }
}