using HospitalManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Mail;
using static HospitalManagementMVC.Controllers.LoginController;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly HMSDbContext _context;

        public PatientController(HMSDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        public IActionResult AppointmentSelectDoctor()
        {
            
                var DoctorList = new SelectList(from i in _context.DoctorRegistrations
                                                select i.DoctorName).ToList();
                ViewBag.DoctorName = DoctorList;

                return View();
        }
        [HttpPost]
        [NoDirectAccess]
        public IActionResult AppointmentSelectDoctor( AppointmentBooking? a)
        {

            
            if (a.AppointmentTime == null)
            {


                // Step1 Completed - Doctor Selected

                DoctorRegistration doctor = (from i in _context.DoctorRegistrations.Where(d => d.DoctorName == a.DoctorName) select i).SingleOrDefault();
                DateTime starttime = DateTime.ParseExact(doctor.StartTime,"HH.mm",null);
                DateTime endtime = DateTime.ParseExact(doctor.EndTime, "HH.mm", null);

                HttpContext.Session.SetString("EmailId", doctor.DoctorId);
                HttpContext.Session.SetString("DoctorName", doctor.DoctorName);
              
                List<string> AvailableSlots = new();
                

                    while (starttime < endtime)
                    {
                        DateTime timeinterval1, timeinterval2;
                        timeinterval1 = starttime;

                        starttime = starttime.AddMinutes(30);

                        timeinterval2 = starttime;

                        if (starttime < endtime)
                        {
                            AvailableSlots.Add(timeinterval1.ToString("HH.mm") + " to " + timeinterval2.ToString("HH.mm"));

                        }
                        else
                        {
                            AvailableSlots.Add(timeinterval1.ToString("HH.mm") + " to " + endtime.ToString("HH.mm"));
                        }

                    }

                    List<SelectListItem> item = AvailableSlots.ConvertAll(a =>
                    {
                        return new SelectListItem()
                        {
                            Text = a.ToString(),
                            Value = a.ToString(),

                            Selected = false,

                        };
                    });
                    ViewBag.item = item;


                }
            else
             {
                // Step2 Completed - Appointment time selected

                  #region SESSION
                    ViewBag.EmailId = HttpContext.Session.GetString("EmailId");
                    a.DoctorId = @ViewBag.EmailId;
                    ViewBag.DoctorName = HttpContext.Session.GetString("DoctorName");
                    a.DoctorName = @ViewBag.DoctorName;
                    ViewBag.PatientId = HttpContext.Session.GetString("PatientId");
                    a.PatientId = @ViewBag.PatientId;
                    ViewBag.PatientName = HttpContext.Session.GetString("PatientName");
                    a.PatientName = @ViewBag.PatientName;
                    #endregion

                    _context.AppointmentBookings.Add(a);
                    _context.SaveChanges();


                    #region EMAIL
                    var senderEmail = new MailAddress("librarymanagement13@gmail.com", "Admin-Grace Hospitals");
                    var receiverEmail = new MailAddress(a.PatientId, "Receiver");
                    var password = "kigksgbmzemtqrax";
                    var sub = " Appointment Details ";
                    var body = " Hello " + a.PatientName + "!  Your Appointment is confirmed.                 The Appointment Details are: " + "                           " +
                        "Appointment Id: " + a.AppointmentId + "                                               Appointment Date: " + a.AppointmentDate
                        + "          Appointment Timing: " + a.AppointmentTime + "                            Doctor Name: Dr." + a.DoctorName + "                                                " +
                        "Please make sure to be present at hospital before the Appointment Time. For further queries, Please Contact us! Telephone: 044-44431," +
                        "044-44432,044-44433";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    #endregion

                    return RedirectToAction("Index", "Patient");
               

            }

            return View();

        }
        [HttpGet]
        public IActionResult AppointmentDetails(string Id)
        {
            ViewBag.PatientId = HttpContext.Session.GetString("PatientId");
            Id = @ViewBag.PatientId;
            if (Id != null)
            {
             List<AppointmentBooking> result =  (from i in _context.AppointmentBookings.Include(x=>x.Doctor).Include(x=>x.Patient)  where i.PatientId == Id
                                                   select i).ToList();

                return View(result);
            }
            else
            {
                return RedirectToAction(
                    
                    "Index", "Patient");
            }
        }
    }
}

