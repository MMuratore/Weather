namespace Weather.SharedKernel.Exception;

public sealed class ConflictException : DomainException
{
    public ConflictException()
    {
    }

    public ConflictException(string code) : base(code)
    {
        Code = code;
    }
    
    public ConflictException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public ConflictException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}