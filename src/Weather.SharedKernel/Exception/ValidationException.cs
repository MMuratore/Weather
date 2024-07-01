namespace Weather.SharedKernel.Exception;

public sealed class ValidationException : DomainException
{
    public ValidationException()
    {
    }

    public ValidationException(string code) : base(code)
    {
        Code = code;
    }
    
    public ValidationException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public ValidationException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}