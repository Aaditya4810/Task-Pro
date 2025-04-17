using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using TaskManagement.Models;
using TaskManagement.Repositories.Interface;

namespace TaskManagement.Repositories.Implements
{
    public class EmployeeRepository : IEmployeeInterface
    {
        private readonly NpgsqlConnection _conn;

        public EmployeeRepository(NpgsqlConnection conn)
        {
            _conn = conn;
        }

        public async Task<int> Add(t_Employee employee)
        {
            try
            {
                await _conn.CloseAsync();
                NpgsqlCommand comcheck = new NpgsqlCommand(@"SELECT * FROM t_employees3 WHERE c_email=@email;", _conn);
                comcheck.Parameters.AddWithValue("@email", employee.c_empEmail);
                await _conn.OpenAsync();

                using (NpgsqlDataReader dr = await comcheck.ExecuteReaderAsync())
                {
                    if (dr.HasRows)
                    {
                        await _conn.CloseAsync();
                        return 0;
                    }
                    else
                    {
                        await _conn.CloseAsync();
                        NpgsqlCommand cm = new NpgsqlCommand(@"INSERT INTO t_employees3 (c_empname,c_email,c_password,c_profile_image)
                         VALUES
                                                       (@empname,@email,@password,@image);", _conn);

                        cm.Parameters.AddWithValue("@empname", employee.c_empName);
                        cm.Parameters.AddWithValue("@email", employee.c_empEmail);
                        cm.Parameters.AddWithValue("@password", employee.c_empPassword);


                        cm.Parameters.AddWithValue("@image", employee.c_profileImage);

                        await _conn.CloseAsync();
                        await _conn.OpenAsync();
                        await cm.ExecuteNonQueryAsync();
                        await _conn.CloseAsync();
                        return 1;
                    }
                }

            }
            catch (Exception ex)
            {
                await _conn.CloseAsync();
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<t_Employee> Login(vm_Login emp)
        {
            t_Employee emp1 = new t_Employee();
            var q1 = "SELECT * FROM t_employees3 WHERE c_email=@email AND c_password=@password;";

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(q1, _conn))
                {
                    cmd.Parameters.AddWithValue("@email", emp.c_empEmail);
                    cmd.Parameters.AddWithValue("@password", emp.c_empPassword);
                    await _conn.OpenAsync();
                    var reader = await cmd.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        emp1.c_empId = (int)reader["c_empid"];
                        emp1.c_empName = (string)reader["c_empname"];
                        emp1.c_empEmail = (string)reader["c_email"];
                        emp1.c_profileImage = (string)reader["c_profile_image"];
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Login Error: " + e.Message);
            }
            finally
            {
                await _conn.CloseAsync();
            }
            return emp1;
        }

        public async Task<int> UpdateProfile(t_UpdateEmployee employee)
        {
            try
            {
                NpgsqlCommand cm = new NpgsqlCommand(@"
                UPDATE t_employees3 SET c_empname=@empname,c_profile_image=@image WHERE c_empid=@empid;", _conn);

                cm.Parameters.AddWithValue("@empname", employee.c_empName);
                cm.Parameters.AddWithValue("@image", employee.c_profileImage);
                cm.Parameters.AddWithValue("@empid", employee.c_empId);
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

        public async Task<t_Employee> GetAllByEmpId(int empid)
        {
            t_Employee emp1 = new t_Employee();
            string q1="SELECT * FROM t_employees3 WHERE c_empid=@empid;";
            try
            {

                using (NpgsqlCommand cm = new NpgsqlCommand(q1, _conn))
                {
                    cm.Parameters.AddWithValue("@empid",empid);
                    _conn.Close();
                    _conn.Open();
                    NpgsqlDataReader datar = cm.ExecuteReader();

                    if (datar.Read())
                    {
                        emp1.c_empId = Convert.ToInt32(datar["c_empid"]);
                        emp1.c_empName = datar["c_empname"].ToString();
                        emp1.c_empEmail = datar["c_email"].ToString();
                        emp1.c_empPassword = datar["c_password"].ToString();
                        emp1.c_profileImage = datar["c_profile_image"].ToString();
                    }    
                    datar.Close();    
                    _conn.Close();   
                }
                return emp1;            
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.Message);
                return null;
            }
            finally
            {
                _conn.Close();
            }
            
        }

       
    }
}