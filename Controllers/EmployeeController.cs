using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagement.Models;
using TaskManagement.Repositories.Interface;

namespace TaskManagement.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeInterface _emp;

        public EmployeeController(IEmployeeInterface emp)
        {
            _emp = emp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(vm_Login login)
        {

            if (ModelState.IsValid)
            {
                t_Employee empData = await _emp.Login(login);
                if (empData.c_empId != 0)
                {
                    HttpContext.Session.SetInt32("EmpId", empData.c_empId);
                    HttpContext.Session.SetString("EmpName", empData.c_empName);
                    HttpContext.Session.SetString("EmpEmail", empData.c_empEmail);

                    HttpContext.Session.SetString("img", empData.c_profileImage);

                    return RedirectToAction("Profile", "Employee");
                }
                else
                {
                    ViewData["Message"] = "Invalid Username and Password";
                }
            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(t_Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.ProfileImage != null && employee.ProfileImage.Length > 0)
                {
                    // Save the uploaded file
                    string uid = Guid.NewGuid().ToString();
                    var fileName = uid + Path.GetExtension(employee.ProfileImage.FileName);
                    var filePath = Path.Combine("../TaskManagement/wwwroot/profile_images", fileName);
                    Directory.CreateDirectory(Path.Combine("../TaskManagement/wwwroot/profile_images"));
                    employee.c_profileImage = fileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        employee.ProfileImage.CopyTo(stream);
                    }
                }
                int result = await _emp.Add(employee);
                if (result == -1)
                {
                    TempData["Message"] = "There is some error in add or updating contact";
                    return View();

                }
                else if (result == 1)
                {
                    TempData["Message"] = "Added/Updated Successfully";
                    return View();

                }
                else
                {
                    TempData["Message"] = "User already exist with same email id";
                    return View();

                }
            }
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            if(HttpContext.Session.GetInt32("EmpId")==null)
            {
                return View("Login");
            }
            else
            {
                t_Employee emp=await _emp.GetAllByEmpId(HttpContext.Session.GetInt32("EmpId").Value);

                ViewBag.EmpData=emp;
                return View();
            }
           
        }

        public IActionResult UpdateProfile()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProfile(t_UpdateEmployee employee)
        {
            if (employee.ProfileImage != null && employee.ProfileImage.Length > 0)
            {
                string uid = Guid.NewGuid().ToString();
                
                // Save the uploaded file
                var fileName = uid + Path.GetExtension(employee.ProfileImage.FileName);
                var filePath = Path.Combine("../TaskManagement/wwwroot/profile_images", fileName);
                Directory.CreateDirectory(Path.Combine("../TaskManagement/wwwroot/profile_images"));
                employee.c_profileImage = fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    employee.ProfileImage.CopyTo(stream);
                }
            }
            if (ModelState.IsValid)
            {

                employee.c_empId = (int)HttpContext.Session.GetInt32("EmpId"); //to get empid
                int result = await _emp.UpdateProfile(employee);
                if (result == 1)
                {
                    await Profile();  //call to show profile after update
                    TempData["Message"] = "Updated Successfully";
                    return View("Profile");
                }
                else
                {
                    TempData["Message"] = "There is some error in updating contact";
                    return View("Profile");
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}