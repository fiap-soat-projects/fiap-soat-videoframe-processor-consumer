using Domain.Entities.Enums;

namespace Infrastructure.Clients.DTOs;

public record UpdateEditionStatusRequest(string userId, EditStatus Status)
{

}
