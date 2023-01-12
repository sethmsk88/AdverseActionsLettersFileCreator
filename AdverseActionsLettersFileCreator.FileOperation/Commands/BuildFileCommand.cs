using AdverseActionsLettersFileCreator.FileOperation.Models;
using MediatR;

namespace AdverseActionsLettersFileCreator.FileOperation.Commands
{
    public class BuildFileCommand : IRequest<AdverseActionFile>
    {
        public string BuildExecutionContext { get; set; }
        public List<Dictionary<string, object>> AdverseActionRecords { get; set; }

        public BuildFileCommand(string exportExecutionContext, List<Dictionary<string, object>> adverseActionRecords)
        {
            BuildExecutionContext = exportExecutionContext;
            AdverseActionRecords = adverseActionRecords;
        }
    }
}
