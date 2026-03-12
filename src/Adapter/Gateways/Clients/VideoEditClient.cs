using Domain.Entities.Enums;
using Domain.Gateways.Clients.Interfaces;
using Infrastructure.Clients.DTOs;
using Infrastructure.Clients.Interfaces;

namespace Adapter.Gateways.Clients;

internal class VideoEditClient : IVideoEditClient
{
    private readonly IVideoFrameClient _videoFrameClient;

    public VideoEditClient(IVideoFrameClient videoFrameClient)
    {
        _videoFrameClient = videoFrameClient;
    }

    public async Task UpdateAsync(string editId, string userId, EditStatus editStatus, CancellationToken cancellationToken)
    {
        var req = new UpdateEditionStatusRequest(userId, editStatus);

        await _videoFrameClient.UpdateEditStatusAsync(editId, req, cancellationToken);
    }
}
