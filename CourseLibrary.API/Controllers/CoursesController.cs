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

        [HttpGet("{courseId}", Name ="GetCoursesForAuthor")]
        public ActionResult<CourseDto> CourseForAuthor(Guid authorId, Guid courseId)
        {
            var courseForAuthor = courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseForAuthor == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CourseDto>(courseForAuthor));
        }

        [HttpPost]
        public ActionResult<CourseDto> CreateCourse(Guid authorId, CourseForCreationDto course)
        {
            if (!courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = mapper.Map<Entities.Course>(course);
            courseLibraryRepository.AddCourse(authorId, courseEntity);
            courseLibraryRepository.Save();

            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCoursesForAuthor", new {
                authorId = authorId,
                courseId= courseToReturn.Id 
            }, courseToReturn);
        }
    }
}
