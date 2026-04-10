namespace Backend.Features.Menu;

public static class MenuEndpoints
{
	public static void MapMenuEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/api/menu");

		group.MapGet("/", async (MenuService service) =>
		{
			var categories = await service.GetAllCategoriesAsync();
			return Results.Ok(categories);
		})
		.WithName("GetMenu")
		.WithOpenApi();

		group.MapGet("/categories/{id}", async (string id, MenuService service) =>
		{
			var category = await service.GetCategoryByIdAsync(id);
			return category is null ? Results.NotFound() : Results.Ok(category);
		})
		.WithName("GetMenuCategoryById")
		.WithOpenApi();

		group.MapGet("/items/{id}", async (string id, MenuService service) =>
		{
			var item = await service.GetItemByIdAsync(id);
			return item is null ? Results.NotFound() : Results.Ok(item);
		})
		.WithName("GetMenuItemById")
		.WithOpenApi();
	}
}
