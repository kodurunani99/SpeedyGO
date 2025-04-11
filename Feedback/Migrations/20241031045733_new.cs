using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedback.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SlotId",
                table: "Feedbacks",
                newName: "TransporterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransporterId",
                table: "Feedbacks",
                newName: "SlotId");
        }
    }
}
