namespace Backend.Features.Menu;

public sealed class MenuCategory
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<MenuItem> Items { get; set; } = new();
}
