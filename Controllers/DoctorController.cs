using HospitalManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementMVC.Controllers
{
    public class DoctorController : Controller
    {
        private readonly HMSDbContext _context;

        public DoctorController(HMSDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AppointmentDetails(string Id)
        {
            ViewBag.PatientId = HttpContext.Session.GetString("DoctorId");
            Id = @ViewBag.PatientId;
            if (Id != null)
            {
                List<AppointmentBooking> result = (from i in _context.AppointmentBookings.Include(x => x.Doctor).Include(x => x.Patient)
                                                   where i.DoctorId == Id
                                                   select i).ToList();

                return View(result);
            }
            else
            {
                return RedirectToAction("Index", "Doctor");
            }
        }
    }
}
