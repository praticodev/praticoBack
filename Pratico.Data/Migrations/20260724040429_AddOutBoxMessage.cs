using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pratico.Data.Migrations
{
    public partial class AddOutBoxMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutBox",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(200)", nullable: false),
                    Conteudo = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "varchar(30)", nullable: false, defaultValue: "Pendente"),
                    Tentativas = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    DataUltimaTentativa = table.Column<DateTime>(type: "Timestamp(0)", nullable: true),
                    DataProcessamento = table.Column<DateTime>(type: "Timestamp(0)", nullable: true),
                    ProximaTentativaEm = table.Column<DateTime>(type: "Timestamp(0)", nullable: true),
                    Erro = table.Column<string>(type: "varchar(4000)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "Timestamp(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutBox", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OutBox_Status_ProximaTentativaEm",
                schema: "public",
                table: "OutBox",
                columns: new[] { "Status", "ProximaTentativaEm" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutBox",
                schema: "public");
        }
    }
}
