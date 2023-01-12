namespace AdverseActionsLettersFileCreator.FileOperation.Models
{
    public class MappingField
    {
        /// <summary>
        /// Contains the name of the json field. Can be nested. For example, CoBuyer.Address.City
        /// If there is an array of items, it will be mapped as
        /// CoBuyer.Address[0].City, CoBuyer.Address[1].City or
        /// Loan.PaymentAmount[1], Loan.PaymentAmount[2]
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// For a fixed length file, this is the placement of the field in the file
        /// </summary>
        public int FieldPlacement { get; set; }
        /// <summary>
        /// For a fixed length file, this is the length of the field
        /// </summary>
        public int? FixedLength { get; set; }
        /// <summary>
        /// For an XML Document, this is the xpath for the field
        /// </summary>
        public string XPath { get; set; }
        /// <summary>
        /// Can be numeric, date, or string. 
        /// </summary>
        public string FieldType { get; set; } = "string";
        /// <summary>
        /// If a floating point numeric, and the field value does not contain the decimal point (like in the Argo booking file),
        /// then this is the scale (for example, Numeric(9,2) has a scale of 2)
        /// </summary>
        public int? Scale { get; set; }
        /// <summary>
        /// If the field type is date, then this format is used to parse the date.
        /// </summary>
        public string DateFormat { get; set; }
        /// <summary>
        /// If the field is not present (for example, in an excel document), this is the default value to put in
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// If true, then this field is part of the key of the object.
        /// This means that if the field is null, the object is null.
        /// </summary>
        public bool IsKey { get; set; }
        /// <summary>
        /// This is a value that could be read from a file which represents a null value.
        /// </summary>
        public string NullValue { get; set; }
        /// <summary>
        /// Padding character for fixed length file fields
        /// </summary>
        public char PadChar { get; set; } = ' ';
        /// <summary>
        /// Alignment direction for fixed length file fields
        /// </summary>
        public string Alignment { get; set; } = "Left";
    }
}
