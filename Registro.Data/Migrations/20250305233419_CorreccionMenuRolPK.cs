using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registro.DAL.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionMenuRolPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    idMenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    icono = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    url = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.idMenu);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "MenuRoles",
                columns: table => new
                {
                    idMenu = table.Column<int>(type: "int", nullable: false),
                    idRol = table.Column<int>(type: "int", nullable: false),
                    IdMenuRol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRoles", x => new { x.idMenu, x.idRol });
                    table.ForeignKey(
                        name: "FK_MenuRoles_Menu_idMenu",
                        column: x => x.idMenu,
                        principalTable: "Menu",
                        principalColumn: "idMenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuRoles_Roles_idRol",
                        column: x => x.idRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    idUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreCompleto = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    idRol = table.Column<int>(type: "int", nullable: true),
                    clave = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true),
                    esActivo = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    fechaRegistro = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.idUsuario);
                    table.ForeignKey(
                        name: "FK_Usuario_Roles_idRol",
                        column: x => x.idRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuRoles_idRol",
                table: "MenuRoles",
                column: "idRol");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_idRol",
                table: "Usuario",
                column: "idRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuRoles");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
