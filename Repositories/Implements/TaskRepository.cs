using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagement.Repositories.Interface;
using TaskManagement.Models;
using Npgsql;
using System.Data;

namespace TaskManagement.Repositories.Implements
{
    public class TaskRepository : ITaskInterface
    {
        private readonly NpgsqlConnection _conn;

        public TaskRepository(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public async Task<int> Add(t_Task data)
        {
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO t_tasks (c_empid,c_title,c_description,c_estimateddays,c_startdate,c_enddate,c_status)
                VALUES
               (@c_empid,@c_title,@c_description,@c_estimateddays,@c_startdate,@c_enddate,@c_status);", _conn);



                cm.Parameters.AddWithValue("@c_empid", data.c_empId);
                cm.Parameters.AddWithValue("@c_title", data.c_title);
                cm.Parameters.AddWithValue("@c_description", data.c_description);
                cm.Parameters.AddWithValue("@c_estimateddays", data.c_estimatedDays);
                cm.Parameters.AddWithValue("@c_startdate", data.c_startDate);
                cm.Parameters.AddWithValue("@c_enddate", data.c_endDate);
                cm.Parameters.AddWithValue("@c_status", data.c_status);

                _conn.Close();
                _conn.Open();
                cm.ExecuteNonQuery();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<int> Delete(string taskId)
        {
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(@"DELETE FROM t_tasks 
                WHERE c_taskid=@taskid", _conn);

                cm.Parameters.AddWithValue("@taskid", int.Parse(taskId));
                _conn.Close();
                _conn.Open();
                cm.ExecuteNonQuery();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<List<t_Task>> GetAllByEmp(string empId)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cm = new NpgsqlCommand("SELECT * FROM t_tasks", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cm.ExecuteReader();

            if (datar.HasRows)
            {
                dt.Load(datar);
            }

            List<t_Task> contactList = new List<t_Task>();
            contactList = (from DataRow dr in dt.Rows
                           where dr["c_empid"].ToString() == empId //particular employee
                           select new t_Task()
                           {
                               c_taskId = Convert.ToInt32(dr["c_taskid"]),
                               c_title = dr["c_title"].ToString(),
                               c_description = dr["c_description"].ToString(),
                               c_estimatedDays = Convert.ToInt32(dr["c_estimateddays"].ToString()),
                               c_startDate = (DateTime)dr["c_startdate"],
                               c_endDate = (DateTime)dr["c_enddate"],
                               c_status = dr["c_status"].ToString(),
                           }).ToList();
            _conn.Close();
            return contactList;
        }

        public async Task<t_Task> GetOne(string taskId)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cm = new NpgsqlCommand("SELECT * FROM t_tasks WHERE c_taskid=@taskid", _conn);
            cm.Parameters.AddWithValue("@taskid", int.Parse(taskId));
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cm.ExecuteReader();
            t_Task task = null;

            if (datar.Read())
            {
                task = new t_Task()
                {
                    c_taskId = Convert.ToInt32(datar["c_taskid"]),
                    c_title = (string)datar["c_title"],
                    c_description = datar["c_description"].ToString(),
                    c_estimatedDays = Convert.ToInt32(datar["c_estimateddays"].ToString()),
                    c_startDate = (DateTime)datar["c_startdate"],
                    c_endDate = (DateTime)datar["c_enddate"],
                    c_status = datar["c_status"].ToString(),

                };
            }
            _conn.Close();
            return task;
        }

        public async Task<int> Update(t_Task data)
        {
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(@"UPDATE t_tasks SET
           c_empid=@c_empid,
           c_title=@c_title,
           c_description=@c_description,
           c_estimateddays=@c_estimateddays,
           c_startdate=@c_startdate,
           c_enddate=@c_enddate,
           c_status=@c_status
           WHERE c_taskid=@c_taskid;", _conn);

                cm.Parameters.AddWithValue("@c_empid", data.c_empId);
                cm.Parameters.AddWithValue("@c_title", data.c_title);
                cm.Parameters.AddWithValue("@c_description", data.c_description);
                cm.Parameters.AddWithValue("@c_estimateddays", data.c_estimatedDays);
                cm.Parameters.AddWithValue("@c_startdate", data.c_startDate);
                cm.Parameters.AddWithValue("@c_enddate", data.c_endDate);
                cm.Parameters.AddWithValue("@c_status", data.c_status);
                cm.Parameters.AddWithValue("@c_taskid", data.c_taskId);


                _conn.Close();
                _conn.Open();
                cm.ExecuteNonQuery();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }


        }
    }
}