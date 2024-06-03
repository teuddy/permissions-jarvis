using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Permissions.Domain.Dto;
using Permissions.Domain.Indexers;
using Permissions.Domain.Models;
using Permissions.Infrastructure.Elastic.Handlers;

namespace Permissions.Infrastructure.Elastic;

public static class ElasticsearchConfigExtensions
{
    public static void AddElasticsearchConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["Elasticsearch:Url"] ?? "http://localhost:9200";
        var index = configuration["Elasticsearch:Index"] ?? "permissions";

        var settings = new ConnectionSettings(new Uri(url))
            .PrettyJson()
            .DefaultIndex(index);

        var client = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(client);
        services.AddSingleton(typeof(IIndexer<PermissionIndex>), typeof(ElasticsearchIndexer));
        
        client.CreateIndexIfNotExists(index);
    }
    
    private static void CreateIndexIfNotExists(this IElasticClient client, string indexName)
    {
        if (!client.Indices.Exists(indexName).Exists)
        {
            client.Indices.Create(indexName, index => index.Map<PermissionIndex>(x => x.AutoMap()));
        }
    }
}