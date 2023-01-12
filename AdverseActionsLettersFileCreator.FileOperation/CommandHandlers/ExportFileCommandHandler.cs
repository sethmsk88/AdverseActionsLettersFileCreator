using AdverseActionsLettersFileCreator.FileOperation.Commands;
using MediatR;

namespace AdverseActionsLettersFileCreator.FileOperation.CommandHandlers
{
    public class ExportFileCommandHandler : IRequestHandler<ExportFileCommand>
    {
        public async Task<Unit> Handle(ExportFileCommand request, CancellationToken cancellationToken)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(request.FilePath, FileMode.Create);
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write(request.FileStream.ToString());
                }
            }
            finally
            {
                fs?.Dispose();
            }

            return await Unit.Task;
        }
    }
}
