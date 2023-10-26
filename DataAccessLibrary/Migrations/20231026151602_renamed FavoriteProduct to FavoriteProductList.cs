using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class renamedFavoriteProducttoFavoriteProductList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProductProduct");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.CreateTable(
                name: "FavoriteProductList",
                columns: table => new
                {
                    FavoriteProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProductList", x => x.FavoriteProductId);
                    table.ForeignKey(
                        name: "FK_FavoriteProductList_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProductListProduct",
                columns: table => new
                {
                    FavoriteProductListFavoriteProductId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProductListProduct", x => new { x.FavoriteProductListFavoriteProductId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_FavoriteProductListProduct_FavoriteProductList_FavoriteProductListFavoriteProductId",
                        column: x => x.FavoriteProductListFavoriteProductId,
                        principalTable: "FavoriteProductList",
                        principalColumn: "FavoriteProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProductListProduct_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProductList_Id",
                table: "FavoriteProductList",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProductListProduct_ProductsProductId",
                table: "FavoriteProductListProduct",
                column: "ProductsProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteProductListProduct");

            migrationBuilder.DropTable(
                name: "FavoriteProductList");

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    FavoriteProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.FavoriteProductId);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProductProduct",
                columns: table => new
                {
                    FavoriteProductsFavoriteProductId = table.Column<int>(type: "int", nullable: false),
                    ProductsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProductProduct", x => new { x.FavoriteProductsFavoriteProductId, x.ProductsProductId });
                    table.ForeignKey(
                        name: "FK_FavoriteProductProduct_FavoriteProducts_FavoriteProductsFavoriteProductId",
                        column: x => x.FavoriteProductsFavoriteProductId,
                        principalTable: "FavoriteProducts",
                        principalColumn: "FavoriteProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteProductProduct_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProductProduct_ProductsProductId",
                table: "FavoriteProductProduct",
                column: "ProductsProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_Id",
                table: "FavoriteProducts",
                column: "Id");
        }
    }
}
