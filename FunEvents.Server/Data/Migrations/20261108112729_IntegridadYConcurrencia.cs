using Microsoft.EntityFrameworkCore.Migrations;


namespace FunEvents.Server.Data.Migrations
{
    public partial class IntegridadYConcurrencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TiposEntrada_EventoId",
                table: "TiposEntrada");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposEntrada_EventoId_Nombre",
                table: "TiposEntrada",
                columns: new[] { "EventoId", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ReservaId",
                table: "Pagos",
                column: "ReservaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_Reservas_ReservaId",
                table: "Pagos",
                column: "ReservaId",
                principalTable: "Reservas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_Reservas_ReservaId",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_TiposEntrada_EventoId_Nombre",
                table: "TiposEntrada");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_ReservaId",
                table: "Pagos");

            migrationBuilder.CreateIndex(
                name: "IX_TiposEntrada_EventoId",
                table: "TiposEntrada",
                column: "EventoId");
        }
    }
}
