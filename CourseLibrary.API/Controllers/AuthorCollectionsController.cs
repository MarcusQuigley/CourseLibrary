using AutoMapper;
using CourseLibrary.API.Helpers;
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
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly ICourseLibraryRepository courseLibraryRepository;
        readonly IMapper mapper;

        public AuthorCollectionsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            this.courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            this.mapper = mapper;
        }

        [HttpGet("({ids})", Name ="GetAuthorCollection")]
        public  ActionResult<IEnumerable<AuthorDto>> AuthorCollection(
            [FromRoute] 
            [ModelBinder(BinderType = typeof(ArrayModelBinding))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var authorEntities = courseLibraryRepository.GetAuthors(ids);
            if (ids.Count() != authorEntities.Count())
            {
                return NotFound();
            }
            
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authorEntities));

        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthors(
            IEnumerable<AuthorForCreationDto> authors)
        {
            var authorsEntity = mapper.Map<IEnumerable<Entities.Author>>(authors);

            foreach (var author in authorsEntity)
            {
                courseLibraryRepository.AddAuthor(author);
            }

            courseLibraryRepository.Save();

            var authorsToReturn = mapper.Map<IEnumerable<AuthorDto>>(authorsEntity);
            var idsAsString = string.Join(",", authorsEntity.Select(a => a.Id));
          
            return CreatedAtRoute("GetAuthorCollection", new { ids = idsAsString }, authorsToReturn);
         }
    }
}
