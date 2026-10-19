using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTransport.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DispositivosPush : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispositivosPush",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Plataforma = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispositivosPush", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispositivosPush_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispositivosPush_Token",
                table: "DispositivosPush",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DispositivosPush_UsuarioId",
                table: "DispositivosPush",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispositivosPush");
        }
    }
}
