namespace IdentityApp.Shared.Exceptions
{
    /// <summary>
    /// Exception thrown when the application is unable to process an object.
    /// </summary>
    public sealed class ApplicationProcessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationProcessException"/> class.
        /// </summary>
        /// <param name="message">The error message that describes the reason for the exception.</param>
        public ApplicationProcessException(string message = "Cannot process object.")
            : base(message) { }
    }

    /// <summary>
    /// Exception thrown when the application cannot be initialized.
    /// </summary>
    public sealed class ApplicationStartupException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationStartupException"/> class.
        /// </summary>
        public ApplicationStartupException()
            : base("Cannot initialize an application.") { }
    }

    /// <summary>
    /// Exception thrown when the application cannot be initialized due to an invalid configuration.
    /// </summary>
    public sealed class ApplicationConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfigurationException"/> class.
        /// </summary>
        public ApplicationConfigurationException()
            : base("Cannot initialize an application due to invalid configuration.") { }
    }

}
