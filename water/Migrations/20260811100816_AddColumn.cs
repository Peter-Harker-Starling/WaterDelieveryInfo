using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace water.Migrations
{
    /// <inheritdoc />
    public partial class AddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Product_Id",
                table: "WaterDeliveryInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Product_Name",
                table: "WaterDeliveryInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sheet_Id",
                table: "WaterDeliveryInfos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product_Id",
                table: "WaterDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "Product_Name",
                table: "WaterDeliveryInfos");

            migrationBuilder.DropColumn(
                name: "Sheet_Id",
                table: "WaterDeliveryInfos");
        }
    }
}
