using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace kusuri.Models.Models;

[Index(nameof(PatientId))]
public class Chat
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public int PatientId { get; set; }
    public bool ByBot { get; set; }
    public Instant CreatedAt { get; set; }
}
