using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FriendOrganizer.DataAccess.Migrations
{
    public partial class AddedProgrammingLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FavoriteLanguageId",
                table: "Friends",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProgrammingLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammingLanguages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FavoriteLanguageId",
                table: "Friends",
                column: "FavoriteLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_ProgrammingLanguages_FavoriteLanguageId",
                table: "Friends",
                column: "FavoriteLanguageId",
                principalTable: "ProgrammingLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_ProgrammingLanguages_FavoriteLanguageId",
                table: "Friends");

            migrationBuilder.DropTable(
                name: "ProgrammingLanguages");

            migrationBuilder.DropIndex(
                name: "IX_Friends_FavoriteLanguageId",
                table: "Friends");

            migrationBuilder.DropColumn(
                name: "FavoriteLanguageId",
                table: "Friends");
        }
    }
}
