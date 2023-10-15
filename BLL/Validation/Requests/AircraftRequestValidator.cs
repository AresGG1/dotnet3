using BLL.DTO.Request;
using FluentValidation;

namespace BLL.Validation.Requests;

public class AircraftRequestValidator : AbstractValidator<AircraftRequest>
{
    public AircraftRequestValidator()
    {
        RuleFor(aircraft => aircraft.Manufacturer)
            .NotEmpty()
            .WithMessage(aircraft => $"{nameof(aircraft.Manufacturer)} can't be empty.")
            .MaximumLength(25)
            .WithMessage(aircraft => $"{nameof(aircraft.Manufacturer)} can't be longer than 25.");
            
            
        RuleFor(aircraft => aircraft.Model)
            .NotEmpty()
            .WithMessage(aircraft => $"{nameof(aircraft.Model)} can't be empty.")
            .MaximumLength(30)
            .WithMessage(aircraft => $"{nameof(aircraft.Model)} can't be longer than 30.");
        
        RuleFor(aircraft => aircraft.Year)
            .NotEmpty()
            .WithMessage(aircraft => $"{nameof(aircraft.Year)} can't be empty.")
            .InclusiveBetween(1965, DateTime.Now.Year)
            .WithMessage(aircraft => $"{nameof(aircraft.Year)} must be between 1965 and " + DateTime.Now.Year.ToString());
        
        RuleFor(aircraft => aircraft.FlightHours)
            .NotEmpty()
            .WithMessage(aircraft => $"{nameof(aircraft.FlightHours)} can't be empty.")
            .InclusiveBetween(0, 5000)
            .WithMessage(aircraft => $"{nameof(aircraft.FlightHours)} must be between 0 and 5000");
    }
}