using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Delivora.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReviews_Customers_CustomerId",
                table: "CustomerReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerReviews_Orders_OrderId",
                table: "CustomerReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerReviews",
                table: "CustomerReviews");

            migrationBuilder.RenameTable(
                name: "CustomerReviews",
                newName: "OrderReviews");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerReviews_OrderId",
                table: "OrderReviews",
                newName: "IX_OrderReviews_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerReviews_CustomerId",
                table: "OrderReviews",
                newName: "IX_OrderReviews_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderReviews",
                table: "OrderReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderReviews_Customers_CustomerId",
                table: "OrderReviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderReviews_Orders_OrderId",
                table: "OrderReviews",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderReviews_Customers_CustomerId",
                table: "OrderReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderReviews_Orders_OrderId",
                table: "OrderReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderReviews",
                table: "OrderReviews");

            migrationBuilder.RenameTable(
                name: "OrderReviews",
                newName: "CustomerReviews");

            migrationBuilder.RenameIndex(
                name: "IX_OrderReviews_OrderId",
                table: "CustomerReviews",
                newName: "IX_CustomerReviews_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderReviews_CustomerId",
                table: "CustomerReviews",
                newName: "IX_CustomerReviews_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerReviews",
                table: "CustomerReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReviews_Customers_CustomerId",
                table: "CustomerReviews",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReviews_Orders_OrderId",
                table: "CustomerReviews",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
