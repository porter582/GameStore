using Microsoft.EntityFrameworkCore.Migrations;

namespace Game.DataAccess.Migrations
{
    public partial class GameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    GenreId = table.Column<int>(nullable: false),
                    RatingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameModel_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameModel_Rating_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Rating",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameModel_GenreId",
                table: "GameModel",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GameModel_RatingId",
                table: "GameModel",
                column: "RatingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameModel");
        }
    }
}
