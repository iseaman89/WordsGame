using AutoMapper;
using WordsGame.Dtos.Users;
using WordsGame.Models;

namespace WordsGame.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<ApiUser, UserDto>().ReverseMap();
    }
}