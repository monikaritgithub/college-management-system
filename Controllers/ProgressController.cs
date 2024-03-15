using Microsoft.AspNetCore.Mvc;
using OracleSampleProject.Models;
using System;

namespace OracleSampleProject.Controllers
{
    public class ProgressController : Controller
    {
        private readonly string _connectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

        public IActionResult Index()
        {
            ProgressModel progressData = new ProgressModel();
            // Assume you have a method to retrieve progress data from the database
            progressData.GetProgressDataFromDatabase(_connectionString);

            ViewBag.ProgressData = progressData;
            return View();
        }

        public IActionResult ShowProgress()
        {
            ProgressModel progressData = new ProgressModel();
            progressData.GetProgressDataFromDatabase(_connectionString);

            return View(progressData.ProgressList);
        }

        public IActionResult AddProgress()
        {
            return View(new ProgressModel());
        }

        [HttpPost]
        public IActionResult AddProgress(ProgressModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProgressModel progressModel = new ProgressModel();

                    // Implement the logic to add progress data to the database
                    progressModel.AddProgressDataToDatabase(model, _connectionString);

                    ViewBag.Message = "Success: Progress added successfully.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "Error: Invalid model state. Please check your input.";
            }

            return View();
        }

        public IActionResult EditProgress(string courseId)
        {
            //if (string.IsNullOrEmpty(courseId))
            //{
            //    return BadRequest("CourseId is required for editing.");
            //}

            ProgressModel progressData = new ProgressModel();
            progressData.GetProgressDataFromDatabase(_connectionString);

            //ProgressModel progressToEdit = progressData.ProgressList.Find(p => p.CourseId == courseId);
            ProgressModel progressToEdit = progressData.ProgressList.Find(p => p.CourseId.Equals(courseId, StringComparison.OrdinalIgnoreCase));

            if (progressToEdit == null)
            {
                return NotFound("Progress not found for the provided CourseId.");
            }

            return View(progressToEdit);
        }

        [HttpPost]
        public IActionResult EditProgress(ProgressModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProgressModel progressModel = new ProgressModel();

                    // Implement the logic to update progress data in the database
                    // Assuming CourseId is not editable, otherwise, update the appropriate identifier
                    // (e.g., StudentName, if it's a unique identifier)
                    // Example query: UPDATE ProgressTable SET StudentName = :newStudentName, ... WHERE CourseId = :courseId

                    ViewBag.Message = "Success: Progress updated successfully.";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = $"Error: {ex.Message}";
                }
            }
            else
            {
                ViewBag.Message = "Error: Invalid model state. Please check your input.";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                return BadRequest("CourseId is required for deletion.");
            }

            try
            {
                ProgressModel progressModel = new ProgressModel();

                // Implement the logic to delete progress data from the database
                // Example query: DELETE FROM ProgressTable WHERE CourseId = :courseId

                ViewBag.Message = "Success: Progress deleted successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return RedirectToAction("ShowProgress");
        }



    }



}
