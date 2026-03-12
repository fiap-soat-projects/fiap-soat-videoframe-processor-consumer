using Domain.Entities.Enums;

namespace Domain.UseCases.Interfaces;

public interface IEditUseCaseResolver
{
    IEditUseCase Resolve(EditType editType);
}
