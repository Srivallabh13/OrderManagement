using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustId",
                table: "Orders",
                column: "CustId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustId",
                table: "Orders",
                column: "CustId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
