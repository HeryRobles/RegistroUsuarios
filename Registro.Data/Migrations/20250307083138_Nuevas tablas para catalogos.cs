using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registro.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Nuevastablasparacatalogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    IdPelicula = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Sinopsis = table.Column<string>(type: "text", nullable: false),
                    Reseña = table.Column<string>(type: "text", nullable: false),
                    ImagenUrl = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    TrailerUrl = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    Calificacion = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    EsActiva = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaEstreno = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.IdPelicula);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    IdComentario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPelicula = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Texto = table.Column<string>(type: "text", nullable: false),
                    Calificacion = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    FechaComentario = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.IdComentario);
                    table.ForeignKey(
                        name: "FK_Comentarios_Peliculas_IdPelicula",
                        column: x => x.IdPelicula,
                        principalTable: "Peliculas",
                        principalColumn: "IdPelicula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentarios_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "idUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_IdPelicula",
                table: "Comentarios",
                column: "IdPelicula");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_IdUsuario",
                table: "Comentarios",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "Peliculas");
        }
    }
}
