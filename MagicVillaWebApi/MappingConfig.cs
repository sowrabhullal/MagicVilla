using AutoMapper;
using MagicVillaWebApi.Models;
using MagicVillaWebApi.Models.Dto;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDto>().ReverseMap();
        CreateMap<Villa, VillaCreateDto>().ReverseMap();
        CreateMap<Villa, VillaUpdateDto>().ReverseMap();
    }
}
