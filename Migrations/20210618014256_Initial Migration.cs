using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidApiNigeria.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CovidNigeriaData",
                columns: table => new
                {
                    StateName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfConfirmedCases = table.Column<int>(type: "int", nullable: false),
                    NumberOfActiveCases = table.Column<int>(type: "int", nullable: false),
                    NumberOfDeaths = table.Column<int>(type: "int", nullable: false),
                    NumberOfDischarged = table.Column<int>(type: "int", nullable: false),
                    SamplesTested = table.Column<int>(type: "int", nullable: false),
                    TotalConfirmedCases = table.Column<int>(type: "int", nullable: false),
                    TotalActiveCases = table.Column<int>(type: "int", nullable: false),
                    TotalDischarged = table.Column<int>(type: "int", nullable: false),
                    TotalDeaths = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovidNigeriaData", x => new { x.StateName, x.Date });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovidNigeriaData");
        }
    }
}
