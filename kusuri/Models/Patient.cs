using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace kusuri.Models.Models;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Instant CreatedAt { get; set; }
}
