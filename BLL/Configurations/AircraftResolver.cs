using AutoMapper;
using BLL.DTO.Response;
using DAL.Models;

namespace BLL.Configurations;

public class AircraftResolver : IValueResolver<Pilot, PilotResponse, List<PilotAircraftResponse>>
{
    public List<PilotAircraftResponse> Resolve(
        Pilot source,
        PilotResponse destination,
        List<PilotAircraftResponse> destMember,
        ResolutionContext context)
    {
        List<PilotAircraftResponse> aircrafts = new List<PilotAircraftResponse>();

        if (null == source.Aircrafts)
        {
            return aircrafts;  
        }
        
        
        foreach (var elem in source.Aircrafts)
        {
            aircrafts.Add(new PilotAircraftResponse()
            {
                Id = elem.Id,
                FlightHours = elem.FlightHours,
                Manufacturer = elem.Manufacturer,
                Model = elem.Model,
                Year = elem.Year
            });
        }

        return aircrafts;
    }
}
