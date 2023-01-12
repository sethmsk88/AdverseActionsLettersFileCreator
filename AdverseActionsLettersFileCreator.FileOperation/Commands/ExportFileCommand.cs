using MediatR;
using System.Text;

namespace AdverseActionsLettersFileCreator.FileOperation.Commands
{
    public class ExportFileCommand : IRequest
    {
        public StringBuilder FileStream { get; set; }
        public string FilePath { get; set; }

        public ExportFileCommand(StringBuilder fileStream, string filePath)
        {
            FileStream = fileStream;
            FilePath = filePath;
        }
    }
}
