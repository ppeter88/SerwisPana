using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeService.Data.Migrations
{
    public partial class SetForeignKeyRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Requests_Reporter",
                table: "Requests",
                column: "Reporter");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Employee_Reporter",
                table: "Requests",
                column: "Reporter",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Employee_Reporter",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_Reporter",
                table: "Requests");
        }
    }
}
