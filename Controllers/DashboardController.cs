// DashboardController.cs
using Microsoft.AspNetCore.Mvc;
using OracleSampleProject.Models;

namespace OracleSampleProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly string _connectionString = "User Id=icp;Password=123;Data Source=localhost:1521/orcl;";

        public IActionResult Index()
        {
            //DashboardModel dashboardData = new DashboardModel();
            //dashboardData.PopulateDashboardData(_connectionString);

            //ViewBag.dashboardData = dashboardData;
            //return View();
            DashboardModel dashboardData = new DashboardModel();
            dashboardData.PopulateDashboardData(_connectionString);

            return View(dashboardData);
        }
    }
}
