// CourseController.cs
using Microsoft.AspNetCore.Mvc;
using OracleSampleProject.Models;
using System;

namespace OracleSampleProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly string _connectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

        public IActionResult Index()
        {
            CourseModel courseData = new CourseModel(_connectionString);
            courseData.GetCourses();

            ViewBag.courseData = courseData;
            return View();
        }

        public IActionResult ShowCourse()
        {
            CourseModel courseData = new CourseModel(_connectionString);
            courseData.GetCourses();

            ViewBag.courseData = courseData;
            return View();
        }

        public IActionResult AddCourse()
        {
            return View(new CourseModel(_connectionString));
        }

        [HttpPost]
        public IActionResult AddCourse(CourseModel model)
        {
            if (String.IsNullOrEmpty(model.CourseTitle))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {
                int lastId = GetLastCourseIdFromDatabase(); // Implement this method to get the last ID

                CourseModel getFormData = new CourseModel(_connectionString);
              //  getFormData.CourseId = (lastId + 1).ToString();
                getFormData.CourseId = model.CourseId;
                getFormData.CourseTitle = model.CourseTitle;

                getFormData.AddCourse(getFormData);
                ViewBag.Message = "Success: Value will be inserted into the database";
                return View();
            }
        }

        private int GetLastCourseIdFromDatabase()
        {
            int lastId = 0; // Implement the logic to get the last ID from the database
            // Replace this with the actual logic to get the last ID from your database
            // You can use the GetLastCourseId method from your CourseModel class if available.

            return lastId;
        }

        public IActionResult EditCourse(string id)
        {
            // Assuming CourseId is a string; adjust the parameter type accordingly
            CourseModel courseModel = new CourseModel(_connectionString);

            // Implement logic to get details of the course with the specified ID
            CourseModel course = courseModel.GetCourseById(id);

            // Pass the course details to the view for editing
            return View(course);
        }

        [HttpPost]
        public IActionResult EditCourse(CourseModel model)
        {
            if (String.IsNullOrEmpty(model.CourseTitle))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View(model);
            }
            else
            {
                CourseModel courseModel = new CourseModel(_connectionString);

                // Implement logic to update the course details in the database
                courseModel.UpdateCourse(model);

                ViewBag.Message = "Success: Value will be updated in the database";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DeleteCourse(string courseId)
        {
            CourseModel courseModel = new CourseModel(_connectionString);

            // Implement logic to delete the course record with the specified ID
            courseModel.DeleteCourse(courseId);

            // Redirect to the page displaying the updated list or perform any other action
            return RedirectToAction("Index");
        }
    }
}
