using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace kusuri.Models;

[Index(nameof(Id), IsUnique = true)]
[Index(nameof(PatientId))]
public class PrescriptionPrompt
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public Status Status { get; set; }
    public Instant CreatedAt { get; set; }
    public Instant ModifiedAt { get; set; }
    public int PatientId { get; set; }
}

public enum Status {
    Deleted,
    Active
}