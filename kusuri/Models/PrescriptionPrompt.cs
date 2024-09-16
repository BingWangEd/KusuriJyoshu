using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace kusuri.Models.Models;

[Index(nameof(Id), nameof(VersionId), IsUnique = true)]
public class PrescriptionPrompt
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public Status Status { get; set; }
    public int VersionId { get; set; }
    public Instant CreatedAt { get; set; }
    public int PatientId { get; set; }
}

public enum Status {
    Deleted,
    Inactive,
    Activie
}