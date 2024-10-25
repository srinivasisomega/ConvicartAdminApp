using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConvicartAdminApp.Migrations
{
    /// <inheritdoc />
    public partial class dsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgUrl",
                table: "OrderItems");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProductImage",
                table: "OrderItems",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImage",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "imgUrl",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
