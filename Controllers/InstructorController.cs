using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using OracleSampleProject.Models;
using System;
using System.Data;

namespace OracleSampleProject.Controllers
{
    public class InstructorController : Controller
    {
        private readonly string _connectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

        public IActionResult Index()
        {
            InstructorModel instructorData = new InstructorModel(_connectionString);
            instructorData.GetInstructors();

            ViewBag.instructorData = instructorData;
            return View(instructorData.InstructorList);
        }

        public IActionResult ShowInstructor()
        {
            InstructorModel instructorData = new InstructorModel(_connectionString);
            instructorData.GetInstructors();

            ViewBag.instructorData = instructorData;
            return View(instructorData.InstructorList);
        }

        public IActionResult AddInstructor()
        {
            return View(new InstructorModel(_connectionString));
        }

        [HttpPost]
        public IActionResult AddInstructor(InstructorModel model)
        {
            if (String.IsNullOrEmpty(model.InstructorName))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {
                int lastId = GetLastInstructorIdFromDatabase(); // Implement this method to get the last ID

                InstructorModel getFormData = new InstructorModel(_connectionString);
                getFormData.InstructorId = lastId + 1;
                getFormData.InstructorName = model.InstructorName;
                getFormData.InstructorCountry = model.InstructorCountry;
                getFormData.ContactNo = model.ContactNo;
                getFormData.Email = model.Email;
                getFormData.CourseId = model.CourseId;

                getFormData.AddInstructor(getFormData);
                ViewBag.Message = "Success: Value will be inserted into the database";
                return View();
            }
        }
        public IActionResult EditInstructor(int id)
        {
            try
            {
                // Assuming you have a method to get an instructor by ID in your InstructorModel
                InstructorModel instructorData = new InstructorModel(_connectionString);
                InstructorModel instructor = instructorData.GetInstructorById(id);

                if (instructor != null)
                {
                    return View(instructor);
                }
                else
                {
                    ViewBag.Message = "Instructor not found.";
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult EditInstructor(InstructorModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Assuming you have a method to update an instructor in your InstructorModel
                    model.UpdateInstructor(model);

                    ViewBag.Message = "Instructor updated successfully.";
                    return RedirectToAction("ShowInstructor");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
                return View("Error");
            }
        }

        public IActionResult DeleteInstructor(int id)
        {
            try
            {
                // Implement logic to delete the instructor with the specified ID
                // Example: Assuming you have a method in the InstructorModel to delete by ID
                DeleteInstructorById(id);

                ViewBag.Message = "Success: Instructor deleted.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.Message = "Error: Unable to delete instructor.";
            }

            return RedirectToAction("ShowInstructor");
        }




        private int GetLastInstructorIdFromDatabase()
        {
            // Implement the logic to get the last ID from the database
            // You can use the GetLastInstructorId method from your InstructorModel class if available.
            int lastId = 0; // Replace with actual logic
            return lastId;
        }

      

        private void DeleteInstructorById(int id)
        {
            InstructorModel instructorModel = new InstructorModel(_connectionString);
            // Assuming you have a method to delete an instructor by ID in your InstructorModel
            instructorModel.DeleteInstructorById(id);
           
        }
    }
}
