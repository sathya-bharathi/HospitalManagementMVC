using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementMVC.Models
{
    public partial class AppointmentBooking
    {
        public int AppointmentId { get; set; }
        [DataType(DataType.Date)]
        //[Compare("DateTime.Today",ErrorMessage ="Enter a Valid Date.")]
        public DateTime? AppointmentDate { get; set; }
        [Required]
        public string? AppointmentTime { get; set; }
        public string? DoctorId { get; set; }
        [NotMapped]
        public string DoctorName { get; set; }
        public string? PatientId { get; set; }
        [NotMapped]
        public string PatientName { get; set; }

        public virtual DoctorRegistration? Doctor { get; set; }
        public virtual PatientRegistration? Patient { get; set; }
    }
}
