using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pacco.Services.Availability.Application.DTO;
using Pacco.Services.Availability.Application.Services;
using Pacco.Services.Availability.Infrastructure.Mongo.Documents;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    internal sealed class ResourcesQueryService : IResourcesQueryService
    {
        private readonly IMongoDatabase _database;
        
        public ResourcesQueryService(IMongoDatabase database)
            => _database = database;
        
        public async Task<IEnumerable<ResourceDto>> GetByTagsAsync(IEnumerable<string> tags, bool matchAllTags)
        {
            var collection = _database.GetCollection<ResourceDocument>("resources");

            if (tags is null || !tags.Any())
            {
                var allDocuments = await collection.Find(_ => true).ToListAsync();
                return allDocuments.Select(d => d.AsDto());
            }

            var documents = collection.AsQueryable();

            documents = matchAllTags
                ? documents.Where(d => tags.All(t => d.Tags.Contains(t)))
                : documents.Where(d => tags.Any(t => d.Tags.Contains(t)));

            var resources = await documents.ToListAsync();
            return resources.Select(d => d.AsDto());
        }
    }
}