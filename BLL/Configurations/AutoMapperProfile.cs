using AutoMapper;
using BLL.DTO.Request;
using BLL.DTO.Response;
using DAL.Models;

namespace BLL.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateAircraftMap();
        CreatePilotMap();
    }
    
    private void CreateAircraftMap()
    {
        CreateMap<AircraftRequest, Aircraft>();
        
        //Response
        CreateMap<Aircraft, AircraftResponse>()
            .ForMember(dest => dest.Pilots, 
                opt => opt.MapFrom<PilotResolver>());
    }
    
    private void CreatePilotMap()
    {
        CreateMap<PilotRequest, Pilot>();
        
        //Response
        CreateMap<Pilot, PilotResponse>()
            .ForMember(dest => dest.Aircrafts,
                opt => opt.MapFrom<AircraftResolver>());
        
    }
}
