using System.Security.Cryptography;
using kusuri.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.Extensions;

namespace kusuri.Classes;

public class PrescriptionEditor (
    AppDbContext appDbContext
) : IPrescriptionEditor {    
    public async Task AddPrescriptionAsync(string content, int patientId, CancellationToken cancellationToken)
    {
        var trimmedContent = content.Trim();
        if (string.IsNullOrEmpty(trimmedContent))
        {
            Console.WriteLine("Nothing to save");
            return;
        }

        var now = SystemClock.Instance.GetCurrentInstant();

        var prescription = new PrescriptionPrompt{
            Content = trimmedContent,
            Status = Status.Active,
            CreatedAt = now,
            ModifiedAt = now,
            PatientId = patientId
        };
        await appDbContext.AddAsync(prescription);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task EditPrescriptionAsync(string content, int prescriptionId, CancellationToken cancellationToken)
    {
        var trimmedContent = content.Trim();
        var prescription = await appDbContext.PrescriptionPrompts
            .Where(p => p.Id == prescriptionId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (prescription == null)
        {
            throw new Exception("処方箋が見つからない");
        }

        if (trimmedContent == prescription.Content)
        {
            Console.WriteLine("Nothing to change");
            return;
        };

        prescription.Content = trimmedContent;
        prescription.ModifiedAt = SystemClock.Instance.GetCurrentInstant();
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeletePrescriptionAsync(int id, CancellationToken cancellationToken)
    {
        var prescription = await appDbContext.PrescriptionPrompts.Where(
            p => p.Id == id
        ).SingleOrDefaultAsync(cancellationToken);

        if (prescription == null)
        {
            throw new Exception("処方箋が見つからない");
        }

        prescription.Status = Status.Deleted;
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}