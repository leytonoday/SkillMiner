using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillMiner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WebScrapingTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WebScrapingTaskId",
                table: "JobListing",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WebScrapingTask",
                columns: table => new
                {
                    DatabaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StartedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebScrapingTask", x => x.DatabaseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebScrapingTask_Id",
                table: "WebScrapingTask",
                column: "Id",
                unique: true)
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebScrapingTask");

            migrationBuilder.DropColumn(
                name: "WebScrapingTaskId",
                table: "JobListing");
        }
    }
}
