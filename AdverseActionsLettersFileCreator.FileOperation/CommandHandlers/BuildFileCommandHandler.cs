using AdverseActionsLettersFileCreator.FileOperation.Commands;
using AdverseActionsLettersFileCreator.FileOperation.Extensions;
using AdverseActionsLettersFileCreator.FileOperation.Models;
using MediatR;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace AdverseActionsLettersFileCreator.FileOperation.CommandHandlers
{
    public class BuildFileCommandHandler : IRequestHandler<BuildFileCommand, AdverseActionFile>
    {
        public BuildFileCommandHandler()
        {
        }

        public async Task<AdverseActionFile> Handle(BuildFileCommand request, CancellationToken cancellationToken)
        {
            // Convert the execution context into a file export request
            var fileOperationRequest = JsonConvert.DeserializeObject<FileOperationRequest>(request.BuildExecutionContext);

            var configurationName = fileOperationRequest.ConfigurationName.FirstOrDefault();

            if (string.IsNullOrEmpty(configurationName))
            {
                throw new ArgumentException("Mapping Configuration not provided in arguments.");
            }

            MappingConfiguration[] MappingConfigurations =
                JsonConvert.DeserializeObject<MappingConfiguration[]>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"FileConfigurations.json")));

            // Find the specified mapping configuration. If it does not exist, throw an exception.
            var mappingConfiguration =
                MappingConfigurations.FirstOrDefault(mc => mc.ConfigurationName == configurationName);

            if (mappingConfiguration == null)
            {
                throw new ArgumentException($"Mapping Configuration {configurationName} is missing.");
            }

            StringBuilder stringBuilder;

            // Import the file based on the format specified in the configuration and return the number of rows imported
            switch (mappingConfiguration.FileFormat)
            {
                case "Fixed":
                    stringBuilder = BuildFixedWidthFile(request.AdverseActionRecords, mappingConfiguration);
                    break;
                case "Delimited":
                    stringBuilder = BuildDelimitedFile(request.AdverseActionRecords, mappingConfiguration);
                    break;
                default:
                    throw new ArgumentException(
                        $"File Import Format {mappingConfiguration.FileFormat} not supported in configuration {mappingConfiguration.ConfigurationName}.");
            }

            // Add trailer record
            stringBuilder.AppendLine(string.Concat("Trailer ", request.AdverseActionRecords.Count));

            return new AdverseActionFile { FileStream = stringBuilder };
        }

        private static StringBuilder BuildFixedWidthFile(IEnumerable<Dictionary<string, object>> recordsList, MappingConfiguration mappingConfiguration)
        {
            var stringBuilder = new StringBuilder();
            var maxPlacement = mappingConfiguration.MappingFields.Max(x => x.FieldPlacement);
            foreach (var record in recordsList)
            {
                var lastFieldLength = 0;
                var lastFieldValueLength = 0;
                for (var i = 0; i <= maxPlacement; i++)
                {
                    var fixedWidthMapping =
                        mappingConfiguration.MappingFields.Find(x => x.FieldPlacement == i);
                    var fieldValue = record[fixedWidthMapping.FieldName];

                    if (!fixedWidthMapping.FixedLength.HasValue)
                    {
                        throw new ArgumentException(
                            $"Mapping configuration {mappingConfiguration.ConfigurationName}, mapping field {fixedWidthMapping.FieldName} has no fixed length.");
                    }
                    fieldValue = (fieldValue ?? "");
                    var fieldValueString = new StringBuilder().Append(fieldValue.ToString().Trim());
                    lastFieldLength = fixedWidthMapping.FixedLength.Value;
                    lastFieldValueLength = fieldValueString.Length;

                    switch (fixedWidthMapping.FieldType)
                    {
                        case "numeric":
                            if (fieldValueString.Length == 0)
                            {
                                if (fixedWidthMapping.NullValue != null)
                                    fieldValueString.Append(fixedWidthMapping.NullValue);
                            }
                            else
                            {
                                if (fixedWidthMapping.Scale != null)
                                {
                                    if (fieldValueString.ToString().Contains('.'))
                                    {
                                        if (fieldValueString.ToString()
                                                .Substring(fieldValueString.ToString().LastIndexOf('.') + 1).Length <=
                                            fixedWidthMapping.Scale.Value)
                                        {
                                            // Handle numerics that do not have complete scale (123.5 with scale of 2 = 123.50)
                                            fieldValueString.Append('0', fixedWidthMapping.Scale.Value -
                                                                         fieldValueString.ToString().Substring(
                                                                                 fieldValueString.ToString()
                                                                                     .LastIndexOf('.') + 1)
                                                                             .Length);
                                        }
                                        else
                                        {
                                            // Handle numerics that have too much scale
                                            var trimStart = fieldValueString.ToString().LastIndexOf('.') + 1 + fixedWidthMapping.Scale.Value;
                                            var trimLength = fieldValueString.ToString().Length - trimStart;
                                            fieldValueString.Remove(trimStart, trimLength);
                                        }
                                    }
                                    else
                                        // Handle integers that need scale added (123 with scale of 2 = 123.00)
                                        fieldValueString.Append(".")
                                            .Append('0', fixedWidthMapping.Scale.Value);
                                }
                                fieldValueString = fieldValueString.Replace(".", "");
                            }
                            break;
                        case "date":
                            if (fieldValueString.Length == 0)
                            {
                                if (fixedWidthMapping.NullValue != null)
                                    fieldValueString.Append(fixedWidthMapping.NullValue);
                            }
                            else
                            {
                                DateTime testedDate;
                                if (DateTime.TryParse(fieldValueString.ToString(), out testedDate))
                                    fieldValueString.Replace(fieldValueString.ToString(), testedDate.ToString(fixedWidthMapping.DateFormat));
                            }
                            break;
                        default:
                            if (fieldValueString.Length == 0)
                            {
                                if (fixedWidthMapping.NullValue != null)
                                    fieldValueString.Append(fixedWidthMapping.NullValue);
                            }
                            break;
                    }

                    // Truncate if larger than fixed length
                    if (fieldValueString.Length > fixedWidthMapping.FixedLength.Value)
                    {
                        fieldValueString = fieldValueString.Remove(fixedWidthMapping.FixedLength.Value,
                            fieldValueString.Length - fixedWidthMapping.FixedLength.Value);
                    }

                    if (fixedWidthMapping.Alignment != null && (fixedWidthMapping.PadChar == ' ' || fixedWidthMapping.PadChar == '0'))
                    {
                        if (fixedWidthMapping.Alignment == "left")
                        {
                            fieldValueString.Append(Convert.ToChar(fixedWidthMapping.PadChar),
                                Convert.ToInt16(fixedWidthMapping.FixedLength - fieldValueString.Length));
                        }
                        else
                        {
                            // StringBuilder must have at least one character for Insert method
                            if (fieldValueString.Length == 0)
                            {
                                fieldValueString.Append(fixedWidthMapping.PadChar, fixedWidthMapping.FixedLength.Value);
                            }
                            else
                            {
                                fieldValueString.Insert(0, fixedWidthMapping.PadChar.ToString(),
                                    Convert.ToInt16(fixedWidthMapping.FixedLength - fieldValueString.Length));
                            }
                        }
                    }
                    stringBuilder.Append(fieldValueString);
                }
                // fix eol line treatment to match SSIS ragged right file eol, ie not padding last field
                stringBuilder.TrimEndByAmount(lastFieldLength - lastFieldValueLength).AppendLine();
            }
            return stringBuilder;
        }

        private static StringBuilder BuildDelimitedFile(IEnumerable<Dictionary<string, object>> recordsList, MappingConfiguration mappingConfiguration)
        {
            var delimiter = mappingConfiguration.Delimiter;
            if (delimiter == ' ')
            {
                throw new ArgumentException(
                    $"Mapping configuration {mappingConfiguration.ConfigurationName}, unknown delimiter.");
            }

            var stringBuilder = new StringBuilder();
            foreach (var record in recordsList)
            {
                var fieldCounter = 0;
                foreach (var field in record)
                {
                    stringBuilder.Append($"{field.Value}{(fieldCounter < record.Count - 1 ? "," : "")}");
                    fieldCounter++;
                }
            }
            stringBuilder.AppendLine();

            return stringBuilder;
        }
    }
}
