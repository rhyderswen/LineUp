using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LineUp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class okShouldWorkFr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestPartyAId",
                table: "ShiftAssignments"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestPartyBId",
                table: "ShiftAssignments"
            );

            migrationBuilder.RenameColumn(
                name: "SwapRequestPartyBId",
                table: "ShiftAssignments",
                newName: "SwapRequestId1"
            );

            migrationBuilder.RenameColumn(
                name: "SwapRequestPartyAId",
                table: "ShiftAssignments",
                newName: "SwapRequestId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ShiftAssignments_SwapRequestPartyBId",
                table: "ShiftAssignments",
                newName: "IX_ShiftAssignments_SwapRequestId1"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ShiftAssignments_SwapRequestPartyAId",
                table: "ShiftAssignments",
                newName: "IX_ShiftAssignments_SwapRequestId"
            );

            migrationBuilder.AddColumn<bool>(
                name: "partyAConfirm",
                table: "SwapRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddColumn<bool>(
                name: "partyBConfirm",
                table: "SwapRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestId",
                table: "ShiftAssignments",
                column: "SwapRequestId",
                principalTable: "SwapRequests",
                principalColumn: "Id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestId1",
                table: "ShiftAssignments",
                column: "SwapRequestId1",
                principalTable: "SwapRequests",
                principalColumn: "Id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestId",
                table: "ShiftAssignments"
            );

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestId1",
                table: "ShiftAssignments"
            );

            migrationBuilder.DropColumn(name: "partyAConfirm", table: "SwapRequests");

            migrationBuilder.DropColumn(name: "partyBConfirm", table: "SwapRequests");

            migrationBuilder.RenameColumn(
                name: "SwapRequestId1",
                table: "ShiftAssignments",
                newName: "SwapRequestPartyBId"
            );

            migrationBuilder.RenameColumn(
                name: "SwapRequestId",
                table: "ShiftAssignments",
                newName: "SwapRequestPartyAId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ShiftAssignments_SwapRequestId1",
                table: "ShiftAssignments",
                newName: "IX_ShiftAssignments_SwapRequestPartyBId"
            );

            migrationBuilder.RenameIndex(
                name: "IX_ShiftAssignments_SwapRequestId",
                table: "ShiftAssignments",
                newName: "IX_ShiftAssignments_SwapRequestPartyAId"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestPartyAId",
                table: "ShiftAssignments",
                column: "SwapRequestPartyAId",
                principalTable: "SwapRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftAssignments_SwapRequests_SwapRequestPartyBId",
                table: "ShiftAssignments",
                column: "SwapRequestPartyBId",
                principalTable: "SwapRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull
            );
        }
    }
}
