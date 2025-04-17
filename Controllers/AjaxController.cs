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
    
    public class AjaxController : Controller
    {
        private readonly ITaskInterface _task;
        public AjaxController(ITaskInterface task)
        {
            _task=task;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTasks(int id=0)
        {
            List<t_Task> tasks=await _task.GetAllByEmp(id.ToString());
            return Ok(tasks);
        }

        public async Task<IActionResult> Add(t_Task task)
        {
            task.c_empId=(int)HttpContext.Session.GetInt32("EmpId");
            if(ModelState.IsValid)
            {
                Console.WriteLine(task.c_taskId);
                if(task.c_taskId==0)
                {
                    int result=await _task.Add(task);
                    if (result == 0)
                        {
                            return BadRequest(new
                            {
                                success = false,
                                message = "There was some error while adding the task"
                            });


                        }
                        else
                        {
                            return Ok(new
                            {
                                success = true,
                                message = "task Insterted Successfully.."

                            });

                        }
                }
                else
                {
                    int result=await _task.Update(task);
                    if (result == 0)
                        {
                            return BadRequest(new
                            {
                                success = false,
                                message = "There was some error while updating the task"
                            });


                        }
                        else
                        {
                            return Ok(new
                            {
                                success = true,
                                message = "task Updated Successfully.."

                            });

                        }
                }
            }
            else
            {
                return BadRequest(new
                {
                    
                    success = false,
                    message = "There was some error while adding the task"
                });
            }
        }

        public async Task<IActionResult> GetOnetasks(int id){
            t_Task task=await _task.GetOne(id.ToString());
            return Ok(task);
        }

        public async Task<IActionResult> Delete(int id){
            int result=await _task.Delete(id.ToString());
            if(result==1){
                return Ok(new
                {
                    success=true,
                    message="Task Deleted Successfully.."
                });
            }
            else{
                return BadRequest(new
                {
                    success=false,
                    message="There was some error while deleting the task"
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}