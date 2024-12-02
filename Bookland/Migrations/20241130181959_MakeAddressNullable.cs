using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookland.Migrations
{
    /// <inheritdoc />
    public partial class MakeAddressNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "Address",
            table: "Users",
            type: "TEXT",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
