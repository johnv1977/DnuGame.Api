using FluentValidation;
using DnuGame.Api.Modules.Rooms.DTOs;
using DnuGame.Api.Modules.Rooms.Repositories;

namespace DnuGame.Api.Modules.Rooms.Validators;

public class CreateRoomDtoValidator : AbstractValidator<CreateRoomDto>
{
    private readonly IRoomRepository _roomRepository;

    public CreateRoomDtoValidator(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug is required")
            .MaximumLength(50)
            .WithMessage("Slug cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("Slug can only contain alphanumeric characters and hyphens")
            .MustAsync(async (slug, cancellation) => !await _roomRepository.ExistsBySlugAsync(slug))
            .WithMessage("Slug already exists");

        RuleFor(x => x.Color)
            .NotEmpty()
            .WithMessage("Color is required")
            .Matches(@"^#[0-9A-Fa-f]{6}$")
            .WithMessage("Color must be a valid hex format (#RRGGBB)");

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithMessage("Icon is required")
            .MaximumLength(100)
            .WithMessage("Icon cannot exceed 100 characters");

        RuleFor(x => x.UserLimit)
            .NotEmpty()
            .WithMessage("UserLimit is required")
            .InclusiveBetween(2, 50)
            .WithMessage("UserLimit must be between 2 and 50");
    }
}
