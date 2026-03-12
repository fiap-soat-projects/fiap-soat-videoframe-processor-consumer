using Domain.Entities;

namespace Domain.UseCases.Interfaces;

public interface IStorageUseCase
{
    Task<string> GetDownloadUrlAsync(string path, CancellationToken cancellationToken);
    string GetEditPath(Edit edit);
}
