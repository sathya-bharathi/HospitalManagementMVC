using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalManagementMVC.Models
{
    public partial class PatientRegistration
    {
        public PatientRegistration()
        {
            AppointmentBookings = new HashSet<AppointmentBooking>();
        }
        [EmailAddress]
        [Display(Name = "Email Id")]
        [Required(ErrorMessage = "Please enter your Email")]
        public string PatientId { get; set; } = null!;
        [Required]
        public string? PatientName { get; set; }
        [Range(1, 120, ErrorMessage = "Enter a valid age")]
        public int? Age { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        public string? Password { get; set; }
        [NotMapped]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public virtual ICollection<AppointmentBooking> AppointmentBookings { get; set; }
    }
}
