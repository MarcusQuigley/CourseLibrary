using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        readonly ICourseLibraryRepository courseLibraryRepository;
        readonly IMapper mapper;

        public CoursesController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]

        public ActionResult<IEnumerable<CourseDto>> CoursesForAuthor(Guid authorId)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var coursesForAuthor = courseLibraryRepository.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(coursesForAuthor));
        }

        [HttpGet("{courseId}")]
        public ActionResult<CourseDto> CourseForAuthor(Guid authorId, Guid courseId)
        {
            var courseForAuthor = courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthor == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CourseDto>(courseForAuthor));
        }
    }
}
