namespace AdverseActionsLettersFileCreator.FileOperation.Models
{
    /// <summary>
    /// Contains the information necessary to import a file.
    /// Will evolve as configuration comes into play.
    /// </summary>
    public class FileOperationRequest
    {
        /// <summary>
        /// Unique name of the configuration which will be used 
        /// for file import.
        /// </summary>
        public string[] ConfigurationName { get; set; }
        /// <summary>
        /// If the configuration location format expects to fill in information,
        /// this is where it is provided
        /// </summary>
        public string[] FormatArgs { get; set; }
    }
}
