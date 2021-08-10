using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeService.Data.Migrations
{
    public partial class SetForeignKeyOnRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bike",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "BikeId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Bike_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Bike",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Bike_CustomerId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "BikeId",
                table: "Requests");

            migrationBuilder.AddColumn<int>(
                name: "Bike",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
