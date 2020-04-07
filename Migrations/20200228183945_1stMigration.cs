using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiveLog.Migrations
{
    public partial class _1stMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Divers",
                columns: table => new
                {
                    DiverID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: false),
                    FName = table.Column<string>(nullable: false),
                    LName = table.Column<string>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    DiveCertID = table.Column<string>(maxLength: 10, nullable: true),
                    CertificateAuthority = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divers", x => x.DiverID);
                });

            migrationBuilder.CreateTable(
                name: "DiveSites",
                columns: table => new
                {
                    DiveSiteID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SiteName = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    StProv = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Altitude = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiveSites", x => x.DiveSiteID);
                });

            migrationBuilder.CreateTable(
                name: "DiverLogs",
                columns: table => new
                {
                    DiverLogID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DiverID = table.Column<int>(nullable: false),
                    DiveSiteID = table.Column<int>(nullable: false),
                    DiveNumber = table.Column<int>(nullable: false),
                    DiveDate = table.Column<DateTime>(nullable: false),
                    BottomTime = table.Column<int>(nullable: false),
                    MaxDepth = table.Column<int>(nullable: false),
                    TankStartPSI = table.Column<int>(nullable: false),
                    TankEndPSI = table.Column<int>(nullable: false),
                    DiveStartTime = table.Column<DateTime>(nullable: false),
                    DiveStopTime = table.Column<DateTime>(nullable: false),
                    SafetyStopDepth = table.Column<int>(nullable: false),
                    SafetyStopTime = table.Column<int>(nullable: false),
                    PressureGroupEntry = table.Column<string>(nullable: false),
                    PressureGroupExit = table.Column<string>(nullable: false),
                    SurfaceInterval = table.Column<int>(nullable: false),
                    WaterTemp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiverLogs", x => x.DiverLogID);
                    table.ForeignKey(
                        name: "FK_DiverLogs_DiveSites_DiveSiteID",
                        column: x => x.DiveSiteID,
                        principalTable: "DiveSites",
                        principalColumn: "DiveSiteID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiverLogs_Divers_DiverID",
                        column: x => x.DiverID,
                        principalTable: "Divers",
                        principalColumn: "DiverID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiverLogs_DiveSiteID",
                table: "DiverLogs",
                column: "DiveSiteID");

            migrationBuilder.CreateIndex(
                name: "IX_DiverLogs_DiverID",
                table: "DiverLogs",
                column: "DiverID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiverLogs");

            migrationBuilder.DropTable(
                name: "DiveSites");

            migrationBuilder.DropTable(
                name: "Divers");
        }
    }
}
