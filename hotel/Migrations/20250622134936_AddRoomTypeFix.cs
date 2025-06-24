using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotel.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomTypeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_TypeRooms_TypeRoomId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TypeRooms",
                table: "TypeRooms");

            migrationBuilder.RenameTable(
                name: "TypeRooms",
                newName: "RoomTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomTypes",
                table: "RoomTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_TypeRoomId",
                table: "Rooms",
                column: "TypeRoomId",
                principalTable: "RoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_TypeRoomId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomTypes",
                table: "RoomTypes");

            migrationBuilder.RenameTable(
                name: "RoomTypes",
                newName: "TypeRooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TypeRooms",
                table: "TypeRooms",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_TypeRooms_TypeRoomId",
                table: "Rooms",
                column: "TypeRoomId",
                principalTable: "TypeRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
