using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class lng_lat45 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorsSeat_Floors_FloorId",
                table: "FloorsSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_FloorsSeat_Seats_SeatId",
                table: "FloorsSeat");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_FloorsSeat_FloorSeatId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloorsSeat",
                table: "FloorsSeat");

            migrationBuilder.RenameTable(
                name: "FloorsSeat",
                newName: "FloorSeats");

            migrationBuilder.RenameIndex(
                name: "IX_FloorsSeat_SeatId",
                table: "FloorSeats",
                newName: "IX_FloorSeats_SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_FloorsSeat_FloorId",
                table: "FloorSeats",
                newName: "IX_FloorSeats_FloorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloorSeats",
                table: "FloorSeats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorSeats_Floors_FloorId",
                table: "FloorSeats",
                column: "FloorId",
                principalTable: "Floors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloorSeats_Seats_SeatId",
                table: "FloorSeats",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_FloorSeats_FloorSeatId",
                table: "Tickets",
                column: "FloorSeatId",
                principalTable: "FloorSeats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FloorSeats_Floors_FloorId",
                table: "FloorSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_FloorSeats_Seats_SeatId",
                table: "FloorSeats");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_FloorSeats_FloorSeatId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FloorSeats",
                table: "FloorSeats");

            migrationBuilder.RenameTable(
                name: "FloorSeats",
                newName: "FloorsSeat");

            migrationBuilder.RenameIndex(
                name: "IX_FloorSeats_SeatId",
                table: "FloorsSeat",
                newName: "IX_FloorsSeat_SeatId");

            migrationBuilder.RenameIndex(
                name: "IX_FloorSeats_FloorId",
                table: "FloorsSeat",
                newName: "IX_FloorsSeat_FloorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FloorsSeat",
                table: "FloorsSeat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FloorsSeat_Floors_FloorId",
                table: "FloorsSeat",
                column: "FloorId",
                principalTable: "Floors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FloorsSeat_Seats_SeatId",
                table: "FloorsSeat",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_FloorsSeat_FloorSeatId",
                table: "Tickets",
                column: "FloorSeatId",
                principalTable: "FloorsSeat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
