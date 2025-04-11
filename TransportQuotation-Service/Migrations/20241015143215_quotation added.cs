using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportQuotation_Service.Migrations
{
    /// <inheritdoc />
    public partial class quotationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    QuotationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransporterId = table.Column<int>(type: "int", nullable: false),
                    TransporterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TransporterPhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransporterLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VehicleCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleCapacityInTons = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleHeightInFeet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleWidthInFeet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PricePerKm = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.QuotationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quotations");
        }
    }
}
