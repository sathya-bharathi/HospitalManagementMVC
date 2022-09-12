using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementMVC.Models
{
    public partial class Admin
    {
        [Required]
        public string AdminId { get; set; } = null!;
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string? Password { get; set; }
    }
}
