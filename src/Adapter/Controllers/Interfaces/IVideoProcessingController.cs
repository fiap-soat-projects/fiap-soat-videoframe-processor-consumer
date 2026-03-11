using Adapter.Presenters.DTOs;

namespace Adapter.Controllers.Interfaces;

public interface IVideoProcessingController
{
    Task ProcessAsync(EditInput processorInput, CancellationToken cancellationToken);
}
