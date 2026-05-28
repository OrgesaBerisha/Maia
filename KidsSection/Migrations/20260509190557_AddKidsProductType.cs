using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class AddKidsProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KidsProductTypeId",
                table: "KidsCards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KidsProductTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KidsProductTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KidsCards_KidsProductTypeId",
                table: "KidsCards",
                column: "KidsProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_KidsCards_KidsProductTypes_KidsProductTypeId",
                table: "KidsCards",
                column: "KidsProductTypeId",
                principalTable: "KidsProductTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KidsCards_KidsProductTypes_KidsProductTypeId",
                table: "KidsCards");

            migrationBuilder.DropTable(
                name: "KidsProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_KidsCards_KidsProductTypeId",
                table: "KidsCards");

            migrationBuilder.DropColumn(
                name: "KidsProductTypeId",
                table: "KidsCards");
        }
    }
}
