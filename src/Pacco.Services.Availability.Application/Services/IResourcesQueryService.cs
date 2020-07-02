using System.Collections.Generic;
using System.Threading.Tasks;
using Pacco.Services.Availability.Application.DTO;

namespace Pacco.Services.Availability.Application.Services
{
    public interface IResourcesQueryService
    {
        Task<IEnumerable<ResourceDto>> GetByTagsAsync(IEnumerable<string> tags, bool matchAllTags);
    }
}