using Infrastructure.Clients.DTOs;
using Infrastructure.Clients.Interfaces;
using System.Net.Http.Json;

namespace Infrastructure.Clients;

public class VideoFrameClient : IVideoFrameClient
{
    private readonly HttpClient _httpClient;

    public VideoFrameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task UpdateEditStatusAsync(
        string editId,
        UpdateEditionStatusRequest req,
        CancellationToken cancellationToken)
    {
        await _httpClient.PatchAsJsonAsync($"/v1/user/videos/edits/{editId}/status", req, cancellationToken);
    }
}
