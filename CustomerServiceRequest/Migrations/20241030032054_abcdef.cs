using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerServiceRequest.Migrations
{
    /// <inheritdoc />
    public partial class abcdef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Distance",
                table: "ServiceRequests",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "TransporterPhone",
                table: "ServiceRequests",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransporterPhone",
                table: "ServiceRequests");

            migrationBuilder.AlterColumn<long>(
                name: "Distance",
                table: "ServiceRequests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
