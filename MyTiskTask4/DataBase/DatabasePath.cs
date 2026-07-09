using System;
using System.IO;

namespace MyTiskTask4.DataBase
{
    public static class DatabasePath
    {
        private const string DatabaseFileName = "tisktask.db";

        /// <summary>
        /// Получить путь к корневой папке решения
        /// </summary>
        public static string GetSolutionRoot()
        {
            // Начинаем с текущей директории
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            // Поднимаемся вверх, пока не найдем папку с .sln файлом
            while (directory != null && !directory.GetFiles("*.slnx").Any())
            {
                directory = directory.Parent;
            }

            // Если нашли решение, возвращаем его путь
            if (directory != null)
            {
                return directory.FullName;
            }

            // Если не нашли .sln, используем родительскую папку от bin/Debug
            var parentDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
            if (parentDir != null)
            {
                var grandParent = parentDir.Parent?.Parent?.Parent;
                if (grandParent != null)
                {
                    return grandParent.FullName;
                }
            }

            // Fallback - текущая директория
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Получить путь к файлу базы данных в корне решения
        /// </summary>
        public static string GetDatabasePath()
        {
            var rootPath = GetSolutionRoot();
            return Path.Combine(rootPath, DatabaseFileName);
        }

        /// <summary>
        /// Получить строку подключения
        /// </summary>
        public static string GetConnectionString()
        {
            return $"Data Source={GetDatabasePath()}";
        }

        /// <summary>
        /// Проверить, существует ли база данных
        /// </summary>
        public static bool DatabaseExists()
        {
            return File.Exists(GetDatabasePath());
        }

        /// <summary>
        /// Показать информацию о пути к БД (для отладки)
        /// </summary>
        public static void ShowDatabaseInfo()
        {
            var dbPath = GetDatabasePath();
            Console.WriteLine($"📁 Путь к БД: {dbPath}");
            Console.WriteLine($"📊 БД существует: {(DatabaseExists() ? "Да" : "Нет")}");
            if (DatabaseExists())
            {
                var fileInfo = new FileInfo(dbPath);
                Console.WriteLine($"📏 Размер: {fileInfo.Length / 1024} KB");
            }
        }
    }
}