using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagement.Models
{
    public class t_Task
    {
        public int c_taskId { get; set; }
        public int c_empId { get; set; }
        public string c_title { get; set; }
        public string c_description { get; set; }
        public int c_estimatedDays { get; set; }
        public DateTime? c_startDate { get; set; }
        public DateTime? c_endDate { get; set; }
        public string? c_status { get; set; }
    }
}