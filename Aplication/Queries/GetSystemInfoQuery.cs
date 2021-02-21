using Aplication.Queries.ViewModels;
using MediatR;

namespace Aplication.Queries
{
    public class GetSystemInfoQuery: IRequest<SystemInfoViewModel>
    {
    }
}