using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NorthwindEfCore.Migrations
{
  // Таблиці вже є в БД (Database First), тому початкова міграція нічого не змінює.
  // Вона лише фіксує поточну модель, від якої рахуються наступні міграції
  public partial class InitialCreate : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
  }
}