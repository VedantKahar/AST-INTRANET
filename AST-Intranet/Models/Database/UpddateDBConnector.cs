using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client; // Assuming you're using Oracle
using System.Web;

namespace AST_Intranet.Models.Database
{
    public class UpdateDBConnector
    {
        //// Method to get today's employee birthdays based on DOB (MM-DD)
        //public static List<string> GetEmployeeBirthdays()
        //{
        //    List<string> birthdays = new List<string>();

        //    try
        //    {
        //        string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

        //        using (OracleConnection connection = new OracleConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Query to fetch employees whose DOB matches today's month and day
        //            string query = @"
        //                SELECT EMP_NAME, DOB
        //                FROM cim_emp_master_list
        //                WHERE TO_CHAR(DOB, 'MM-DD') = TO_CHAR(SYSDATE, 'MM-DD')
        //                AND STATUS = 'Active'"; // Filter to ensure the employee is active

        //            using (OracleCommand command = new OracleCommand(query, connection))
        //            {
        //                OracleDataReader reader = command.ExecuteReader();
        //                while (reader.Read())
        //                {
        //                    string name = reader["EMP_NAME"].ToString();
        //                    DateTime dob = Convert.ToDateTime(reader["DOB"]);

        //                    // Calculate age based on DOB
        //                    int age = DateTime.Now.Year - dob.Year;
        //                    if (dob.Date > DateTime.Now.AddYears(-age)) age--; // Adjust age if birthday hasn't occurred yet this year

        //                    birthdays.Add($"{name} - {age} years");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error fetching birthdays: {ex.Message}");
        //    }

        //    return birthdays;
        //}

        public static List<string> GetEmployeeBirthdays()
        {
            List<string> birthdays = new List<string>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["OracleDbConnection"].ConnectionString;

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Hardcoded date to fetch birthdays for January 1st (01-01)
                    string query = @"
                            SELECT EMP_NAME, DOB
                            FROM CIM_EMP_MASTER_LIST
                            WHERE EXTRACT(MONTH FROM DOB) = 1
                            AND EXTRACT(DAY FROM DOB) = 1
                            AND UPPER(STATUS) = 'ACTIVE'; 
                            ;";

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        OracleDataReader reader = command.ExecuteReader();
                        int count = 0; // Debug variable to count birthdays
                        while (reader.Read())
                        {
                            string name = reader["EMP_NAME"].ToString();
                            DateTime dob = Convert.ToDateTime(reader["DOB"]);

                            // Calculate age based on DOB
                            int age = DateTime.Now.Year - dob.Year;
                            if (dob.Date > DateTime.Now.AddYears(-age)) age--; // Adjust age if birthday hasn't occurred yet this year

                            birthdays.Add($"{name} - {age} years");

                            // Log for debugging
                            count++;
                        }

                        // Log the count of birthdays retrieved
                        Console.WriteLine($"Found {count} birthdays on January 1st.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching birthdays: {ex.Message}");
            }
         
            return birthdays;
        }


    }
}
