namespace Weather.SharedKernel.Domain.Exception;

public sealed class DomainConflictException : DomainException
{
    public DomainConflictException()
    {
    }

    public DomainConflictException(string code) : base(code)
    {
        Code = code;
    }
    
    public DomainConflictException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public DomainConflictException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}