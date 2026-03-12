using Domain.UseCases;
using Domain.UseCases.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Domain.UseCases.EditUseCaseResolverTests;

public abstract class EditUseCaseResolverDependenciesMock
{
    protected readonly ILogger<EditUseCaseResolver> _logger;
    protected readonly IEnumerable<IEditUseCase> _editUseCases;
    protected readonly EditUseCaseResolver _sut;

    protected EditUseCaseResolverDependenciesMock()
    {
        _logger = Substitute.For<ILogger<EditUseCaseResolver>>();
        _editUseCases = new List<IEditUseCase>();

        _sut = new EditUseCaseResolver(_logger, _editUseCases);
    }

    protected EditUseCaseResolver CreateSutWithEditUseCases(IEnumerable<IEditUseCase> editUseCases)
    {
        return new EditUseCaseResolver(_logger, editUseCases);
    }
}
