namespace Weather.Notification;

internal sealed class EmailOptions
{
    public const string Section = "Email";

    public string DefaultFrom { get; init; } = string.Empty;
    public SmtpOptions Smtp { get; init; } = new();
}

internal class SmtpOptions
{
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
}