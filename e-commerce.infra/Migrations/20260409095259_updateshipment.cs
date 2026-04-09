using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.infra.Migrations
{
    /// <inheritdoc />
    public partial class updateshipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShippingZoneId",
                table: "shipments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_shipments_ShippingZoneId",
                table: "shipments",
                column: "ShippingZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_shippingZones_ShippingZoneId",
                table: "shipments",
                column: "ShippingZoneId",
                principalTable: "shippingZones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shipments_shippingZones_ShippingZoneId",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "IX_shipments_ShippingZoneId",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "ShippingZoneId",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "shipments");
        }
    }
}
