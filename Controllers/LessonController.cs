// LessonController.cs
using Microsoft.AspNetCore.Mvc;
using OracleSampleProject.Models;
using System;

namespace OracleSampleProject.Controllers
{
    public class LessonController : Controller
    {
        private readonly string _connectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

        public IActionResult Index()
        {
            LessonModel lessonData = new LessonModel(_connectionString);
            lessonData.GetLessons();

            ViewBag.lessonData = lessonData;
            return View();
        }

        public IActionResult ShowLesson()
        {
            LessonModel lessonData = new LessonModel(_connectionString);
            lessonData.GetLessons();

            ViewBag.lessonData = lessonData;
            return View();
        }

        public IActionResult AddLesson()
        {
            return View(new LessonModel(_connectionString));
        }

        [HttpPost]
        public IActionResult AddLesson(LessonModel model)
        {
            if (String.IsNullOrEmpty(model.LessonTitle))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {
                int lastLessonNo = GetLastLessonNoFromDatabase(model.CourseId); // Implement this method to get the last lesson number

                LessonModel getFormData = new LessonModel(_connectionString);
                getFormData.CourseId = model.CourseId;
                getFormData.LessonNo = lastLessonNo + 1;
                getFormData.LessonTitle = model.LessonTitle;

                getFormData.AddLesson(getFormData);
                ViewBag.Message = "Success: Value will be inserted into the database";
                return View();
            }
        }

        private int GetLastLessonNoFromDatabase(string courseId)
        {
            int lastLessonNo = 0; // Implement the logic to get the last lesson number from the database
            // You can use the GetLastLessonNo method from your LessonModel class if available.

            return lastLessonNo;
        }

        public IActionResult EditLesson(string courseId, int lessonNo)
        {
            LessonModel lessonData = new LessonModel(_connectionString);

            // Implement logic to get details of the lesson with the specified CourseId and LessonNo
            LessonModel lesson = lessonData.GetLessonByCourseIdAndLessonNo(courseId, lessonNo);

            // Pass the lesson details to the view
            return View(lesson);
        }

        [HttpPost]
        public IActionResult EditLesson(LessonModel model)
        {
            if (String.IsNullOrEmpty(model.LessonTitle))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View(model);
            }
            else
            {
                // Implement logic to update the lesson details in the database
                LessonModel lessonData = new LessonModel(_connectionString);
                lessonData.EditLesson(model);

                ViewBag.Message = "Success: Value will be updated in the database";
                return RedirectToAction("ShowLesson");
            }
        }

        [HttpPost]
        public IActionResult DeleteLesson(string courseId, int lessonNo)
        {
            // Implement logic to delete the lesson with the specified CourseId and LessonNo
            LessonModel lessonData = new LessonModel(_connectionString);
            lessonData.DeleteLesson(courseId, lessonNo);

            // Redirect to the page displaying the updated lesson list or perform any other action
            return RedirectToAction("ShowLesson");
        }
    }
}
