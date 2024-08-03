using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class tempREservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Reservations_TempReservedId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TempReservedId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Reservations");

            migrationBuilder.AddColumn<Guid>(
                name: "TempReservationId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TempReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TempReservations_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TempReservationId",
                table: "Tickets",
                column: "TempReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_TempReservations_DiscountId",
                table: "TempReservations",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TempReservations_TempReservationId",
                table: "Tickets",
                column: "TempReservationId",
                principalTable: "TempReservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TempReservations_TempReservationId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TempReservations");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TempReservationId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TempReservationId",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "Reservations",
                type: "datetime2",
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
    }
}
