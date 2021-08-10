using Microsoft.EntityFrameworkCore.Migrations;

namespace BikeService.Data.Migrations
{
    public partial class addForeignKeysForRequestPos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "RequestPos",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestPos_RequestId",
                table: "RequestPos",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestPos_ServiceId",
                table: "RequestPos",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPos_Requests_RequestId",
                table: "RequestPos",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestPos_Service_ServiceId",
                table: "RequestPos",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestPos_Requests_RequestId",
                table: "RequestPos");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestPos_Service_ServiceId",
                table: "RequestPos");

            migrationBuilder.DropIndex(
                name: "IX_RequestPos_RequestId",
                table: "RequestPos");

            migrationBuilder.DropIndex(
                name: "IX_RequestPos_ServiceId",
                table: "RequestPos");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "RequestPos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
