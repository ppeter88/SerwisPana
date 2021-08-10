using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeService.Data.Migrations
{
    public partial class CorrectFkOnRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Bike_CustomerId",
                table: "Requests");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_BikeId",
                table: "Requests",
                column: "BikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Bike_BikeId",
                table: "Requests",
                column: "BikeId",
                principalTable: "Bike",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Bike_BikeId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_BikeId",
                table: "Requests");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Bike_CustomerId",
                table: "Requests",
                column: "CustomerId",
                principalTable: "Bike",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
