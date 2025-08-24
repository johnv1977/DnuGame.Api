using FluentValidation;
using DnuGame.Api.Modules.Rooms.DTOs;

namespace DnuGame.Api.Modules.Rooms.Validators;

public class UpdateRoomDtoValidator : AbstractValidator<UpdateRoomDto>
{
    public UpdateRoomDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Color)
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("Color must be a valid hex format (#RRGGBB)")
            .When(x => !string.IsNullOrEmpty(x.Color));

        RuleFor(x => x.Icon)
            .MaximumLength(100)
            .WithMessage("Icon cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Icon));

        RuleFor(x => x.UserLimit)
            .InclusiveBetween(2, 50)
            .WithMessage("UserLimit must be between 2 and 50")
            .When(x => x.UserLimit.HasValue);
    }
}
