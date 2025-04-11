using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportQuotation_Service.Migrations
{
    /// <inheritdoc />
    public partial class servicenameadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransporterId",
                table: "Quotations",
                newName: "TransporterEmail");

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "Quotations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "Quotations");

            migrationBuilder.RenameColumn(
                name: "TransporterEmail",
                table: "Quotations",
                newName: "TransporterId");
        }
    }
}
