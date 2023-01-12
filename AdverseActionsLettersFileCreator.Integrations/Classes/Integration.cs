using AdverseActionsLettersFileCreator.Integrations.Commands;
using AdverseActionsLettersFileCreator.Integrations.Interfaces;
using AdverseActionsLettersFileCreator.Integrations.Models;
using AdverseActionsLettersFileCreator.Integrations.Queries;
using MediatR;
using Microsoft.Extensions.Options;

namespace AdverseActionsLettersFileCreator.Integrations.Classes
{
    public class Integration : IIntegration
    {
        private readonly IMediator _mediator;
        private readonly IOptions<ApplicationSettings> _appSettings;

        public Integration(IMediator mediator,
            IOptions<ApplicationSettings> appSettings)
        {
            _mediator = mediator;
            _appSettings = appSettings;
        }

        public async Task RunAsync()
        {
            // Make sure the configured ExportAdverseActionsFile path exists
            if (!Directory.Exists(_appSettings.Value.ExportFileLocation))
            {
                string errorMessage = $"The export folder path [{_appSettings.Value.ExportFileLocation}] is not found or does not exists. Please check the export folder path and run the job again.";

                throw new Exception(errorMessage);
            }

            List<AdverseActionResponse> letterList = await _mediator.Send(new GetDndbLettersQuery());

            // Sort letter list by LetterType
            var sortedLetterList = letterList.OrderBy(x => x.LetterType).ToList();

            // Convert to list of dictionaries for the export
            var dictionaryList = sortedLetterList.Select(letter =>
                letter.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(letter))).ToList();

            // Create and export Adverse Actions file
            await _mediator.Send(new CreateAdverseActionsFileCommand(dictionaryList));
        }


    }
}