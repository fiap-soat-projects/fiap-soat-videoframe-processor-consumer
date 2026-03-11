using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Exceptions;

[ExcludeFromCodeCoverage]
public abstract class DomainException(string message) : Exception(message)
{

}