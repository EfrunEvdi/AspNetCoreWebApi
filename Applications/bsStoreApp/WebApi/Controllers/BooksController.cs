using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBook()
        {
            var books = _context.Books.ToList();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (book is null)
                {
                    return NotFound(); // 404
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); // 400
                }

                _context.Books.Add(book);
                _context.SaveChanges();

                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute] int id, [FromBody] Book book)
        {
            try
            {
                var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (entity is null)
                {
                    return NotFound(); // 404
                }

                // Check Id
                if (id != book.Id)
                {
                    return BadRequest(); // 400
                }

                entity.Title = book.Title;
                entity.Price = book.Price;

                _context.SaveChanges();
                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (entity is null)
                {
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    }); // 404
                }

                _context.Books.Remove(entity);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPatch]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

            if (entity is null)
            {
                return NotFound(); // 404
            }

            bookPatch.ApplyTo(entity);
            return NoContent(); // 204
        }
    }
}
