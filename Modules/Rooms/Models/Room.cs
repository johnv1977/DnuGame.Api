using System.ComponentModel.DataAnnotations;

namespace DnuGame.Api.Modules.Rooms.Models;

public class Room
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Slug { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(7)] // Para formato hex #RRGGBB
    public string Color { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Icon { get; set; } = string.Empty;
    
    [Range(2, 50)]
    public int UserLimit { get; set; }
    
    public bool IsOpen { get; set; } = true;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}
