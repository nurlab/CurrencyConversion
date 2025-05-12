namespace CC.Application.Configrations
{
    /// <summary>
    /// Represents the configuration settings for security-related options such as certificate paths and passwords.
    /// </summary>
    public class SecuritySettings
    {
        /// <summary>
        /// Gets or sets the path to the security certificate.
        /// </summary>
        /// <value>
        /// The full file path of the certificate used for signing or encryption operations.
        /// </value>
        public string CertificatePath { get; set; }

        /// <summary>
        /// Gets or sets the password for the security certificate.
        /// </summary>
        /// <value>
        /// The password required to access the certificate for signing or encryption operations.
        /// </value>
        public string CertificatePassword { get; set; }
    }
}
