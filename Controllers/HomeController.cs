using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OracleSampleProject.Models;



namespace OracleSampleProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

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
    }
}
 