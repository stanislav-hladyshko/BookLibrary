#nullable enable
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BooksLibrary.Bll;
using CommonData.CustomValidationAttributes;
using CommonData.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BooksLibrary.Api.Controllers
{
    /// <summary>
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    public class BooksController : BaseController
    {
        private readonly BooksService _booksService;

        /// <summary>
        /// </summary>
        public BooksController()
        {
            _booksService = new BooksService();
        }

        /// <summary>
        ///     Get list of all books
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <response code="200">List of items</response>
        [HttpGet]
        [Route("/Books/GetBooksList")]
        [SwaggerResponse(200, type: typeof(IEnumerable<BookDto>))]
        public async Task<IActionResult> GetBooksList([FromQuery] [MinValue(0)] int? pageSize = DefaultPageSize,
                                                      [FromQuery] [MinValue(0)] int? pageNumber = DefaultPageNumber)
        {
            IEnumerable<BookDto> books = await _booksService.GetBooksListAsync(pageSize ?? DefaultPageSize,
                pageNumber ?? DefaultPageNumber);

            return ResolveResponse(books);
        }

        /// <summary>
        ///     Gets books sorted by author name
        /// </summary>
        /// <param name="authorName"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <response code="200">List of Books sorted by author</response>
        [HttpGet]
        [Route("/Books/GetBooksByAuthor")]
        [SwaggerResponse(200, type: typeof(IEnumerable<BookDto>))]
        public async Task<IActionResult> GetBooksByAuthor([FromQuery] [Required] [DataType(DataType.Text)] string authorName,
                                                          [FromQuery] [MinValue(0)] int? pageSize = DefaultPageSize,
                                                          [FromQuery] [MinValue(0)] int? pageNumber = DefaultPageNumber)
        {
            IEnumerable<BookAuthorDto> books = await _booksService.GetBooksByAuthorAsync(pageSize ?? DefaultPageSize,
                                                             pageNumber ?? DefaultPageNumber,
                                                             authorName);

            return ResolveResponse(books);
        }

        /// <summary>
        ///     Gets List of Authors
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <response code="200">List of items</response>
        [HttpGet]
        [Route("/Books/GetAuthorsList")]
        [SwaggerResponse(200, type: typeof(IEnumerable<AuthorDto>))]
        public async Task<IActionResult> GetAuthorsList([FromQuery] [MinValue(0)] int? pageSize = DefaultPageSize,
                                                        [FromQuery] [MinValue(0)] int? pageNumber = DefaultPageNumber)
        {
            IEnumerable<AuthorDto> authorDto = await _booksService.GetAuthorsAsync(pageSize ?? DefaultPageSize,
                                                                            pageNumber ?? DefaultPageNumber);

            return ResolveResponse(authorDto);
        }

        /// <summary>
        ///     Gets book by book name
        /// </summary>
        /// <param name="bookName"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <response code="200">List of items</response>
        [HttpGet]
        [Route("/Books/GetBookByTitle")]
        [SwaggerResponse(200, type: typeof(IEnumerable<BookDto>))]
        public async Task<IActionResult> GetBookByTitle([FromQuery] [Required] string bookName,
                                                        [FromQuery] [MinValue(0)] int? pageSize = DefaultPageSize,
                                                        [FromQuery] [MinValue(0)] int? pageNumber = DefaultPageNumber)
        {
            IEnumerable<BookDto> authorDto = await _booksService.GetBookByBookName(bookName,pageSize ?? DefaultPageSize,
                                                                            pageNumber ?? DefaultPageNumber);

            return ResolveResponse(authorDto);
        }
        
        

        /// <summary>
        ///     Create new book
        /// </summary>
        /// <param name="newBook"></param>
        /// <param name="authors"></param>
        /// <response code="200">Book was created</response>
        [HttpPut]
        [Route("/Books/CreateBook")]
        [SwaggerResponse(200)]
        public async Task<IActionResult> CreateBook([FromQuery] [Required] BookDto newBook,
                                                    [FromBody] [Required] List<AuthorDto> authors)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _booksService.CreateNewBookAsync(newBook, authors);
            return Ok();
        }

        /// <summary>
        ///     Update exist book
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="pageAmount"></param>
        /// <param name="graduationYear"></param>
        /// <param name="bookDescription"></param>
        /// <param name="bookTitle"></param>
        /// <response code="200">Book updated</response>
        [HttpPut]
        [Route("/Books/UpdateBookByBookId")]
        [SwaggerResponse(200)]
        public async Task<IActionResult> UpdateBookByBookId([Required] int bookId,
                                                            [MinValue(0)] int pageAmount,
                                                            [BookGraduationYear(-1800)] int graduationYear,
                                                            [DataType(DataType.Text)] string bookDescription = "",
                                                            [DataType(DataType.Text)] string bookTitle = "")
        {
            if (!ModelState.IsValid) return BadRequest();
           
            if (await _booksService.IsIdExist(bookId) == false)
               return BadRequest(new Error() {Code = 400, Message = "Id is not valid"});
            
            await _booksService.UpdateBookAsync(bookId,
                                                bookTitle,
                                                bookDescription,
                                                graduationYear,
                                                pageAmount);
            return Ok();
        }

        /// <summary>
        ///     Delete book
        /// </summary>
        /// <param name="bookId"></param>
        /// <response code="200">Book was deleted</response>
        [HttpDelete]
        [Route("/Books/DeleteBook")]
        [SwaggerResponse(200)]
      //  [SwaggerRequestExample(typeof(BookAuthorDto), typeof(BookAuthorDto))]
        public async Task<IActionResult> DeleteBook([Required] [FromQuery] int bookId)
        {
            if (await _booksService.IsIdExist(bookId) == false)
               return BadRequest(new Error(){Code = 400, Message = "Id is not valid"});

            await _booksService.DeleteBookAsync(bookId);
            
            return Ok();
        }
    }
}