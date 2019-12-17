using AutoMapper;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourseParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CourseLibrary.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        readonly IMapper mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            this.mapper = mapper;
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(
          [FromQuery] AuthorsResourseParameters authorsResourseParameters)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourseParameters);
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        [HttpGet("{authorId}",Name ="GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);
            if (authorFromRepo == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author) 
        {
            var authorEntity = mapper.Map<Entities.Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor", 
                new { authorId = authorToReturn.Id }, 
                authorToReturn);
        }

        [HttpPut("{authorId}")]
        public IActionResult UpdateAuthor(Guid authorId, AuthorForUpdateDto author)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return BadRequest();
            }

            var authorEntity = _courseLibraryRepository.GetAuthor(authorId);
            mapper.Map(author, authorEntity);
            _courseLibraryRepository.UpdateAuthor(authorEntity);
            _courseLibraryRepository.Save();

            return NoContent();
        }

        [HttpPatch("{authorId}")]
        public IActionResult PatchAuthor(Guid authorId, JsonPatchDocument<AuthorForUpdateDto> patchDocument)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return BadRequest();
            }

            var authorEntity = _courseLibraryRepository.GetAuthor(authorId);
            if (authorEntity == null) 
            {
                return NotFound();
            }
            var authorToPatch = mapper.Map<AuthorForUpdateDto>(authorEntity);
            patchDocument.ApplyTo(authorToPatch);

            //mapper.Map(author, authorEntity);
            //_courseLibraryRepository.UpdateAuthor(authorEntity);
            //_courseLibraryRepository.Save();

            return Ok();
        }
    }
}
