using BLL.DTO.Request;
using FluentValidation;

namespace BLL.Validation.Requests;

// public class PilotRequestValidator : AbstractValidator<PilotRequest>
// {
//     public PilotRequestValidator()
//     {
//         RuleFor(pilot => pilot.Age)
//             .NotEmpty()
//             .WithMessage(pilot => $"{nameof(pilot.Age)} can't be empty.")
//             .InclusiveBetween(21, 60)
//             .WithMessage("Age must be between 21 and 60");
//
//         RuleFor(pilot => pilot.LastName)
//             .NotEmpty()
//             .WithMessage(pilot => $"{nameof(pilot.LastName)} can't be empty.")
//             .MaximumLength(30)
//             .WithMessage(pilot => $"{nameof(pilot.LastName)} can't be longer than 30 chars.");
//         
//         RuleFor(pilot => pilot.FirstName)
//             .NotEmpty()
//             .WithMessage(pilot => $"{nameof(pilot.FirstName)} can't be empty.")
//             .MaximumLength(30)
//             .WithMessage(pilot => $"{nameof(pilot.FirstName)} can't be longer than 30 chars.");
//         
//         RuleFor(pilot => pilot.Rating)
//             .NotEmpty()
//             .WithMessage(pilot => $"{nameof(pilot.FirstName)} can't be empty.")
//             .InclusiveBetween(0, 10)
//             .WithMessage("Rating must be between 0.0 and 10.0");
//     }
// }
