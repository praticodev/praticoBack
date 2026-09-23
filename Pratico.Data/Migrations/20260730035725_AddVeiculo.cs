using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pratico.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVeiculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Veiculo",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImovelId = table.Column<Guid>(type: "uuid", nullable: false),
                    VeiculoFabricanteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Placa = table.Column<string>(type: "varchar(20)", nullable: false),
                    Cor = table.Column<string>(type: "varchar(100)", nullable: false),
                    Modelo = table.Column<string>(type: "varchar(100)", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    FotoPath = table.Column<string>(type: "varchar(255)", nullable: true),
                    ImagemUrl = table.Column<string>(type: "varchar(255)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "Timestamp(0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veiculo_Imovel_ImovelId",
                        column: x => x.ImovelId,
                        principalSchema: "public",
                        principalTable: "Imovel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Veiculo_VeiculoFabricante_VeiculoFabricanteId",
                        column: x => x.VeiculoFabricanteId,
                        principalSchema: "public",
                        principalTable: "VeiculoFabricante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_ImovelId",
                schema: "public",
                table: "Veiculo",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculo_VeiculoFabricanteId",
                schema: "public",
                table: "Veiculo",
                column: "VeiculoFabricanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Veiculo",
                schema: "public");
        }
    }
}
