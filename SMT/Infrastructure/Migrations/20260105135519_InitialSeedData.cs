using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoardComponents",
                columns: table => new
                {
                    BoardId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ComponentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardComponents", x => new { x.BoardId, x.ComponentId });
                    table.ForeignKey(
                        name: "FK_BoardComponents_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardComponents_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderBoards",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    BoardId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBoards", x => new { x.OrderId, x.BoardId });
                    table.ForeignKey(
                        name: "FK_OrderBoards_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderBoards_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Boards",
                columns: new[] { "Id", "Description", "Length", "Name", "Width" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000002"), "Main control PCB for ASMPT placement system", 120.0, "Control Board PCB", 80.0 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Power supply PCB for SMT machine", 150.0, "Power Board PCB", 100.0 }
                });

            migrationBuilder.InsertData(
                table: "Components",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Standard 10k Ohm SMD resistor", "Resistor 10kΩ" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Ceramic capacitor 100nF", "Capacitor 100nF" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Main control IC for PCB logic", "Microcontroller IC" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Description", "Name", "OrderDate" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "SMT production order for ASMPT demo line", "ASMPT-SMT-ORDER-001", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "BoardComponents",
                columns: new[] { "BoardId", "ComponentId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("11111111-1111-1111-1111-111111111111"), 20 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("22222222-2222-2222-2222-222222222222"), 10 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("33333333-3333-3333-3333-333333333333"), 1 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new Guid("11111111-1111-1111-1111-111111111111"), 15 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new Guid("22222222-2222-2222-2222-222222222222"), 8 }
                });

            migrationBuilder.InsertData(
                table: "OrderBoards",
                columns: new[] { "BoardId", "OrderId" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000002"), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("00000000-0000-0000-0000-000000000003"), new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardComponents_ComponentId",
                table: "BoardComponents",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBoards_BoardId",
                table: "OrderBoards",
                column: "BoardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardComponents");

            migrationBuilder.DropTable(
                name: "OrderBoards");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
