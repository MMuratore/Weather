namespace Weather.SharedKernel.Domain.Exception;

public sealed class DomainForbiddenException : DomainException
{
    public DomainForbiddenException()
    {
    }

    public DomainForbiddenException(string code) : base(code)
    {
        Code = code;
    }
    
    public DomainForbiddenException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public DomainForbiddenException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}