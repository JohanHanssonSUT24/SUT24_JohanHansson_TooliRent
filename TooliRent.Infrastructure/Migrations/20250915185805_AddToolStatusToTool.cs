using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TooliRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddToolStatusToTool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToolStatus",
                table: "Tools",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tools",
                newName: "ToolStatus");
        }
    }
}
