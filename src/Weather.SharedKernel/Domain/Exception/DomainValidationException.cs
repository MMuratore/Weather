namespace Weather.SharedKernel.Domain.Exception;

public sealed class DomainValidationException : DomainException
{
    public DomainValidationException()
    {
    }

    public DomainValidationException(string code) : base(code)
    {
        Code = code;
    }
    
    public DomainValidationException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public DomainValidationException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}