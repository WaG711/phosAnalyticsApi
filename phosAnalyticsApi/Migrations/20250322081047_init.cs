using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace phosAnalyticsApi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChartData",
                columns: table => new
                {
                    ChartDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartData", x => x.ChartDataId);
                });

            migrationBuilder.CreateTable(
                name: "ChartPoint",
                columns: table => new
                {
                    ChartPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    ChartDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartPoint", x => x.ChartPointId);
                    table.ForeignKey(
                        name: "FK_ChartPoint_ChartData_ChartDataId",
                        column: x => x.ChartDataId,
                        principalTable: "ChartData",
                        principalColumn: "ChartDataId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartPoint_ChartDataId",
                table: "ChartPoint",
                column: "ChartDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChartPoint");

            migrationBuilder.DropTable(
                name: "ChartData");
        }
    }
}
