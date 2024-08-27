using AutoMapper;
using Backend.Core.DtoModels;
using Backend.Core.Interfaces;
using Backend.Core.Repository;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Backend.Controllers
{
    [Route("content/[controller]")]
    [ApiController]
    public class ContentController : Controller
    {
        private readonly IContentRepository _contentRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ContentController(IContentRepository contentRepository, DataContext context, IMapper mapper)
        {
            _contentRepository = contentRepository;
            _dataContext = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Content>))]
        public IActionResult GetContents()
        {
            var contents = _mapper.Map<List<ContentDto>>(_contentRepository.GetContents());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(contents);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CreateContentDto))]
        [ProducesResponseType(400)]
        public IActionResult AddContent([FromBody] CreateContentDto contentAdd) 
        {
            if (contentAdd == null)
            {
                return BadRequest(ModelState);
            }

            var contentExists = _contentRepository.GetContents()
                .Any(c => c.Title.Trim().ToUpper() == contentAdd.Title.TrimEnd().ToUpper());

            if (contentExists)
            {
                ModelState.AddModelError("", "Content already exists!");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contentMap = _mapper.Map<Content>(contentAdd);

            if (!_contentRepository.AddContent(contentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving the content.");
                return StatusCode(500, ModelState);
            }

            return Ok("Created");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("title/{title}")]
        [ProducesResponseType(200, Type = typeof(ContentDto))]
        [ProducesResponseType(404)]
        public IActionResult GetContentByTitle(string title)
        {
            var content = _mapper.Map<ContentDto>(_contentRepository.GetContentByTitle(title));

            if (content == null)
            {
                return NotFound();
            }

            return Ok(content);
        }


    }
}
