using Microsoft.EntityFrameworkCore;
using MyTiskTask4;
using MyTiskTask4.DataBase;
using MyTiskTask4.Services;
using System;
using System.Windows.Forms;

namespace TiskTask4.ConsoleUI;

public class Program
{

    // Сервис для работы с БД
    //private readonly DbService _userService;
    private static readonly DbService _userService = new DbService();
    static void Main()
    {
        using var context = new AppDbContext();

        
        string? login;
        string? password;
        MainWindowConsole mainWindow;

        Console.WriteLine("Добро пожаловать в менеджер задач!");
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1 - Вход");
        Console.WriteLine("2 - Регистрация");
        Console.WriteLine("3 - Выход");

        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("\n--- Вход ---");

                Console.Write("Введите логин: ");
                login = Console.ReadLine();

                Console.Write("Введите пароль: ");
                password = Console.ReadLine();

                // ЗАЩИТА ОТ NULL: Проверяем, что пользователь не ввел пустоту
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Ошибка: Логин и пароль не могут быть пустыми!");
                    Console.ReadKey();
                    return;
                }

                // Ищем пользователя
                var user = _userService.FindUserByLogin(login);

                if (user == null)
                {
                    Console.WriteLine("Ошибка: Не удалось загрузить данные пользователя.");
                    Console.ReadKey();
                    return;
                }

                // Пытаемся авторизовать пользователя
                if (_userService.VerifyPassword(user, password))
                {

                    mainWindow = new MainWindowConsole(user);
                    mainWindow.Menu();
                }
                else
                {
                    Console.WriteLine("Неверный логин или пароль!");
                    Console.ReadKey();
                }
                break;

            case "2":
                Console.WriteLine("\n--- Регистрация ---");

                Console.Write("Введите логин: ");
                login = Console.ReadLine();

                Console.Write("Введите пароль: ");
                password = Console.ReadLine();

                // ЗАЩИТА ОТ NULL: Проверяем пустой ввод при регистрации
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Ошибка: Логин и пароль не могут быть пустыми!");
                    Console.ReadKey();
                    return;
                }

                try
                {
                    _userService.CreateUser(login, password);
                    Console.WriteLine("=====================================");
                    Console.WriteLine("🎉 Пользователь успешно зарегистрирован!");
                    Console.WriteLine("=====================================");

                    // Ищем пользователя
                    user = _userService.FindUserByLogin(login);

                    if (user == null)
                    {
                        Console.WriteLine("Ошибка: Не удалось загрузить данные пользователя.");
                        Console.ReadKey();
                        return;
                    }


                    var users = _userService.VerifyPassword(user, password);


                    // Пытаемся авторизовать пользователя
                    if (_userService.VerifyPassword(user, password))
                    {

                        mainWindow = new MainWindowConsole(user);
                        mainWindow.Menu();
                    }
                    else
                    {
                        Console.WriteLine("Неверный логин или пароль!");
                        Console.ReadKey();
                    }
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при регистрации: {ex.Message}");
                    Console.ReadKey();
                }
                break;

            case "3":
                return;

            default:
                Console.WriteLine("Неверный выбор. Программа завершает работу.");
                Console.ReadKey();
                break;
        }
    }
}
