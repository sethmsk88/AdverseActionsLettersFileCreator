namespace AdverseActionsLettersFileCreator.Integrations.Models
{
    public class ApplicationSettings
    {
        public int SqlTimeout { get; set; }
        public ConnectionStrings? ConnectionStrings { get; set; }
        public string? TenantId { get; set; }
        public string? ClientId { get; set; }
        public string? NotificationServiceResourceId { get; set; }
        public string? NotificationServiceUrl { get; set; }
        public string? ClientIdentityCertificateCommonName { get; set; }
        public string? ClientIdentityCertificateIssuer { get; set; }
        public string? EmailFromAddress { get; set; }
        public List<string>? EmailSuccessRecipients { get; set; }
        public List<string>? EmailFailureRecipients { get; set; }
        public string? EmailSupportSubjectPrefix { get; set; }
        public string? EmailSupportPriority { get; set; }
        public bool EncryptionEnabled { get; set; }
        public string? EftEncryptionKey { get; set; }
        public string? FilePathRoot { get; set; }
        /// <summary>
        /// Path to file to be exported
        /// </summary>
        public string? ExportFileLocation { get; set; }
        /// <summary>
        /// Export file name
        /// </summary>
        public string? ExportFileName { get; set; }
        public string? LettersInputFilePath { get; set; }
        /// <summary>
        /// Path to archive files
        /// </summary>
        public string? ArchiveFileLocation { get; set; }
        /// <summary>
        /// Path to artifact files durign processing
        /// </summary>
        public string? ArtifactFileLocation { get; set; }
        public string? SupportArtifactFilename { get; set; }
        public string? EmailArtifactFilename { get; set; }
        public int ArchiveFileRetentionPeriod { get; set; }
        public bool RunArgoAAProcess { get; set; }
        /// <summary>
        /// Dictionary of attributes specific to process
        /// </summary>
        public Attributes? Attributes { get; set; }
        // public Dictionary<string, object> Attributes { get; set; }
    }

    public class ConnectionStrings
    {
        public string? LosConnectionString { get; set; }
        public string? CarsConnectionString { get; set; }
    }

    public class Attributes
    {
        public List<string>? ExcludedStates { get; set; }
        public List<string>? ExcludedPayTypes { get; set; }
        public int AdverseActionLookbackDays { get; set; }
        public int CounterofferLookbackDays { get; set; }
        public int DndbLookbackDays { get; set; }
        public string? SqlTimeout { get; set; }
        public Dictionary<string, string>? LetterTypes { get; set; }
        public string? DndbLetterType { get; set; }
    }
}
