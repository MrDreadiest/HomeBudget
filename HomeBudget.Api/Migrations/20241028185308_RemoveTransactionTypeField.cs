using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBudget.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTransactionTypeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "TransactionCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "TransactionCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
