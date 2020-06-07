using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BooksLibrary.Dal;
using BooksLibrary.Dal.Models;
using CommonData.Mapping;
using CommonData.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Bll
{
    public class BooksService
    {
        private readonly Mapper _dtoToEntityMapper;
        private readonly Mapper _entityToDtoMapper;

        public BooksService()
        {
            _dtoToEntityMapper = new MappingConfiguration().DtoToEntityMapperConfiguration();
            _entityToDtoMapper = new MappingConfiguration().EntityToDtoMapperConfiguration();
        }

        public async Task<List<BookDto>> GetBooksListAsync(int pageSize, int pageNumber)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            IQueryable<Book> baseQuery = bookLibraryContext.Books.AsQueryable();

            baseQuery = baseQuery.Skip(--pageNumber * pageSize).Take(pageSize);

            List<Book> result = await baseQuery.ToListAsync();

            List<BookDto> mappedResult = _entityToDtoMapper.Map<List<BookDto>>(result);

            return mappedResult;
        }

        public async Task<List<BookAuthorDto>> GetBooksByAuthorAsync(int pageSize,
                                                                     int pageNumber,
                                                                     string authorName)
        {
            try
            {
                using BookLibraryContext bookLibraryContext = new BookLibraryContext();

                var getBooksByAuthorQuery = from bookAuthor in bookLibraryContext.BooksAuthors
                                            join author in bookLibraryContext.Authors on bookAuthor.AuthorId equals author.Id
                                            join book in bookLibraryContext.Books on bookAuthor.BookId equals book.Id
                                            where author.Name.Contains(authorName)
                                            select new
                                            {
                                                AuthorName = author.Name,
                                                BookName = book.Name
                                            };

                var paginationQuery = getBooksByAuthorQuery.Skip(--pageNumber * pageSize).Take(pageSize);

                var getBooksByAuthor = await paginationQuery.AsNoTracking().ToListAsync();

                return getBooksByAuthor.Select(book => new BookAuthorDto
                {
                    AuthorName = book.AuthorName,
                    BookName = book.BookName
                }).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        ///     Get list of authors
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<List<AuthorDto>> GetAuthorsAsync(int pageSize, int pageNumber)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            List<Author> result = await bookLibraryContext.Authors.Skip(--pageNumber * pageSize)
                                                                  .Take(pageSize)
                                                                  .AsNoTracking().ToListAsync();

            return _entityToDtoMapper.Map<List<AuthorDto>>(result);
        }

        /// <summary>
        ///     Get list of authors
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="bookName"></param>
        /// <returns></returns>
        public async Task<List<BookDto>> GetBookByBookName(string bookName, int pageSize, int pageNumber)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            var baseQuery = bookLibraryContext.Books.Where(x => x.Name.Contains(bookName));

            baseQuery = baseQuery.Skip(--pageNumber * pageSize).Take(pageSize);

            List<Book> result = await baseQuery.ToListAsync();

            return _entityToDtoMapper.Map<List<BookDto>>(result);
        }

        /// <summary>
        ///     Create new book with, at least, one author
        /// </summary>
        /// <param name="newBook"></param>
        /// <param name="authors"></param>
        /// <returns></returns>
        public async Task CreateNewBookAsync(BookDto newBook, List<AuthorDto> authors)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            Book mappedBookResult = _dtoToEntityMapper.Map<Book>(newBook);

            await bookLibraryContext.Books.AddAsync(mappedBookResult);

            await bookLibraryContext.SaveChangesAsync();

            //TODO: Move foreach into separate method
            foreach (AuthorDto author in authors)
            {
                Author mappedAuthor = _dtoToEntityMapper.Map<Author>(author);

                await bookLibraryContext.Authors.AddAsync(mappedAuthor);

                await bookLibraryContext.SaveChangesAsync();

                int authorId = mappedAuthor.Id;

                await bookLibraryContext.BooksAuthors.AddAsync(
                    new BookAuthor
                    {
                        AuthorId = authorId,
                        BookId = mappedBookResult.Id
                    });
            }

            await bookLibraryContext.SaveChangesAsync();
        }

        /// <summary>
        ///     Update book by id
        /// </summary>
        /// <param name="bookTitle"></param>
        /// <param name="bookId"></param>
        /// <param name="bookDescription"></param>
        /// <param name="graduationYear"></param>
        /// <param name="pageAmount"></param>
        /// <returns></returns>
        public async Task UpdateBookAsync(int bookId,
                                          string bookTitle,
                                          string bookDescription,
                                          int graduationYear,
                                          int pageAmount)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            Book bookToUpdate = await bookLibraryContext.Books.FirstOrDefaultAsync(x => x.Id == bookId);

            if (bookToUpdate == null)
                return;

            if (!string.IsNullOrEmpty(bookDescription))
                bookToUpdate.Description = bookDescription;

            if (!string.IsNullOrEmpty(bookTitle))
                bookToUpdate.Name = bookTitle;

            bookToUpdate.GraduationYear = graduationYear;

            bookToUpdate.PagesAmount = pageAmount;

            await bookLibraryContext.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            Book bookToDelete = bookLibraryContext.Books.FirstOrDefault(x => x.Id == bookId);

            if (bookToDelete != null)
            {
                bookLibraryContext.Books.Remove(bookToDelete);

                List<BookAuthor> bookAuthors = await bookLibraryContext.BooksAuthors.Where(x => x.BookId == bookId)
                    .ToListAsync();

                if (bookAuthors != null)
                    bookLibraryContext.BooksAuthors.RemoveRange(bookAuthors);

                await bookLibraryContext.SaveChangesAsync();

                //Delete author if author don't have more books
                foreach (var bookAuthor in bookAuthors)
                {
                    List<BookAuthor> haveAuthorEnyBooks = await bookLibraryContext.BooksAuthors
                                                                                  .Where(x => x.AuthorId == bookAuthor.AuthorId)
                                                                                  .AsNoTracking()
                                                                                  .ToListAsync();

                    if (haveAuthorEnyBooks.Count == 0)
                        await DeleteAuthorById(bookAuthor.AuthorId);

                }
            }
            await bookLibraryContext.SaveChangesAsync();
        }

        public async Task<bool> IsIdExist(int idToCheck)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            Book result = await bookLibraryContext.Books.FirstOrDefaultAsync(x => x.Id == idToCheck);

            return result != null;
        }

        public async Task DeleteAuthorById(int authodId)
        {
            using BookLibraryContext bookLibraryContext = new BookLibraryContext();

            Author result = await bookLibraryContext.Authors.FirstOrDefaultAsync(x => x.Id == authodId);

            bookLibraryContext.Authors.Remove(result);

            await bookLibraryContext.SaveChangesAsync();
        }


    }
}