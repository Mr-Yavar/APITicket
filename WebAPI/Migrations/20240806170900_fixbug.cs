using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixbug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Reservations_ReservationId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TempReservations_TempReservationId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReservationId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TempReservationId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TempReservationId",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReservedGroupId",
                table: "Transactions",
                column: "ReservedGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TempReservedId",
                table: "Transactions",
                column: "TempReservedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Reservations_ReservedGroupId",
                table: "Transactions",
                column: "ReservedGroupId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TempReservations_TempReservedId",
                table: "Transactions",
                column: "TempReservedId",
                principalTable: "TempReservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Reservations_ReservedGroupId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TempReservations_TempReservedId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReservedGroupId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TempReservedId",
                table: "Transactions");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservationId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReservationId",
                table: "Transactions",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TempReservationId",
                table: "Transactions",
                column: "TempReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Reservations_ReservationId",
                table: "Transactions",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TempReservations_TempReservationId",
                table: "Transactions",
                column: "TempReservationId",
                principalTable: "TempReservations",
                principalColumn: "Id");
        }
    }
}
