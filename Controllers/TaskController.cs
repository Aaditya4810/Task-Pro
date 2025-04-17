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
   
    public class TaskController : Controller
    {

        private readonly ITaskInterface _task;

        public TaskController(ITaskInterface task)
        {
            _task = task;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        // public IActionResult List()
        // {
        //     return View();
        // }

        public async Task<ActionResult> List()
        {
             
            if(HttpContext.Session.GetInt32("EmpId")!=null)
            {
                List<t_Task> task = await _task.GetAllByEmp(HttpContext.Session.GetInt32("EmpId").ToString());
                
                return View(task);
            }
            else
            {
                return RedirectToAction("Profile","Employee");
            }
           
        }

        [HttpGet]
        public async Task<ActionResult> Add(string id="")
        {
            t_Task task=new t_Task();
            if (id != "")
            {
                task = await _task.GetOne(id);
            }
            return View(task);
        }

        [HttpPost]
        public async Task<ActionResult> Add(t_Task task)
        {
            if(ModelState.IsValid)
            {   
                task.c_empId=(int)HttpContext.Session.GetInt32("EmpId");
                var result=0;
                if (task.c_taskId == 0)
                {
                    result = await _task.Add(task);
                }
                else
                {
                    result = await _task.Update(task);
                }
                if (result == 0)
                {
                    TempData["Message"]="There is some error in add or updating contact";
                    // return View("List");
                    return RedirectToAction("List","Task");
                }
                else
                {
                    TempData["Message"] = "Added/Updated Successfully";
                    return RedirectToAction("List","Task");
                    // return View("List");
                }   
            }     
            
            return View("List");
           
        }

        public async Task<ActionResult> Delete(string id)
        {
            int status = await _task.Delete(id);   
            if (status == 1)
            {
                ViewData["Message"] = "Deleted Successfully";
                return RedirectToAction("List");
            }
            else
            {
                ViewData["Message"] = "Not Deleted";
                return RedirectToAction("List");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}