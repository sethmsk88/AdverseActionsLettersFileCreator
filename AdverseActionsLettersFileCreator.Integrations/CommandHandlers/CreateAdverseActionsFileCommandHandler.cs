using AdverseActionsLettersFileCreator.FileOperation.Commands;
using AdverseActionsLettersFileCreator.FileOperation.Models;
using AdverseActionsLettersFileCreator.Integrations.Commands;
using AdverseActionsLettersFileCreator.Integrations.Models;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text;

namespace AdverseActionsLettersFileCreator.Integrations.CommandHandlers
{
    public class CreateAdverseActionsFileCommandHandler : IRequestHandler<CreateAdverseActionsFileCommand>
    {
        private readonly IMediator _mediator;
        private readonly IOptions<ApplicationSettings> _appSettings;

        public CreateAdverseActionsFileCommandHandler(IMediator mediator,
            IOptions<ApplicationSettings> appSettings)
        {
            _mediator = mediator;
            _appSettings = appSettings;
        }

        public async Task<Unit> Handle(CreateAdverseActionsFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Save file in outbound directory
                var exportExecutionContext = BuildFileExecutionContext(new List<string>()
                    {
                        "ConfigurationName:ArgoAdverseActionFile",
                        "FormatArgs:" + _appSettings.Value.ExportFileLocation,
                        "FormatArgs:" + _appSettings.Value.ExportFileName
                    });

                // Create Adverse Actions file
                AdverseActionFile adverseActionsFile = await _mediator.Send(new BuildFileCommand(exportExecutionContext, request.AdverseActionRecords));

                // Encrypt Adverse Actions file
                var unencryptedContent = Encoding.UTF8.GetBytes(adverseActionsFile.FileStream.ToString());

                if (_appSettings.Value.EncryptionEnabled)
                {
                    var publicKeyBytes = File.ReadAllBytes(_appSettings.Value.EftEncryptionKey);
                    var encryptedContent = (await _mediator.Send(new EncryptFileCommand(unencryptedContent, publicKeyBytes, _appSettings.Value.ExportFileName))).FileByte;

                    // Save encrypted Adverse Actions file
                    await SaveFile(encryptedContent);
                }
                else
                {
                    // Save unencrypted Adverse Actions file
                    await SaveFile(unencryptedContent);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return await Unit.Task;
        }

        private string BuildFileExecutionContext(IReadOnlyCollection<string> args)
        {
            // if we only have one argument, return an empty object
            if (args.Count < 2)
            {
                return "{}"; // empty json object
            }

            var executionContextDictionary = new Dictionary<string, List<string>>();

            foreach (var argParts in args.Select(executionContextArg => executionContextArg.Split(':'))) // Skip the first argument which has the facade name
            {
                if (!executionContextDictionary.ContainsKey(argParts[0]))
                {
                    executionContextDictionary.Add(argParts[0], new List<string>());
                }
                executionContextDictionary[argParts[0]].Add(string.Join(":", argParts.Skip(1))); // In case part of the value has a colon in it, join the leftover parts back ...
            }

            var sb = new StringBuilder();
            foreach (var executionContext in executionContextDictionary)
            {
                sb.AppendFormat("\"{0}\":", executionContext.Key);
                sb.Append("[");
                foreach (var val in executionContext.Value)
                {
                    if (val.StartsWith("\"") && val.EndsWith("\""))
                    {
                        sb.Append(val); // If the value already has quotes, no need to add them
                    }
                    else
                    {
                        sb.AppendFormat("\"{0}\",", val);
                    }
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("],");
            }

            // remove the last comma and surround the string with { } to create a json object
            return sb.Remove(sb.Length - 1, 1).Replace("\\", "\\\\").Insert(0, "{").Append("}").ToString();
        }

        private async Task<string> SaveFile(byte[] fileContent)
        {
            string filePath = Path.Combine(_appSettings.Value.ExportFileLocation, _appSettings.Value.ExportFileName);

            // Make filename unique
            filePath = String.Concat(filePath, DateTime.Now.ToString("yyyyMMddHHmmss"));

            // Export file
            // Write encrypted file to ExportFileLocation
            File.WriteAllBytes(filePath, fileContent);

            return filePath;
        }
    }
}
