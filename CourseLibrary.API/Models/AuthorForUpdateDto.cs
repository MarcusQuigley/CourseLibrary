using System;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    public class AuthorForUpdateDto
    {
        [Required(ErrorMessage ="Please enter a First Name.")]
        [MaxLength(50,ErrorMessage ="Limit 50 characters only for First Name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter a Last Name.")]
        [MaxLength(50, ErrorMessage = "Limit 50 characters only for Last Name.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter a Date of Birth.")]
        public DateTimeOffset DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please enter a Category.")]
        [MaxLength(50, ErrorMessage = "Limit 50 characters only for Category.")]
        public string MainCategory { get; set; }
    }
}
