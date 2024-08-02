using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class temp4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TempReservationId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TempReservationId",
                table: "Tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservedId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TempReservedId",
                table: "Tickets",
                column: "TempReservedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Reservations_TempReservedId",
                table: "Tickets",
                column: "TempReservedId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Reservations_TempReservedId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TempReservedId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TempReservedId",
                table: "Tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservationId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TempReservationId",
                table: "Tickets",
                column: "TempReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Reservations_TempReservationId",
                table: "Tickets",
                column: "TempReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
