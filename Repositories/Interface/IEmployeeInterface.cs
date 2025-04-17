using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Models;

namespace TaskManagement.Repositories.Interface
{
    public interface IEmployeeInterface
    {
        #region Registeration
        Task<int>Add(t_Employee employee);
        #endregion

        #region Login
        Task<t_Employee>Login(vm_Login login);
        #endregion

        #region Profile
        Task<int> UpdateProfile(t_UpdateEmployee employee);
        #endregion

        Task<t_Employee>GetAllByEmpId(int empid);

    }
}