using System;
using System.Collections; // For ArrayList
using System.Collections.Generic; // For using Dictionary.
using System.Data; // For CommandType Property
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Microsoft.AspNetCore.HttpOverrides;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace OracleSampleProject.Models
{
    public class StudentModel
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Contact { get; set; }

        public DateTime Dob { get; set; }

        public string Country { get; set; }

        public string Ecourse { get; set; }

        public DateTime Edate { get; set; }
        public List<StudentModel> StudentList = new List<StudentModel>();
        public List<Enrollment> Enrollments { get; set; }

        public void GetStudents()
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "SELECT id,name,email,contact,dob,country,ecourse,edate FROM student";
                    OracleCommand cmd = new OracleCommand(queryString, con);
                    cmd.BindByName = true;
                    cmd.CommandType = CommandType.Text;

                    // Connect with the database, and select the data.
                    con.Open();
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        StudentModel sm = new StudentModel();
                        sm.ID = reader.GetInt16(0);
                        sm.Name = reader.GetString(1);
                        sm.Email = reader.GetString(2);
                        sm.Contact = reader.GetString(3);
                        sm.Dob = reader.GetDateTime(4);
                        sm.Country = reader.GetString(5);
                        sm.Ecourse = reader.GetString(6);
                        sm.Edate = reader.GetDateTime(7);






                        StudentList.Add(sm);

                    }
                    reader.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        /* public void AddStudent(StudentModel student)
         {
             //try
             //{


             string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";
                 using (OracleConnection con = new OracleConnection(conString))
                 {
                     string queryString = "INSERT INTO STUDENT (id, name, email, contact, dob, ecourse, edate, country) VALUES ('" + student.ID + "','" + student.Name + "','" + student.Email + "' , '" + student.Contact + "','" + student.Dob + "','" + student.Ecourse + "','" + student.Edate + "','" + student.Country + "')";

                     OracleCommand cmd = new OracleCommand(queryString, con);
                     con.Open();
                     cmd.ExecuteNonQuery();
                     con.Close();
                 }
           //  }
            // catch (Exception ex)
             //{
               //  Console.WriteLine(ex.Message);

             //}
         }
        */

        private int GetLastStudentId(OracleConnection con)
        {
            // Retrieve the last ID from the database
            string queryString = "SELECT MAX(id) FROM STUDENT";

            using (OracleCommand cmd = new OracleCommand(queryString, con))
            {
                con.Open();
                int lastId = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
                return lastId;
            }
        }

        public void AddStudent(StudentModel student)
        {
           // try
            //{

                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

                using (OracleConnection con = new OracleConnection(conString))
                {

                    // Get the last ID from the database
                    int lastId = GetLastStudentId(con);

                    // Increment the last ID by 1 for the new student
                    student.ID = lastId + 1;


                    // insert operation
                    string queryString = "INSERT INTO STUDENT (id, name, email, contact, dob, ecourse, edate, country) " +
                                         "VALUES (:id, :name, :email, :contact, :dob, :ecourse, :edate, :country)";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        // Add parameters with their values
                        cmd.Parameters.Add(":id", OracleDbType.Int32).Value = student.ID;
                        cmd.Parameters.Add(":name", OracleDbType.Varchar2, 255).Value = student.Name;
                        cmd.Parameters.Add(":email", OracleDbType.Varchar2, 255).Value = student.Email;
                        cmd.Parameters.Add(":contact", OracleDbType.Varchar2, 255).Value = student.Contact;
                        cmd.Parameters.Add(":dob", OracleDbType.Date).Value = student.Dob;
                        cmd.Parameters.Add(":ecourse", OracleDbType.Varchar2, 20).Value = student.Ecourse;
                        cmd.Parameters.Add(":edate", OracleDbType.Date).Value = student.Edate;
                        cmd.Parameters.Add(":country", OracleDbType.Varchar2, 20).Value = student.Country;

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
         //   }
           // catch (Exception ex)
            //{
              //  Console.WriteLine(ex.Message);
            //}
        }

        //public static void DeleteStudent(int studentId)
        //{
        //    //try
        //    //{
        //        using (OracleConnection con = new OracleConnection("User Id=icp;Password=123;Data Source=localhost:1521/orcl;"))
        //        {
        //            con.Open();

        //            // Implement the SQL query to delete the student record
        //            string deleteQuery = "DELETE FROM STUDENT WHERE ID = :studentId";

        //            using (OracleCommand cmd = new OracleCommand(deleteQuery, con))
        //            {
        //                cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;

        //                cmd.ExecuteNonQuery();
        //            }

        //            con.Close();
        //        }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // Handle the exception as needed (logging, rethrow, etc.)
        //    //    Console.WriteLine(ex.Message);
        //    //    throw;
        //    //}
        //}

        public static StudentModel GetStudentById(int studentId)
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

                using (OracleConnection con = new OracleConnection(conString))
                {
                    con.Open();

                    // Implement the SQL query to get the student details by ID
                    string queryString = "SELECT id, name, email, contact, dob, country, ecourse, edate FROM student WHERE id = :studentId";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Create a new StudentModel and populate it with data from the database
                                return new StudentModel
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Contact = reader.GetString(3),
                                    Dob = reader.GetDateTime(4),
                                    Country = reader.GetString(5),
                                    Ecourse = reader.GetString(6),
                                    Edate = reader.GetDateTime(7)
                                };
                            }
                        }
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed (logging, rethrow, etc.)
            }

            // Return null if no student with the specified ID is found
            return null;
        }


        public void EditStudent(StudentModel student)
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

                using (OracleConnection con = new OracleConnection(conString))
                {
                    // Update operation
                    string queryString = "UPDATE STUDENT SET name = :name, email = :email, " +
                                         "contact = :contact, dob = :dob, ecourse = :ecourse, " +
                                         "edate = :edate, country = :country WHERE id = :id";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        // Add parameters with their values
                        cmd.Parameters.Add(":name", OracleDbType.Varchar2, 255).Value = student.Name;
                        cmd.Parameters.Add(":email", OracleDbType.Varchar2, 255).Value = student.Email;
                        cmd.Parameters.Add(":contact", OracleDbType.Varchar2, 255).Value = student.Contact;
                        cmd.Parameters.Add(":dob", OracleDbType.Date).Value = student.Dob;
                        cmd.Parameters.Add(":ecourse", OracleDbType.Varchar2, 20).Value = student.Ecourse;
                        cmd.Parameters.Add(":edate", OracleDbType.Date).Value = student.Edate;
                        cmd.Parameters.Add(":country", OracleDbType.Varchar2, 20).Value = student.Country;
                        cmd.Parameters.Add(":id", OracleDbType.Int32).Value = student.ID;

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteStudent(int studentId)
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

                using (OracleConnection con = new OracleConnection(conString))
                {
                    con.Open();

                    // Implement the SQL query to delete the student record
                    string deleteQuery = "DELETE FROM STUDENT WHERE ID = :studentId";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, con))
                    {
                        cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;

                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void EnrollInCourse(int studentId, string courseId)
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "INSERT INTO Enrollment (StudentId, CourseId) VALUES (:studentId, :courseId)";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed (logging, rethrow, etc.)
            }
        }

        public void WithdrawFromCourse(int studentId, string courseId)
        {
            try
            {
                string conString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

                using (OracleConnection con = new OracleConnection(conString))
                {
                    string queryString = "DELETE FROM Enrollment WHERE StudentId = :studentId AND CourseId = :courseId";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        con.Open();

                        cmd.Parameters.Add(":studentId", OracleDbType.Int32).Value = studentId;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = courseId;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed (logging, rethrow, etc.)
            }
        }

    }
}
