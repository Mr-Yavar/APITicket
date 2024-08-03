using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class transactionsreserver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Reservations_ReservationId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReservationId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservedGroupId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservationId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservedId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Reservations_ReservationId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TempReservations_TempReservationId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TempReservationId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReservedGroupId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TempReservationId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TempReservedId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReservationId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Reservations_ReservationId",
                table: "Transactions",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
