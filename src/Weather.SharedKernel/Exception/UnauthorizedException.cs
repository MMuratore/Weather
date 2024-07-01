namespace Weather.SharedKernel.Exception;

public sealed class UnauthorizedException : DomainException
{
    public UnauthorizedException()
    {
    }

    public UnauthorizedException(string code) : base(code)
    {
        Code = code;
    }
    
    public UnauthorizedException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public UnauthorizedException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}