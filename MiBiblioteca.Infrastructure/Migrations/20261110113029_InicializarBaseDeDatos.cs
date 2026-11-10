using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiBiblioteca.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicializarBaseDeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "libro",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Isbn_10 = table.Column<string>(type: "text", nullable: true),
                    Isbn_13 = table.Column<string>(type: "text", nullable: true),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Subtitulo = table.Column<string>(type: "text", nullable: true),
                    Editorial = table.Column<string>(type: "text", nullable: true),
                    AnioPublicacion = table.Column<string>(type: "text", nullable: true),
                    PortadaUrl = table.Column<string>(type: "text", nullable: true),
                    Paginas = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libro", x => x.LibroId);
                });

            migrationBuilder.CreateTable(
                name: "tipo_usuario",
                columns: table => new
                {
                    TipoUsuarioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipo_usuario", x => x.TipoUsuarioId);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Clave = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    TipoUsuarioId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_usuario_tipo_usuario_TipoUsuarioId",
                        column: x => x.TipoUsuarioId,
                        principalTable: "tipo_usuario",
                        principalColumn: "TipoUsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contacto",
                columns: table => new
                {
                    ContactoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioVinculadoId = table.Column<int>(type: "integer", nullable: true),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacto", x => x.ContactoId);
                    table.ForeignKey(
                        name: "FK_contacto_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_contacto_usuario_UsuarioVinculadoId",
                        column: x => x.UsuarioVinculadoId,
                        principalTable: "usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ejemplar",
                columns: table => new
                {
                    EjemplarId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LibroId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Notas = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ejemplar", x => x.EjemplarId);
                    table.ForeignKey(
                        name: "FK_ejemplar_libro_LibroId",
                        column: x => x.LibroId,
                        principalTable: "libro",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ejemplar_usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prestamo",
                columns: table => new
                {
                    PrestamoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EjemplarId = table.Column<int>(type: "integer", nullable: false),
                    ContactoId = table.Column<int>(type: "integer", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notas = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prestamo", x => x.PrestamoId);
                    table.ForeignKey(
                        name: "FK_prestamo_contacto_ContactoId",
                        column: x => x.ContactoId,
                        principalTable: "contacto",
                        principalColumn: "ContactoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_prestamo_ejemplar_EjemplarId",
                        column: x => x.EjemplarId,
                        principalTable: "ejemplar",
                        principalColumn: "EjemplarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tipo_usuario",
                columns: new[] { "TipoUsuarioId", "Descripcion", "Tipo" },
                values: new object[,]
                {
                    { 1, "Administrador de la aplicación", "admin" },
                    { 2, "Usuario estándar", "usuario" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_contacto_UsuarioId",
                table: "contacto",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_contacto_UsuarioVinculadoId",
                table: "contacto",
                column: "UsuarioVinculadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ejemplar_LibroId",
                table: "ejemplar",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_ejemplar_UsuarioId",
                table: "ejemplar",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_libro_Isbn_13",
                table: "libro",
                column: "Isbn_13",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_prestamo_ContactoId",
                table: "prestamo",
                column: "ContactoId");

            migrationBuilder.CreateIndex(
                name: "IX_prestamo_EjemplarId",
                table: "prestamo",
                column: "EjemplarId");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_Email",
                table: "usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_TipoUsuarioId",
                table: "usuario",
                column: "TipoUsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prestamo");

            migrationBuilder.DropTable(
                name: "contacto");

            migrationBuilder.DropTable(
                name: "ejemplar");

            migrationBuilder.DropTable(
                name: "libro");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "tipo_usuario");
        }
    }
}
