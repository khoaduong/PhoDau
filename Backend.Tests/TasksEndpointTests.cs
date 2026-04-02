using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Backend.Features.Tasks.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Backend.Tests;

public class TasksEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TasksEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTasks_ShouldReturnEmptyListInitially()
    {
        var response = await _client.GetAsync("/api/tasks");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var items = await response.Content.ReadFromJsonAsync<List<TaskResponse>>();
        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Fact]
    public async Task CreateTask_GetById_Update_Delete_Flow()
    {
        var create = new TaskRequest { Title = "Test Task", Description = "Integration test" };
        var createResponse = await _client.PostAsJsonAsync("/api/tasks", create);

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(created);
        Assert.Equal(create.Title, created!.Title);
        Assert.Equal(create.Description, created.Description);

        var getResponse = await _client.GetAsync($"/api/tasks/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched!.Id);

        var update = new TaskRequest { Title = "Updated", Description = "Updated desc", IsCompleted = true };
        var updateResponse = await _client.PutAsJsonAsync($"/api/tasks/{created.Id}", update);
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(updated);
        Assert.Equal(update.Title, updated!.Title);
        Assert.True(updated.IsCompleted);

        var deleteResponse = await _client.DeleteAsync($"/api/tasks/{created.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getAfterDelete = await _client.GetAsync($"/api/tasks/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
    }
}
