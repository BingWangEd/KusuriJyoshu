interface IPrescriptionEditor
{
    Task AddPrescriptionAsync(string Content, int patientId, CancellationToken cancellationToken);
    Task EditPrescriptionAsync(string Content, int prescriptionId, CancellationToken cancellationToken);
    Task DeletePrescriptionAsync(int id, CancellationToken cancellationToken);
}
