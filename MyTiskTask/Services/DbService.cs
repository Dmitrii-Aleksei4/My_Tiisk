using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyTiskTask.DataBase;
using MyTiskTask.Model;

namespace MyTiskTask.Services
{
    internal class DbService : IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed = false;

        /// <summary>
        /// Конструктор - создает подключение к БД
        /// </summary>
        public DbService()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        /// <summary>
        /// Найти пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Объект User или null</returns>
        public User FindUserByLogin(string login)
        {
            return _context.Users
                .Include(u => u.UserModel)  // Загружаем связанные данные
                .FirstOrDefault(u => u.Name == login);
        }

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>Созданный объект User</returns>
        public User CreateUser(string login, string password)
        {
            var user = new User
            {
                Name = login,
                CreatedAtUtc = DateTime.Now,
                UserModel = new UserModel
                {
                    Password = password,
                    
                }
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        /// <summary>
        /// Проверить пароль пользователя
        /// </summary>
        /// <param name="user">Объект User</param>
        /// <param name="password">Пароль для проверки</param>
        /// <returns>true - пароль верный, false - неверный</returns>
        public bool VerifyPassword(User user, string password)
        {
            if (user?.UserModel == null)
                return false;

            return user.UserModel.Password == password;
        }

        /// <summary>
        /// Обновить дату последнего входа пользователя
        /// </summary>
        /// <param name="user">Объект User</param>
        public void UpdateLastLoginDate(User user)
        {
            if (user?.UserModel == null)
                return;

            
            _context.SaveChanges();
        }

        /// <summary>
        /// Проверить, существует ли пользователь с таким логином
        /// </summary>
        /// <param name="login">Логин</param>
        /// <returns>true - существует, false - нет</returns>
        public bool UserExists(string login)
        {
            return _context.Users.Any(u => u.Name == login);
        }

        /// <summary>
        /// Получить пользователя по ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Объект User или null</returns>
        public User GetUserById(long id)
        {
            return _context.Users
                .Include(u => u.UserModel)
                .FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        public List<User> GetAllUsers()
        {
            return _context.Users
                .Include(u => u.UserModel)
                .ToList();
        }

        /// <summary>
        /// Сменить пароль пользователя
        /// </summary>
        /// <param name="user">Объект User</param>
        /// <param name="newPassword">Новый пароль</param>
        public void ChangePassword(User user, string newPassword)
        {
            if (user?.UserModel == null)
                return;

            user.UserModel.Password = newPassword;
            _context.SaveChanges();
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="user">Объект User</param>
        public void DeleteUser(User user)
        {
            if (user == null)
                return;

            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// Освободить ресурсы
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _context?.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
