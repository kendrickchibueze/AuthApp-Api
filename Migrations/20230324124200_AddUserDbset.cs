﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuthApp_Api.Migrations
{
    public partial class AddUserDbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");
        }
    }
}
