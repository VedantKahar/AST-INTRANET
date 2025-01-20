using System;
using System.Collections.Generic;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace AST_Intranet.Models.Database
{
    public class EmployeeDBConnector
    {
        // Method to get total number of employees
        public static int GetTotalEmployees()
        {
            int totalEmployees = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalEmployees = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total employees: {ex.Message}");
            }
            return totalEmployees;
        }

        // Method to get new joiners (past month)
        public static int GetNewJoiners()
        {
            int newJoiners = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master WHERE join_date >= :joinDate";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter(":joinDate", OracleDbType.Date)).Value = DateTime.Now.AddMonths(-1);
                        newJoiners = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching new joiners: {ex.Message}");
            }
            return newJoiners;
        }

        // Method to get total number of departments
        public static int GetTotalDepartments()
        {
            int totalDepartments = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM department_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        totalDepartments = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total departments: {ex.Message}");
            }
            return totalDepartments;
        }

        // Method to get department details
        public static List<string> GetDepartments()
        {
            List<string> departments = new List<string>();
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DEPT_NAME FROM department_master";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                departments.Add(reader.GetString(reader.GetOrdinal("DEPT_NAME")));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching departments: {ex.Message}");
            }
            return departments;
        }


        public static List<Dictionary<string, object>> GetEmployeesByDepartment(int departmentId, int page, int pageSize)
        {
            List<Dictionary<string, object>> employees = new List<Dictionary<string, object>>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Calculate the starting row for pagination
                    int startRow = (page - 1) * pageSize;

                    string query = "SELECT * FROM (SELECT emp.*, ROWNUM AS rn FROM cim_emp_master emp WHERE DEPARTMENT = :departmentId) WHERE rn BETWEEN :startRow AND :endRow";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentId", departmentId));
                        command.Parameters.Add(new OracleParameter("startRow", startRow + 1));  // Oracle ROWNUM starts from 1
                        command.Parameters.Add(new OracleParameter("endRow", startRow + pageSize));

                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string columnName = reader.GetName(i);
                                    object columnValue = reader.GetValue(i);
                                    employee.Add(columnName, columnValue);
                                }

                                employees.Add(employee);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching employees: {ex.Message}");
            }

            return employees;
        }

        public static int GetTotalEmployeesInDepartment(int departmentId)
        {
            int totalEmployees = 0;
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM cim_emp_master WHERE DEPARTMENT = :departmentId";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        command.Parameters.Add(new OracleParameter("departmentId", departmentId));
                        totalEmployees = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching total employees: {ex.Message}");
            }
            return totalEmployees;
        }

    }
}
