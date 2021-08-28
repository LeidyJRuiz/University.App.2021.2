using System;
using System.ComponentModel.DataAnnotations;

namespace University.BL.DTOs
{
    public class StudentsDTO
    {
        [Required(ErrorMessage = "The ID is required")]
        public int ID { get; set; }

        [Required(ErrorMessage = "The LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The FirstMidName is required")]
        public string FirstMidName { get; set; }

        [Required(ErrorMessage = "The EnrollmentDate is required")]
        public DateTime EnrollmentDate { get; set; }

        [Required(ErrorMessage = "The FullName is required")]
        public string FullName { get; set; }


    }
}
