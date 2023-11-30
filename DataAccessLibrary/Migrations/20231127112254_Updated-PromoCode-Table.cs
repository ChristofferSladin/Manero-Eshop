using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedPromoCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PromoCode_PromoCodeId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode");

            migrationBuilder.RenameTable(
                name: "PromoCode",
                newName: "PromoCodes");

            migrationBuilder.AlterColumn<decimal>(
                name: "PromoCodeAmount",
                table: "PromoCodes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "PromoCodeIsUsed",
                table: "PromoCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PromoCodeText",
                table: "PromoCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PromoCodeValidity",
                table: "PromoCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCodes",
                table: "PromoCodes",
                column: "PromoCodeId");

            migrationBuilder.CreateTable(
                name: "UserPromoCode",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PromoCodeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPromoCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPromoCode_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPromoCode_PromoCodes_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "PromoCodes",
                        principalColumn: "PromoCodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPromoCode_PromoCodeId",
                table: "UserPromoCode",
                column: "PromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPromoCode_UserId",
                table: "UserPromoCode",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PromoCodes_PromoCodeId",
                table: "Orders",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "PromoCodeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PromoCodes_PromoCodeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "UserPromoCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PromoCodes",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PromoCodeIsUsed",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PromoCodeText",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PromoCodeValidity",
                table: "PromoCodes");

            migrationBuilder.RenameTable(
                name: "PromoCodes",
                newName: "PromoCode");

            migrationBuilder.AlterColumn<decimal>(
                name: "PromoCodeAmount",
                table: "PromoCode",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PromoCode",
                table: "PromoCode",
                column: "PromoCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PromoCode_PromoCodeId",
                table: "Orders",
                column: "PromoCodeId",
                principalTable: "PromoCode",
                principalColumn: "PromoCodeId");
        }
    }
}
