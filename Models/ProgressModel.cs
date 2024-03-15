using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace OracleSampleProject.Models
{
    public class ProgressModel
    {
        public string StudentName { get; set; }
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int LessonNo { get; set; }
        public string LessonTitle { get; set; }
        public string LessonStatus { get; set; }
        public DateTime LastAccessedDate { get; set; }
        public string CourseInstructorName { get; set; }

        public List<ProgressModel> ProgressList = new List<ProgressModel>();

        // You may need additional properties depending on the requirements

        public ProgressModel()
        {
            // Default constructor
            LastAccessedDate = DateTime.Now;
        }


        // Additional constructors if needed

        public void AddProgress(ProgressModel progress)
        {
            // Method to add progress information to the list
            ProgressList.Add(progress);
        }
        public void AddProgressDataToDatabase(ProgressModel model, string connectionString)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();

                    // Implement the logic to add progress data to the database
                    string queryString = "INSERT INTO ProgressTable (StudentName, CourseId, CourseTitle, LessonNo, LessonTitle, LessonStatus, LastAccessedDate, CourseInstructorName) " +
                                         "VALUES (:studentName, :courseId, :courseTitle, :lessonNo, :lessonTitle, :lessonStatus, :lastAccessedDate, :courseInstructorName)";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        // Add parameters with their values
                        cmd.Parameters.Add(":studentName", OracleDbType.Varchar2, 255).Value = model.StudentName;
                        cmd.Parameters.Add(":courseId", OracleDbType.Varchar2, 50).Value = model.CourseId;
                        cmd.Parameters.Add(":courseTitle", OracleDbType.Varchar2, 255).Value = model.CourseTitle;
                        cmd.Parameters.Add(":lessonNo", OracleDbType.Int32).Value = model.LessonNo;
                        cmd.Parameters.Add(":lessonTitle", OracleDbType.Varchar2, 255).Value = model.LessonTitle;
                        cmd.Parameters.Add(":lessonStatus", OracleDbType.Varchar2, 50).Value = model.LessonStatus;
                        cmd.Parameters.Add(":lastAccessedDate", OracleDbType.Date).Value = model.LastAccessedDate;
                        cmd.Parameters.Add(":courseInstructorName", OracleDbType.Varchar2, 255).Value = model.CourseInstructorName;

                        // Execute the query
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed (logging, rethrow, etc.)
                throw;
            }
        }
        public void GetProgressDataFromDatabase(string connectionString)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();

                    string queryString = "SELECT * FROM ProgressTable";

                    using (OracleCommand cmd = new OracleCommand(queryString, con))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();

                        // Assuming ProgressModel properties match the table columns
                        while (reader.Read())
                        {
                            ProgressModel progress = new ProgressModel
                            {
                               
                                StudentName = reader.GetString(1),
                                CourseId = reader.GetString(2),
                                CourseTitle = reader.GetString(3),
                                LessonNo = reader.GetInt32(4),
                                LessonTitle = reader.GetString(5),
                                LessonStatus = reader.GetString(6),
                                LastAccessedDate = reader.GetDateTime(7),
                                CourseInstructorName = reader.GetString(8)
                            };

                            // Add progress data to a list or process it as needed
                            ProgressList.Add(progress);
                        }

                        reader.Dispose();
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle the exception as needed (logging, rethrow, etc.)
                throw;
            }
        }
        public void ClearProgress()
        {
            // Method to clear progress information
            ProgressList.Clear();
        }
    }
}
