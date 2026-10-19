using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTransport.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AcessoAjudante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                table: "Ajudantes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ajudantes_UsuarioId",
                table: "Ajudantes",
                column: "UsuarioId",
                unique: true,
                filter: "[UsuarioId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Ajudantes_Usuarios_UsuarioId",
                table: "Ajudantes",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ajudantes_Usuarios_UsuarioId",
                table: "Ajudantes");

            migrationBuilder.DropIndex(
                name: "IX_Ajudantes_UsuarioId",
                table: "Ajudantes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Ajudantes");
        }
    }
}
