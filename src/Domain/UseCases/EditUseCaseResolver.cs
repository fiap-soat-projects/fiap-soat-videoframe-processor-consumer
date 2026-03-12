using Domain.Entities.Enums;
using Domain.UseCases.Interfaces;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases;

public class EditUseCaseResolver : IEditUseCaseResolver
{
    private readonly IEnumerable<IEditUseCase> _editUseCases;
    private readonly ILogger<EditUseCaseResolver> _logger;

    public EditUseCaseResolver(ILogger<EditUseCaseResolver> logger, IEnumerable<IEditUseCase> editUseCases)
    {
        _editUseCases = editUseCases;
        _logger = logger;
    }

    public IEditUseCase Resolve(EditType editType)
    {
        var useCase = _editUseCases.FirstOrDefault(x => x.EditType == editType);

        if (useCase == null) 
        {
            _logger.LogError("EditType not supported: {EditType}", editType.ToString());
            throw new Exception("EditType not supported");
        }

        return useCase;
    }
}
