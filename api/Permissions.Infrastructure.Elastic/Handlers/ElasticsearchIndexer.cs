using Microsoft.Extensions.Logging;
using Nest;
using Permissions.Domain.Dto;
using Permissions.Domain.Indexers;

namespace Permissions.Infrastructure.Elastic.Handlers;

public class ElasticsearchIndexer : IIndexer<PermissionIndex>
{
    private readonly IElasticClient _elastic;
    private readonly ILogger<ElasticsearchIndexer> _logger;
    
    public ElasticsearchIndexer(IElasticClient elastic, ILogger<ElasticsearchIndexer> logger)
    {
        _elastic = elastic;
        _logger = logger;
    }
    
    public async Task SyncToIndexAsync(PermissionIndex entity)
    {
        _logger.LogInformation("Checking if entity already exists in elastic search");
        var searchResponse = await _elastic.GetAsync<PermissionIndex>(entity.Id);

        if (searchResponse.IsValid && searchResponse.Found)
        {
            _logger.LogInformation("Entity already exists, updating...");
            var valueFromIndex = searchResponse.Source;
            valueFromIndex.EmployeeForename = entity.EmployeeForename;
            valueFromIndex.EmployeeSurname = entity.EmployeeSurname;
            valueFromIndex.PermissionType = entity.PermissionType;
            valueFromIndex.GrantedOn = entity.GrantedOn;
            
            var updateResponse = await _elastic.UpdateAsync<PermissionIndex>(valueFromIndex, u => u.Doc(valueFromIndex));

            if (updateResponse.IsValid && updateResponse.Result == Result.Updated)
            {
                _logger.LogInformation("Entity with id {Id} updated successfully", entity.Id);   
            }
            else
            {
                _logger.LogInformation("Couldn't update entity with {Id}", entity.Id);
            }
            
            return;
        }
        
        var indexingResponse = await _elastic.IndexDocumentAsync(entity);

        if (indexingResponse.IsValid && indexingResponse.Result == Result.Created)
        {
            _logger.LogInformation("Entity with id {Id} create successfully", entity.Id);   
        }
        else
        {
            _logger.LogInformation("Couldn't create entity with {Id}", entity.Id);
        }
    }
}