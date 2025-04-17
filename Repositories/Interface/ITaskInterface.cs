using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Repositories.Interface
{
    public interface ITaskInterface
    {
        Task<List<t_Task>> GetAllByEmp(string empid);
        Task<t_Task>GetOne(string taskid);
        Task<int> Add(t_Task taskData);
        Task<int> Update(t_Task taskData);
        Task<int> Delete(string taskid);

        
    }
}