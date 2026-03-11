using Domain.Entities.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Exceptions;

[ExcludeFromCodeCoverage]
public class InvalidEntityPropertyException<TEntity> : DomainException where TEntity : IDomainEntity
{
    private static readonly string _entityClassName = typeof(TEntity).Name;

    private const string INVALID_ENTITY_PROPERTY_TEMPLATE_MESSAGE = "The property {0} of {1} is invalid";

    protected InvalidEntityPropertyException(string propertyName)
        : base(string.Format(INVALID_ENTITY_PROPERTY_TEMPLATE_MESSAGE, propertyName, _entityClassName))
    {

    }

    public static void ThrowIfNull(object? value, string propertyName)
    {
        if (value is null)
        {
            throw new InvalidEntityPropertyException<TEntity>(propertyName);
        }
    }

    public static void ThrowIfNullOrWhiteSpace(string? value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidEntityPropertyException<TEntity>(propertyName);
        }
    }

    public static void ThrowIfIsEqualOrLowerThanZero(decimal value, string propertyName)
    {
        if (value <= 0)
        {
            throw new InvalidEntityPropertyException<TEntity>(propertyName);
        }
    }

    public static void ThrowIfIsEqualOrLowerThanZero(long value, string propertyName)
    {
        if (value <= 0)
        {
            throw new InvalidEntityPropertyException<TEntity>(propertyName);
        }
    }

    public static void ThrowIfIsEqualOrLowerThanZero(int value, string propertyName)
    {
        if (value <= 0.00M)
        {
            throw new InvalidEntityPropertyException<TEntity>(propertyName);
        }
    }
}
