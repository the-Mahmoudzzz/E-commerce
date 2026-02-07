using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.infra.Migrations
{
    /// <inheritdoc />
    public partial class Edit_FeedBack_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_shippingZones_ShippingZoneId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ShippingZoneId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_shippingZones_ShippingZoneId",
                table: "AspNetUsers",
                column: "ShippingZoneId",
                principalTable: "shippingZones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_shippingZones_ShippingZoneId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Feedbacks");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingZoneId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_shippingZones_ShippingZoneId",
                table: "AspNetUsers",
                column: "ShippingZoneId",
                principalTable: "shippingZones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
