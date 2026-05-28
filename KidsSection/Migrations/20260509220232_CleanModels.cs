using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KidsSection.Migrations
{
    /// <inheritdoc />
    public partial class CleanModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KidsCards_KidsProductTypes_KidsProductTypeId",
                table: "KidsCards");

            migrationBuilder.AlterColumn<int>(
                name: "KidsProductTypeId",
                table: "KidsCards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KidsCards_KidsProductTypes_KidsProductTypeId",
                table: "KidsCards",
                column: "KidsProductTypeId",
                principalTable: "KidsProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KidsCards_KidsProductTypes_KidsProductTypeId",
                table: "KidsCards");

            migrationBuilder.AlterColumn<int>(
                name: "KidsProductTypeId",
                table: "KidsCards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_KidsCards_KidsProductTypes_KidsProductTypeId",
                table: "KidsCards",
                column: "KidsProductTypeId",
                principalTable: "KidsProductTypes",
                principalColumn: "Id");
        }
    }
}
