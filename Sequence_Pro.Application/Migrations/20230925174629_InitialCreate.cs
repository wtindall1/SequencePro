using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sequence_Pro.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SequenceAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UniprotId = table.Column<string>(type: "text", nullable: false),
                    ProteinSequence = table.Column<string>(type: "text", nullable: false),
                    SequenceLength = table.Column<int>(type: "integer", nullable: false),
                    MolecularWeight = table.Column<double>(type: "double precision", nullable: false),
                    AminoAcidComposition = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceAnalyses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SequenceAnalyses");
        }
    }
}
