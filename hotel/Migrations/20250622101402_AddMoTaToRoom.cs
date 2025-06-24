using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel.Migrations
{
    /// <inheritdoc />
    public partial class AddMoTaToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "RoomImages",
                newName: "Path");

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "Rooms");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "RoomImages",
                newName: "ImageUrl");
        }
    }
}
