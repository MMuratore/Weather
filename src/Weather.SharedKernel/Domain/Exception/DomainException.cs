namespace Weather.SharedKernel.Domain.Exception;

public abstract class DomainException : System.Exception
{
    public string? Property;
    public string? Code;

    public DomainException()
    {
    }
    
    public DomainException(string code) : base(code)
    {
        Code = code;
    }
    
    public DomainException(string property, string code) : base(code)
    {
        Property = property;
        Code = code;
    }

    public DomainException(string property, string code, System.Exception inner) : base(code, inner)
    {
        Property = property;
        Code = code;
    }
}