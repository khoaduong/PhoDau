using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMenuSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: "classic-cheese");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: "cold-brew");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: "double-smash");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: "sparkling-lemon");

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "Id",
                keyValue: "burgers");

            migrationBuilder.DeleteData(
                table: "MenuCategories",
                keyColumn: "Id",
                keyValue: "drinks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "burgers", "Burgers" },
                    { "drinks", "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "Description", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { "classic-cheese", "burgers", "Beef patty, cheddar, lettuce, tomato, house sauce", true, "Classic Cheese", 8.90m },
                    { "cold-brew", "drinks", "Single-origin cold brew coffee", true, "Cold Brew", 4.20m },
                    { "double-smash", "burgers", "Two smash patties, American cheese, pickles, onions", true, "Double Smash", 11.50m },
                    { "sparkling-lemon", "drinks", "House-made lemon soda", true, "Sparkling Lemon", 3.50m }
                });
        }
    }
}
