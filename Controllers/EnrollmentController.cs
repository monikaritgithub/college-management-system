using Microsoft.AspNetCore.Mvc;
using OracleSampleProject.Models;

namespace OracleSampleProject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly StudentModel _studentModel;
        private readonly CourseModel _courseModel;

        public EnrollmentController(StudentModel studentModel, CourseModel courseModel)
        {
            _studentModel = studentModel;
            _courseModel = courseModel;
        }

        // GET: Enrollment/Enroll
        public IActionResult Enroll()
        {
            // You can implement logic here to fetch available courses and display them in a view
            // For demonstration purposes, I'm assuming you have a view named "Enroll.cshtml" to display available courses
            var courses = _courseModel.GetCoursesList();
            return View(courses);
        }

        // POST: Enrollment/Enroll
        [HttpPost]
        public IActionResult Enroll(int studentId, string courseId)
        {
            // Implement logic to enroll the student in the selected course
            _studentModel.EnrollInCourse(studentId, courseId);

            // Redirect to a success page or back to the enrollment page
            return RedirectToAction("Enroll");
        }

        // POST: Enrollment/Withdraw
        [HttpPost]
        public IActionResult Withdraw(int studentId, string courseId)
        {
            // Implement logic to withdraw the student from the selected course
            _studentModel.WithdrawFromCourse(studentId, courseId);

            // Redirect to a success page or back to the enrollment page
            return RedirectToAction("Enroll");
        }
    }
}
