using AutoMapper;
using BLL.DTO.Response;
using DAL.Models;

namespace BLL.Configurations;

public class PilotResolver :  IValueResolver<Aircraft,AircraftResponse,List<AircraftPilotResponse>>
{
    public List<AircraftPilotResponse> Resolve(
        Aircraft source,
        AircraftResponse destination,
        List<AircraftPilotResponse> destMember,
        ResolutionContext context)
    {
        List<AircraftPilotResponse> pilots = new List<AircraftPilotResponse>();

        if (null == source.Pilots)
        {
            return pilots;  
        }
        
        
        foreach (var elem in source.Pilots)
        {
            pilots.Add(new AircraftPilotResponse()
            {
                Id = elem.Id,
                FirstName = elem.FirstName,
                LastName = elem.LastName,
                Age = elem.Age,
                Rating = elem.Rating
            });
        }

        return pilots;
    }
}