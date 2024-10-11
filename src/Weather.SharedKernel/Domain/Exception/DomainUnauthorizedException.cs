namespace Weather.SharedKernel.Domain.Exception;

public sealed class DomainUnauthorizedException : DomainException
{
    public DomainUnauthorizedException()
    {
    }

    public DomainUnauthorizedException(string code) : base(code)
    {
        Code = code;
    }
    
    public DomainUnauthorizedException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public DomainUnauthorizedException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}