namespace IdentityApp.Shared.Exceptions
{
    public sealed class ApplicationProcessException(string message = "Cannot process object.")
    : Exception(message)
    { }

    public sealed class ApplicationStartupException()
    : Exception("Cannot initialize an application.")
    { }

    public sealed class ApplicationConfigurationException()
    : Exception("Cannot initialize an application due to invalid configuration.")
    { }
}
