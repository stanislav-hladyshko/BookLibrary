using AutoMapper;

namespace CommonData.Mapping
{
    public class MappingConfiguration
    {
        public Mapper DtoToEntityMapperConfiguration()
        {
            return new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new DtoToEntityMappingProfile())));
        }

        public Mapper EntityToDtoMapperConfiguration()
        {
            return new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new EntityToDtoMappingProfile())));
        }
    }
}