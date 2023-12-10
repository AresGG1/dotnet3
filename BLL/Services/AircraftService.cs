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
using MassTransit;
using Shared;

namespace BLL.Services;

public class AircraftService : IAircraftService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IAircraftRepository _aircraftRepository;

    public AircraftService(IUnitOfWork unitOfWork, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _aircraftRepository = unitOfWork.AircraftRepository;
    }
    

    public async Task<PagedList<AircraftResponse>> GetAsync(AircraftParameters parameters)
    {
        var aircrafts = await _aircraftRepository.GetAsync(parameters);
        
        return aircrafts?.Map(_mapper.Map<Aircraft, AircraftResponse>);
        
    }

    public async Task<AircraftResponse> GetByIdAsync(int id)
    {
        var aircraft = await _aircraftRepository.GetCompleteEntityAsync(id);

        return _mapper.Map<Aircraft, AircraftResponse>(aircraft);
    }

    public async Task<AircraftResponse> InsertAsync(AircraftRequest request)
    {
        var aircraft = _mapper.Map<AircraftRequest, Aircraft>(request);
        var insertedAircraft = await _aircraftRepository.InsertAsync(aircraft);

        await _unitOfWork.SaveChangesAsync();

        await _publishEndpoint.Publish<AircraftCreated>(new
        {
            Model = request.Model,
            Manufacturer = request.Manufacturer,
            Year = request.Year,
            FlightHours = request.FlightHours
        });
        
        return _mapper.Map<Aircraft, AircraftResponse>(insertedAircraft);
    }

    public async Task UpdateAsync(AircraftRequest request, int id)
    {
        var existEnt = await _aircraftRepository.GetByIdAsync(id);
        _mapper.Map(request, existEnt);
        // await _aircraftRepository.UpdateAsync(aircraft);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _aircraftRepository.GetByIdAsync(id);
        await _aircraftRepository.DeleteAsync(id);
        
        await _unitOfWork.SaveChangesAsync();
    }
    
}
