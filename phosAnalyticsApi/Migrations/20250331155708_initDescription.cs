using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace phosAnalyticsApi.Migrations
{
    /// <inheritdoc />
    public partial class initDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ChartData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ChartData");
        }
    }
}
