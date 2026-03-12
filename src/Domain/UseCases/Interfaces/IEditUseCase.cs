using Domain.Entities;
using Domain.Entities.Enums;

namespace Domain.UseCases.Interfaces;

public interface IEditUseCase
{
    public EditType EditType { get; }
    Task ProcessAsync(VideoJob job, CancellationToken cancellationToken);
}
