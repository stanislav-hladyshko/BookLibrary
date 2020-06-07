using AutoMapper;
using BooksLibrary.Dal.Models;
using CommonData.Models;

namespace CommonData.Mapping
{
    public class EntityToDtoMappingProfile : Profile
    {
        public EntityToDtoMappingProfile()
        {
            CreateMap<Author, AuthorDto>() /*.ForMember(dest=>dest.Id ,opt => opt.MapFrom(x=>x.Id))
                .ForMember(dest=>dest.Name ,opt => opt.MapFrom(x=>x.Name))
                .ForMember(dest=>dest.DateOfBirth ,opt => opt.MapFrom(x=>x.DateOfBirth));*/;

            CreateMap<Book, BookDto>() /*.ForMember(dest => dest.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(dest => dest, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(dest => dest.GraduationYear, opt => opt.MapFrom(x => x.GraduationYear))
                .ForMember(dest => dest.PagesAmount, opt => opt.MapFrom(x => x.PagesAmount))*/;
        }
    }
}