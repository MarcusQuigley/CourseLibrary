using CourseLibrary.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    [CourseMustBeDifferentFromDescriptionAttribute(ErrorMessage = "Title must be different than Description")]
    public class CourseForCreationDto
    {
        [Required(ErrorMessage ="You should fill out a title")]
        [MaxLength(100, ErrorMessage = "Title should not be more than 100 characters")]
        public string Title { get; set; }
        [MaxLength(1500, ErrorMessage = "Description should not be more than 1500 characters")]
        public string Description { get; set; }
    }
}
