using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingInfrastructure.Migrations
{
    public partial class ConvertUserIdToString_SetNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- Для таблиці Payments ---
            migrationBuilder.AddColumn<string>(
                name: "NewUserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            // Копіюємо дані, конвертуючи int у string
            migrationBuilder.Sql("UPDATE Payments SET NewUserId = CAST(UserId AS nvarchar(450))");

            // Оновлюємо рядки, де значення NewUserId не відповідає жодному Id у AspNetUsers
            migrationBuilder.Sql(@"
                UPDATE Payments 
                SET NewUserId = NULL 
                WHERE NewUserId NOT IN (SELECT Id FROM AspNetUsers)
            ");

            // Видаляємо старий зовнішній ключ, якщо він є
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User",  // замініть на актуальну назву FK, якщо потрібно
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "Payments",
                newName: "UserId");

            // Додаємо новий FK із SetNull
            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // --- Для таблиці ShoppingCart ---
            migrationBuilder.AddColumn<string>(
                name: "NewUserId",
                table: "ShoppingCart",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.Sql("UPDATE ShoppingCart SET NewUserId = CAST(UserId AS nvarchar(450))");

            migrationBuilder.Sql(@"
                UPDATE ShoppingCart 
                SET NewUserId = NULL 
                WHERE NewUserId NOT IN (SELECT Id FROM AspNetUsers)
            ");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_User",  // замініть, якщо назва інша
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCart");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "ShoppingCart",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_UserId",
                table: "ShoppingCart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // --- Для таблиці ShoppingCartProducts ---
            migrationBuilder.AddColumn<string>(
                name: "NewUserId",
                table: "ShoppingCartProducts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.Sql("UPDATE ShoppingCartProducts SET NewUserId = CAST(UserId AS nvarchar(450))");

            migrationBuilder.Sql(@"
                UPDATE ShoppingCartProducts 
                SET NewUserId = NULL 
                WHERE NewUserId NOT IN (SELECT Id FROM AspNetUsers)
            ");

            migrationBuilder.DropForeignKey(
            name: "FK_SCP_Users",
            table: "ShoppingCartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartProducts_User",
                table: "ShoppingCartProducts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCartProducts");

            migrationBuilder.RenameColumn(
                name: "NewUserId",
                table: "ShoppingCartProducts",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartProducts_AspNetUsers_UserId",
                table: "ShoppingCartProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Зворотня операція: перетворення знову в int.
            // Зверніть увагу, що конвертація з string у int може бути складнішою, особливо якщо деякі значення NULL.
            // Нижче наведено базовий приклад, який ви можете налаштувати за потребою.

            // --- Для таблиці Payments ---
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UserId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "OldUserId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE Payments SET OldUserId = TRY_CAST(UserId AS int)");
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "Payments",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User",
                table: "Payments",
                column: "UserId",
                principalTable: "AspNetUsers", // або інша таблиця, якщо потрібно
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // --- Для таблиці ShoppingCart ---
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_UserId",
                table: "ShoppingCart");

            migrationBuilder.AddColumn<int>(
                name: "OldUserId",
                table: "ShoppingCart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE ShoppingCart SET OldUserId = TRY_CAST(UserId AS int)");
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCart");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "ShoppingCart",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_User",
                table: "ShoppingCart",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // --- Для таблиці ShoppingCartProducts ---
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartProducts_AspNetUsers_UserId",
                table: "ShoppingCartProducts");

            migrationBuilder.AddColumn<int>(
                name: "OldUserId",
                table: "ShoppingCartProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE ShoppingCartProducts SET OldUserId = TRY_CAST(UserId AS int)");
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCartProducts");

            migrationBuilder.RenameColumn(
                name: "OldUserId",
                table: "ShoppingCartProducts",
                newName: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SCP_Users",
                table: "ShoppingCartProducts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
