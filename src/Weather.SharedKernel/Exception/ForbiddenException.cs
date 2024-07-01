namespace Weather.SharedKernel.Exception;

public sealed class ForbiddenException : DomainException
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string code) : base(code)
    {
        Code = code;
    }
    
    public ForbiddenException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public ForbiddenException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}