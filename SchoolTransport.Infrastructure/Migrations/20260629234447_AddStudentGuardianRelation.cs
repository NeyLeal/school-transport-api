using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolTransport.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentGuardianRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGuardians_Guardians_GuardianId",
                table: "StudentGuardians");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGuardians_Students_StudentId",
                table: "StudentGuardians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentGuardians",
                table: "StudentGuardians");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "StudentGuardians",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "StudentGuardians",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentGuardians",
                table: "StudentGuardians",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGuardians_StudentId_GuardianId",
                table: "StudentGuardians",
                columns: new[] { "StudentId", "GuardianId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGuardians_Guardians_GuardianId",
                table: "StudentGuardians",
                column: "GuardianId",
                principalTable: "Guardians",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGuardians_Students_StudentId",
                table: "StudentGuardians",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGuardians_Guardians_GuardianId",
                table: "StudentGuardians");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentGuardians_Students_StudentId",
                table: "StudentGuardians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentGuardians",
                table: "StudentGuardians");

            migrationBuilder.DropIndex(
                name: "IX_StudentGuardians_StudentId_GuardianId",
                table: "StudentGuardians");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StudentGuardians");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "StudentGuardians");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentGuardians",
                table: "StudentGuardians",
                columns: new[] { "StudentId", "GuardianId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGuardians_Guardians_GuardianId",
                table: "StudentGuardians",
                column: "GuardianId",
                principalTable: "Guardians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGuardians_Students_StudentId",
                table: "StudentGuardians",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
