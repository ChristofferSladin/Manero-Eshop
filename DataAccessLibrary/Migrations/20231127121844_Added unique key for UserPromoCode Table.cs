using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddeduniquekeyforUserPromoCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPromoCode_AspNetUsers_UserId",
                table: "UserPromoCode");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPromoCode_PromoCodes_PromoCodeId",
                table: "UserPromoCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPromoCode",
                table: "UserPromoCode");

            migrationBuilder.RenameTable(
                name: "UserPromoCode",
                newName: "UserPromoCodes");

            migrationBuilder.RenameIndex(
                name: "IX_UserPromoCode_UserId",
                table: "UserPromoCodes",
                newName: "IX_UserPromoCodes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPromoCode_PromoCodeId",
                table: "UserPromoCodes",
                newName: "IX_UserPromoCodes_PromoCodeId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UserPromoCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "UserPromoCodeId",
                table: "UserPromoCodes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPromoCodes",
                table: "UserPromoCodes",
                column: "UserPromoCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPromoCodes_AspNetUsers_UserId",
                table: "UserPromoCodes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPromoCodes_PromoCodes_PromoCodeId",
                table: "UserPromoCodes",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "PromoCodeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPromoCodes_AspNetUsers_UserId",
                table: "UserPromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPromoCodes_PromoCodes_PromoCodeId",
                table: "UserPromoCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPromoCodes",
                table: "UserPromoCodes");

            migrationBuilder.DropColumn(
                name: "UserPromoCodeId",
                table: "UserPromoCodes");

            migrationBuilder.RenameTable(
                name: "UserPromoCodes",
                newName: "UserPromoCode");

            migrationBuilder.RenameIndex(
                name: "IX_UserPromoCodes_UserId",
                table: "UserPromoCode",
                newName: "IX_UserPromoCode_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserPromoCodes_PromoCodeId",
                table: "UserPromoCode",
                newName: "IX_UserPromoCode_PromoCodeId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "UserPromoCode",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPromoCode",
                table: "UserPromoCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPromoCode_AspNetUsers_UserId",
                table: "UserPromoCode",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPromoCode_PromoCodes_PromoCodeId",
                table: "UserPromoCode",
                column: "PromoCodeId",
                principalTable: "PromoCodes",
                principalColumn: "PromoCodeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
