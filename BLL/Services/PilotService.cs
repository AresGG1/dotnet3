using AutoMapper;
using BLL.DTO.Request;
using BLL.DTO.Response;
using BLL.Exceptions;
using BLL.Interfaces.Services;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using DAL.Models;
using DAL.Pagination;
using DAL.Parameters;

namespace BLL.Services;

public class PilotService : IPilotService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPilotRepository _pilotRepository;

    public PilotService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _pilotRepository = unitOfWork.PilotRepository;
    }

    public async Task<PagedList<PilotResponse>> GetAsync(PilotParameters parameters)
    {
        var pilots = await _pilotRepository.GetAsync(parameters);
        
        return pilots?.Map(_mapper.Map<Pilot, PilotResponse>);    
    }

    public async Task<PilotResponse> GetByIdAsync(int id)
    {
        var pilot = await _pilotRepository.GetCompleteEntityAsync(id);

        return _mapper.Map<Pilot, PilotResponse>(pilot);
    }

    public async Task<PilotResponse> InsertAsync(PilotRequest request)
    {
        var pilot = _mapper.Map<PilotRequest, Pilot>(request);
        var insertedPilot = await _pilotRepository.InsertAsync(pilot);

        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<Pilot, PilotResponse>(insertedPilot);
    }

    public async Task UpdateAsync(PilotRequest request, int id)
    {
        var existEnt = await _pilotRepository.GetByIdAsync(id);
        _mapper.Map(request, existEnt);
        existEnt.Id = id;
        // await _pilotRepository.UpdateAsync(pilot);

        await _unitOfWork.SaveChangesAsync();

    }

    public async Task DeleteAsync(int id)
    {
        await _pilotRepository.GetByIdAsync(id);
        await _pilotRepository.DeleteAsync(id);
        
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task AssignPilot(PilotAircraftRequest pilotAircraftRequest)
    {
        var pilot = await _pilotRepository.GetCompleteEntityAsync(pilotAircraftRequest.PilotId);

        var pilotAircrafts = pilot.Aircrafts;

        if (CheckForAircraft(pilotAircraftRequest.AircraftId, pilotAircrafts))
        {
            throw new AlreadyAssignedException(
                string.Format("Pilot {0} is already assigned for aircraft {1}",
                pilotAircraftRequest.PilotId, 
                pilotAircraftRequest.AircraftId)
                );
        }

        if (pilotAircrafts?.Count == 5)
        {
            throw new ToManyAircraftsException(
                $"{pilotAircraftRequest.PilotId} has to many aircrafts assigned");
        }

        var aircraft = 
            await _unitOfWork.AircraftRepository.GetByIdAsync(pilotAircraftRequest.AircraftId);

        if (!CheckExperience(aircraft, pilot))
        {
            throw new ExperienceException(
                $"{pilot.Id} has not enough experience for aircraft {aircraft.Id}");
        }

        AddAircraft(pilot, aircraft);

        await _unitOfWork.SaveChangesAsync();
    }

    private bool CheckForAircraft(int aircraftId, List<Aircraft> aircrafts)
    {
        return aircrafts?.Find(aircraft => aircraft.Id == aircraftId) != null;
    }

    private bool CheckExperience(Aircraft aircraft, Pilot pilot)
    {
        return aircraft.FlightHours >= 1000 && pilot.Age < 30;
    }

    private void AddAircraft(Pilot pilot, Aircraft aircraft)
    {
        if (pilot.Aircrafts == null)
        {
            pilot.Aircrafts = new List<Aircraft>();
        }
        
        pilot.Aircrafts.Add(aircraft);
    }

    public async Task RemoveAssignation(PilotAircraftRequest pilotAircraftRequest)
    {
        var pilot = await _pilotRepository.GetCompleteEntityAsync(pilotAircraftRequest.PilotId);

        var aircraft =
            pilot.Aircrafts?.Find(aircraft => aircraft.Id == pilotAircraftRequest.AircraftId);

        if (aircraft == null)
        {
            throw new NotAssignedException(
                $"pilot {pilot.Id} is not assigned for aircraft {pilotAircraftRequest.AircraftId}");
        }

        pilot.Aircrafts.Remove(aircraft);

        await _unitOfWork.SaveChangesAsync();
    }
}
