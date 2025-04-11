using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerServiceRequest.Migrations
{
    /// <inheritdoc />
    public partial class abcde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "EstimatedCost",
                table: "ServiceRequests",
                type: "float",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "EstimatedCost",
                table: "ServiceRequests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
