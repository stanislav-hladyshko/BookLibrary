using System.Collections.Generic;
using AutoMapper;
using BooksLibrary.Dal.Models;
using CommonData.Models;

namespace CommonData.Mapping
{
    public class DtoToEntityMappingProfile : Profile
    {
        public DtoToEntityMappingProfile()
        {
            CreateMap<AuthorDto, Author>();
            CreateMap<IEnumerable<AuthorDto>, IEnumerable<Author>>();
            CreateMap<BookDto, Book>();
            CreateMap<IEnumerable<BookDto>, IEnumerable<Book>>();
        }
    }
}