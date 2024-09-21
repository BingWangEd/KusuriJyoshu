interface IPrescriptionEditor
{
    Task<int> AddPrescriptionAsync(string Content, int patientId, CancellationToken cancellationToken);
    Task<bool> EditPrescriptionAsync(string Content, int prescriptionId, CancellationToken cancellationToken);
    Task<bool> DeletePrescriptionAsync(int id, CancellationToken cancellationToken);
}
