using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Services;

namespace Pacco.Services.Availability.Application.Queries.Handlers
{
    public class GetResourcesAppHandler : IQueryHandler<GetResources, IEnumerable<ResourceDto>>
    {
        private readonly IResourcesQueryService _queryService;

        public GetResourcesAppHandler(IResourcesQueryService queryService)
            => _queryService = queryService;

        public Task<IEnumerable<ResourceDto>> HandleAsync(GetResources query)
            => _queryService.GetByTagsAsync(query.Tags, query.MatchAllTags);
    }
}