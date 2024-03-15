using Microsoft.AspNetCore.Mvc;
using OracleSampleProject.Models;
using System.Diagnostics;

namespace OracleSampleProject.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            StudentModel sqldata = new StudentModel();
            sqldata.GetStudents();
            //StudentModel students = new StudentModel();
            //students.OnGet();

            ViewBag.sqldata = sqldata;
            return View();
        }
        public IActionResult Students()
        {
            StudentModel sqldata = new StudentModel();
            sqldata.GetStudents();
            //StudentModel students = new StudentModel();
            //students.OnGet();

            ViewBag.sqldata = sqldata;
            return View();
        }

        public IActionResult AddStudent()
        {
            return View(new StudentModel());
        }

        [HttpPost]
        public IActionResult AddStudent(StudentModel model)
        {
            /* Make sure that inputted value isn't null or empty */
            if (String.IsNullOrEmpty(model.Name) || String.IsNullOrEmpty(model.Email))
            {
                ViewBag.Message = "Error: Don't submit an empty value.";
                return View();
            }
            else
            {
                // Fetch the last ID from the database or any other source
                // model.ID = GetLastStudentIdFromDatabase(); // Implement this method to get the last ID

                StudentModel getFormData = new StudentModel();
                getFormData.ID = model.ID;
                getFormData.Name = model.Name;
                getFormData.Email = model.Email;
                getFormData.Contact = model.Contact;
                getFormData.Dob = model.Dob;
                getFormData.Country = model.Country;
                getFormData.Ecourse = model.Ecourse;
                getFormData.Edate = model.Edate;

                getFormData.AddStudent(getFormData);
                ViewBag.Message = "Success: Value will be inserted into database";
                return View();


            }
        }
        private int GetLastStudentIdFromDatabase()
        {
            int lastId = 0; // Implement the logic to get the last ID from the database
                            // Replace this with the actual logic to get the last ID from your database
                            // You can use the GetLastStudentId method from your StudentModel class if available.

            return lastId;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Delete(int studentId)
        {
            // Create an instance of StudentModel
            StudentModel studentModel = new StudentModel();

            // Implement logic to delete the student record with the specified ID
            // Example: Assuming you have a DeleteStudent method in StudentModel
            studentModel.DeleteStudent(studentId);

            // Redirect to the page displaying the updated list or perform any other action
            return RedirectToAction("Index");
        }

        public IActionResult EditStudent(int id)
        {
            // Create an instance of StudentModel
            StudentModel studentModel = new StudentModel();

            // Implement logic to get details of the student with the specified ID
            StudentModel student = StudentModel.GetStudentById(id);

            // Check if the student is found
            if (student != null)
            {
                // Pass the student details to the view for editing
                return View(student);
            }
            else
            {
                // Handle the case where the student is not found
                return NotFound(); // You can customize this as per your application needs
            }
        }

        [HttpPost]
        public IActionResult EditStudent(StudentModel model)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Update the student in the database
                StudentModel studentModel = new StudentModel();
                studentModel.EditStudent(model);
                ViewBag.Message = "Success: Value will be updated in the database";
                return RedirectToAction("Index");
            }
            else
            {
                // If the model state is not valid, return to the edit view with validation errors
                return View(model);
            }
        }




    }
}