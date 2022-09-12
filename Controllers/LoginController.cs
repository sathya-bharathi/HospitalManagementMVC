using HospitalManagementMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HospitalManagementMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HMSDbContext db;
        private readonly ISession session;
        public LoginController(HMSDbContext _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AUTOREDIRECT
        public class NoDirectAccessAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                var canAcess = false;

                // check the refer
                var referer = filterContext.HttpContext.Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referer))
                {
                    var rUri = new System.UriBuilder(referer).Uri;
                    var req = filterContext.HttpContext.Request;
                    if (req.Host.Host == rUri.Host && req.Host.Port == rUri.Port && req.Scheme == rUri.Scheme)
                    {
                        canAcess = true;
                    }
                }

                // ... check other requirements

                if (!canAcess)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Index", area = "" }));
                }
            }
        }
        #endregion


        public IActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Admin(Admin r)
        {


            var result = (from i in db.Admins
                          where i.AdminId == r.AdminId && i.Password == r.Password
                          select i).SingleOrDefault();
            if (result != null)
            {
                HttpContext.Session.SetString("AdminId", result.AdminId);
                HttpContext.Session.SetString("Name", result.Name);
                return RedirectToAction("Index", "Admin");
            }
            else
                return View();

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Patient()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Patient(PatientRegistration r)
        {
            var result = (from i in db.PatientRegistrations
                          where i.PatientId == r.PatientId && i.Password == r.Password
                          select i).SingleOrDefault();
            if (result != null)
            {
                HttpContext.Session.SetString("PatientId", result.PatientId);
                HttpContext.Session.SetString("PatientName", result.PatientName);
                return RedirectToAction("Index", "Patient");
            }
            else
                return View();

        }
        public IActionResult PLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Patient", "Login");
        }

        public IActionResult Doctor()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Doctor(DoctorRegistration r)
        {
            var result = (from i in db.DoctorRegistrations
                          where i.DoctorId == r.DoctorId && i.Password == r.Password
                          select i).SingleOrDefault();
            if (result != null)
            {
                HttpContext.Session.SetString("DoctorId", result.DoctorId);
                HttpContext.Session.SetString("DcotorName", result.DoctorName);
                return RedirectToAction("Index", "Doctor");
            }
            else
                return View();

        }
        public IActionResult DLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Doctor", "Login");
        }
    }
}
