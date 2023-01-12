namespace AdverseActionsLettersFileCreator.FileOperation.Models
{
    public class MappingConfiguration
    {
        /// <summary>
        /// The name of the configuration
        /// </summary>
        public string ConfigurationName { get; set; }
        /// <summary>
        /// The format of the file being ingested
        /// </summary>
        public string FileFormat { get; set; }
        /// <summary>
        /// The location of the file. If there is no formatting like dates involved, 
        /// this can simply be the name of the file
        /// </summary>
        public string FileLocationFormat { get; set; }
        /// <summary>
        /// A collection of the fields which need to be mapped
        /// </summary>
        public List<MappingField> MappingFields { get; set; }
        /// <summary>
        /// If true, the first row of the file is skipped.
        /// </summary>
        public bool SkipFirstRow { get; set; }
        /// <summary>
        /// If true, the last row of the file is skipped.
        /// TODO: Not currently supported for xml files
        /// </summary>
        public bool SkipLastRow { get; set; }
        /// <summary>
        /// For xml files, this xpath defines the nodes that represent an individual record.
        /// Mapping fields represent child nodes.
        /// </summary>
        public string RecordXPath { get; set; }
        /// <summary>
        /// The character that is used as a delimiter in a delimited file.
        /// For example, in a csv file this would be ','
        /// </summary>
        public char Delimiter { get; set; } = ',';
        /// <summary>
        /// The character that is used to depict a quoted string in 
        /// a delimited file. Usually "
        /// </summary>
        public char QuoteChar { get; set; } = '\"';
        /// <summary>
        /// The Api route that will be called for each record.
        /// </summary>
        public string ApiRoute { get; set; }
        /// <summary>
        /// The http method that will be called
        /// Valid values: POST, PUT
        /// </summary>
        public string HttpMethod { get; set; } = "POST";
        /// <summary>
        /// Default batch size for sending requests to the api.
        /// </summary>
        public int BatchSize { get; set; } = 1;
    }
}
