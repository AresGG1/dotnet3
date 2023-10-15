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
        CreateMap<Aircraft, AircraftResponse>();
    }
    
    private void CreatePilotMap()
    {
        CreateMap<PilotRequest, Pilot>();
        CreateMap<Pilot, PilotResponse>();
    }
}
