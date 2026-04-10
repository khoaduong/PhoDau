using Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Menu;

public class MenuService
{
	private readonly AppDbContext _context;

	public MenuService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<IReadOnlyList<MenuCategory>> GetAllCategoriesAsync()
	{
		return await _context.MenuCategories
			.Include(c => c.Items)
			.ToListAsync();
	}

	public async Task<MenuCategory?> GetCategoryByIdAsync(string id)
	{
		return await _context.MenuCategories
			.Include(c => c.Items)
			.FirstOrDefaultAsync(c => c.Id == id);
	}

	public async Task<MenuItem?> GetItemByIdAsync(string id)
	{
		return await _context.MenuItems
			.FirstOrDefaultAsync(i => i.Id == id);
	}
}

