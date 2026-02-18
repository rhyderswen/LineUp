using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LineUp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new { ID = table.Column<Guid>(type: "uuid", nullable: false) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.ID);
                }
            );

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    authToken = table.Column<string>(type: "text", nullable: false),
                    dateCoverage = table.Column<DateOnly[]>(type: "date[]", nullable: false),
                    weekdayCoverage = table.Column<bool[]>(type: "boolean[]", nullable: false),
                    hourCoverage = table.Column<TimeOnly[]>(
                        type: "time without time zone[]",
                        nullable: false
                    ),
                    shiftAssignment = table.Column<string[,]>(type: "text[]", nullable: false),
                    preferencesID = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Schedules_Preferences_preferencesID",
                        column: x => x.preferencesID,
                        principalTable: "Preferences",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    availability = table.Column<TimeOnly[,]>(
                        type: "time without time zone[]",
                        nullable: false
                    ),
                    ScheduleID = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: false),
                    PreferencesID = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Availabilities_Preferences_PreferencesID",
                        column: x => x.PreferencesID,
                        principalTable: "Preferences",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_Availabilities_Schedules_ScheduleID",
                        column: x => x.ScheduleID,
                        principalTable: "Schedules",
                        principalColumn: "ID"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_PreferencesID",
                table: "Availabilities",
                column: "PreferencesID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_ScheduleID",
                table: "Availabilities",
                column: "ScheduleID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_preferencesID",
                table: "Schedules",
                column: "preferencesID"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Availabilities");

            migrationBuilder.DropTable(name: "Schedules");

            migrationBuilder.DropTable(name: "Preferences");
        }
    }
}
