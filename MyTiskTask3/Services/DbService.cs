using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyTiskTask3.DataBase;
using MyTiskTask3.Model;

namespace MyTiskTask3.Services
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
                    // ХЕШИРУЕМ ПАРОЛЬ 
                    Password = PasswordHasher.HashPassword(password),

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
            // ПРОВЕРЯЕМ ХЕШ
            return PasswordHasher.VerifyPassword(password, user.UserModel.Password);
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

        // ============================================================
        //  =====  ВЫВОД ЗАДАЧИ НА ЭКРАН =====
        // ============================================================
        /// <summary>Получить все задачи пользователя</summary>
        public List<UserTask> GetUserTasks(int userId)
        {
            return _context.UserTasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Created)
                .ToList();
        }

        /// <summary>Получить задачи по статусу</summary>
        public List<UserTask> GetTasksByStatus(int userId, UserTaskStatus status)
        {
            return _context.UserTasks
                .Where(t => t.UserId == userId && t.Status == status)
                .OrderByDescending(t => t.Created)
                .ToList();
        }

        /// <summary>Получить задачу по ID</summary>
        public UserTask GetTaskById(int taskId)
        {
            return _context.UserTasks
                .FirstOrDefault(t => t.Id == taskId);
        }


        // ============================================================
        //  =====  ВВОД ЗАДАЧИ =====
        // ============================================================


      

        /// <summary>Создать задачу</summary>
        public UserTask CreateTask(long userId, string title, string description)
        {
            var task = new UserTask
            {
                UserId = (int)userId,
                Title = title,
                Description = description,
                Status = UserTaskStatus.New,
                Created = DateTime.Now,
                TimeSpent = TimeSpan.Zero,
                IsRunning = false,
                IsCompleted = false,
                StartedAtUtc = null,
                CompletedAtUtc = null
            };

            _context.UserTasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        /// <summary>Обновить задачу по ID</summary>
        public void UpdateTask(int taskId, string title, string description)
        {
            // 1. Находим задачу в БД
            var task = _context.UserTasks.Find(taskId);
            if (task == null)
                throw new Exception("Задача не найдена!");

            // 2. Обновляем поля
            task.Title = title;
            task.Description = description;
            // Status, TimeSpent, IsRunning и т.д. НЕ меняем!

            // 3. Сохраняем изменения
            _context.SaveChanges();
        }

        /// <summary>Удалить задачу</summary>
        public void DeleteTask(int taskId)
        {
            var task = _context.UserTasks.Find(taskId);
            if (task != null)
            {
                _context.UserTasks.Remove(task);
                _context.SaveChanges();
            }
        }

        /// <summary>Запустить задачу</summary>
        public void StartTask(int taskId)
        {
            var task = _context.UserTasks.Find(taskId);
            if (task == null) return;

            if (task.IsCompleted)
                throw new InvalidOperationException("Нельзя запустить завершённую задачу!");

            task.IsRunning = true;
            task.Status = UserTaskStatus.InProgress;
            task.StartedAtUtc = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>Поставить на паузу</summary>
        public void PauseTask(int taskId)
        {
            var task = _context.UserTasks.Find(taskId);
            if (task == null) return;

            if (!task.IsRunning)
                throw new InvalidOperationException("Задача не запущена!");

            task.IsRunning = false;
            task.Status = UserTaskStatus.Paused;

            if (task.StartedAtUtc.HasValue)
            {
                var elapsed = DateTime.Now - task.StartedAtUtc.Value;
                task.TimeSpent += elapsed;
            }

            _context.SaveChanges();
        }

        /// <summary>Завершить задачу</summary>
        public void CompleteTask(int taskId)
        {
            var task = _context.UserTasks.Find(taskId);
            if (task == null) return;

            if (task.IsRunning)
            {
                if (task.StartedAtUtc.HasValue)
                {
                    var elapsed = DateTime.Now - task.StartedAtUtc.Value;
                    task.TimeSpent += elapsed;
                }
                task.IsRunning = false;
            }

            task.Status = UserTaskStatus.Completed;
            task.IsCompleted = true;
            task.CompletedAtUtc = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>Получить активную задачу</summary>
        public UserTask GetActiveTask(long userId)
        {
            return _context.UserTasks
                .FirstOrDefault(t => t.UserId == userId && t.IsRunning);
        }

        /// <summary>Есть ли активная задача</summary>
        public bool HasActiveTask(long userId)
        {
            return _context.UserTasks.Any(t => t.UserId == userId && t.IsRunning);
        }

    }
}
