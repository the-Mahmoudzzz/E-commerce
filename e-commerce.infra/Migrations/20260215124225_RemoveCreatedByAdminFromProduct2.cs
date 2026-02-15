using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.infra.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreatedByAdminFromProduct2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_categories_categories_CategoryId",
            //    table: "categories");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_orderDetails_Products_ProductId",
            //    table: "orderDetails");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_AspNetUsers_CreatedByAdminId",
            //    table: "Products");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_AspNetUsers_SellerId",
            //    table: "Products");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_AspNetUsers_UserId",
            //    table: "Products");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Product_categories_CategoryId",
            //    table: "Products");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProductReviews_Product_ProductId",
            //    table: "ProductReviews");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_shoppingCartItems_Product_ProductId",
            //    table: "shoppingCartItems");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Wishlists_Product_ProductId",
            //    table: "Wishlists");

            //migrationBuilder.DropIndex(
            //    name: "IX_categories_CategoryId",
            //    table: "categories");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Product",
            //    table: "Products");

            //migrationBuilder.DropIndex(
            //    name: "IX_Product_CreatedByAdminId",
            //    table: "Products");

            //migrationBuilder.DropColumn(
            //    name: "CategoryId",
            //    table: "categories");

            //migrationBuilder.DropColumn(
            //    name: "CreatedByAdminId",
            //    table: "Products");

            //migrationBuilder.RenameTable(
            //    name: "Product",
            //    newName: "products");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Product_UserId",
            //    table: "products",
            //    newName: "IX_products_UserId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Product_SellerId",
            //    table: "products",
            //    newName: "IX_products_SellerId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_Product_CategoryId",
            //    table: "products",
            //    newName: "IX_products_CategoryId");

            //migrationBuilder.AddColumn<int>(
            //    name: "ApprovedByAdminId",
            //    table: "products",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsApproved",
            //    table: "products",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_products",
            //    table: "products",
            //    column: "Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_categories_ParentCategoryId",
            //    table: "categories",
            //    column: "ParentCategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_products_ApprovedByAdminId",
            //    table: "products",
            //    column: "ApprovedByAdminId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_categories_categories_ParentCategoryId",
            //    table: "categories",
            //    column: "ParentCategoryId",
            //    principalTable: "categories",
            //    principalColumn: "Id");

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_orderDetails_products_ProductId",
            //        table: "orderDetails",
            //        column: "ProductId",
            //        principalTable: "products",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_ProductReviews_products_ProductId",
            //        table: "ProductReviews",
            //        column: "ProductId",
            //        principalTable: "products",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_products_AspNetUsers_ApprovedByAdminId",
            //        table: "products",
            //        column: "ApprovedByAdminId",
            //        principalTable: "AspNetUsers",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Restrict);

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_products_AspNetUsers_SellerId",
            //        table: "products",
            //        column: "SellerId",
            //        principalTable: "AspNetUsers",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Restrict);

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_products_AspNetUsers_UserId",
            //        table: "products",
            //        column: "UserId",
            //        principalTable: "AspNetUsers",
            //        principalColumn: "Id");

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_products_categories_CategoryId",
            //        table: "products",
            //        column: "CategoryId",
            //        principalTable: "categories",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_shoppingCartItems_products_ProductId",
            //        table: "shoppingCartItems",
            //        column: "ProductId",
            //        principalTable: "products",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);

            //    migrationBuilder.AddForeignKey(
            //        name: "FK_Wishlists_products_ProductId",
            //        table: "Wishlists",
            //        column: "ProductId",
            //        principalTable: "products",
            //        principalColumn: "Id",
            //        onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_ParentCategoryId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_orderDetails_products_ProductId",
                table: "orderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_products_AspNetUsers_ApprovedByAdminId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_AspNetUsers_SellerId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_AspNetUsers_UserId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_CategoryId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCartItems_products_ProductId",
                table: "shoppingCartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_products_ProductId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_categories_ParentCategoryId",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_ApprovedByAdminId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ApprovedByAdminId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "products");

            //migrationBuilder.RenameTable(
            //    name: "products",
            //    newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_products_UserId",
                table: "Products",
                newName: "IX_Product_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_products_SellerId",
                table: "Products",
                newName: "IX_Product_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_products_CategoryId",
                table: "Products",
                newName: "IX_Product_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByAdminId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_CategoryId",
                table: "categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CreatedByAdminId",
                table: "Products",
                column: "CreatedByAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_CategoryId",
                table: "categories",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_orderDetails_Product_ProductId",
                table: "orderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_CreatedByAdminId",
                table: "Products",
                column: "CreatedByAdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_SellerId",
                table: "Products",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_AspNetUsers_UserId",
                table: "Products",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Product_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCartItems_Product_ProductId",
                table: "shoppingCartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Product_ProductId",
                table: "Wishlists",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
