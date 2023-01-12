using AdverseActionsLettersFileCreator.Integrations.Models;
using MediatR;

namespace AdverseActionsLettersFileCreator.Integrations.Queries
{
    public class GetDndbLettersQuery : IRequest<List<AdverseActionResponse>>
    {
    }
}
