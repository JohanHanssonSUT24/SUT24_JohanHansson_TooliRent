using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TooliRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddToolStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvalible",
                table: "Tools");

            migrationBuilder.AddColumn<int>(
                name: "ToolStatus",
                table: "Tools",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToolStatus",
                table: "Tools");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvalible",
                table: "Tools",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
