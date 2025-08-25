using System.ComponentModel.DataAnnotations;

namespace DnuGame.Api.Common.DTO;

public record ErrorResponse(string Type, string Title, string Detail, int Status);
