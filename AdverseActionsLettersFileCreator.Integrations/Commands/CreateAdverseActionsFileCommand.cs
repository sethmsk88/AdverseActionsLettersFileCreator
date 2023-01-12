using MediatR;

namespace AdverseActionsLettersFileCreator.Integrations.Commands
{
    public class CreateAdverseActionsFileCommand : IRequest
    {
        public List<Dictionary<string, object>> AdverseActionRecords { get; set; }

        public CreateAdverseActionsFileCommand(List<Dictionary<string, object>> adverseActionRecords)
        {
            AdverseActionRecords = adverseActionRecords;
        }
    }
}
