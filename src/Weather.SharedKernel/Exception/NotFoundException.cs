namespace Weather.SharedKernel.Exception;

public sealed class NotFoundException : DomainException
{
    public NotFoundException()
    {
    }

    public NotFoundException(string code) : base(code)
    {
        Code = code;
    }
    
    public NotFoundException(string property, string code) : base(property, code)
    {
        Property = property;
        Code = code;
    }

    public NotFoundException(string property, string code, System.Exception inner) : base(property, code, inner)
    {
        Property = property;
        Code = code;
    }
}