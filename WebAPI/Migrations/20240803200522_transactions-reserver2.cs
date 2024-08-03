using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class transactionsreserver2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TempReservations_TempReservationId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TempReservationId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TempReservationId",
                table: "Tickets");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TempReservedId",
                table: "Tickets",
                column: "TempReservedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TempReservations_TempReservedId",
                table: "Tickets",
                column: "TempReservedId",
                principalTable: "TempReservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TempReservations_TempReservedId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TempReservedId",
                table: "Tickets");

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservationId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TempReservationId",
                table: "Tickets",
                column: "TempReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TempReservations_TempReservationId",
                table: "Tickets",
                column: "TempReservationId",
                principalTable: "TempReservations",
                principalColumn: "Id");
        }
    }
}
