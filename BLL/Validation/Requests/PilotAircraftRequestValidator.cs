using BLL.DTO.Request;
using FluentValidation;

namespace BLL.Validation.Requests;

public class PilotAircraftRequestValidator : AbstractValidator<PilotAircraftRequest>
{

    public PilotAircraftRequestValidator()
    {
        RuleFor(aircraftPilot => aircraftPilot.AircraftId)
            .NotEmpty()
            .WithMessage(aircraftPilot => $"{nameof(aircraftPilot.AircraftId)} can't be empty.");

        RuleFor(aircraftPilot => aircraftPilot.PilotId)
            .NotEmpty()
            .WithMessage(aircraftPilot => $"{nameof(aircraftPilot.PilotId)} can't be empty.");
    }

}