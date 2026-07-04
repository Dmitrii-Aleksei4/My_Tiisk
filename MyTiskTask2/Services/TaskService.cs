using Microsoft.EntityFrameworkCore;
using MyTiskTask2.Model;
using MyTiskTask2.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TaskStatus = MyTiskTask2.Model.UserTaskStatus;

namespace MyTiskTask2.Services
{
    internal class TaskService
    {
        /// <summary>
        /// Получить русское название статуса
        /// </summary>
        public static string GetStatusDisplayName(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.All => "Все",
                TaskStatus.New => "Новая",
                TaskStatus.InProgress => "В работе",
                TaskStatus.Paused => "На паузе",
                TaskStatus.Completed => "Завершена",
                _ => "Неизвестно"
            };
        }

        /// <summary>
        /// Получить все статусы для выпадающего списка (фильтр)
        /// </summary>
        public static List<KeyValuePair<TaskStatus, string>> GetAllStatusesForFilter()
        {
            return new List<KeyValuePair<TaskStatus, string>>
            {
                new KeyValuePair<TaskStatus, string>(TaskStatus.All, "Все"),
                new KeyValuePair<TaskStatus, string>(TaskStatus.New, "Новая"),
                new KeyValuePair<TaskStatus, string>(TaskStatus.InProgress, "В работе"),
                new KeyValuePair<TaskStatus, string>(TaskStatus.Paused, "На паузе"),
                new KeyValuePair<TaskStatus, string>(TaskStatus.Completed, "Завершена")
            };


        }


        /// <summary>
        /// Преобразовать строку в статус (для фильтра)
        /// </summary>
        public static UserTaskStatus? ParseStatus(string statusName)
        {
            return statusName switch
            {
                "Все" => null,  // null означает "Все"
                "Новая" => UserTaskStatus.New,
                "В работе" => UserTaskStatus.InProgress,
                "На паузе" => UserTaskStatus.Paused,
                "Завершена" => UserTaskStatus.Completed,
                _ => throw new ArgumentException($"Неизвестный статус: {statusName}")
            };
        }

    }
}
