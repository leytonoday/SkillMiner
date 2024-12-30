using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillMiner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MicrosoftJobListing_HadKeywordsExtracted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HadKeywordsExtracted",
                table: "MicrosoftJobListing",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HadKeywordsExtracted",
                table: "MicrosoftJobListing");
        }
    }
}
