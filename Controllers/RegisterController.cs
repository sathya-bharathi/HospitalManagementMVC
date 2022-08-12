using HospitalManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementMVC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly HMSDbContext _context;

        public RegisterController(HMSDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PatientRegistration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PatientRegistration(PatientRegistration obj)
        {
            _context.PatientRegistrations.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("Patient", "Login");
        }
    }
}
