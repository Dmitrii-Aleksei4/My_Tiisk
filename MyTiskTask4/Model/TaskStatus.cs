using System;
using System.Collections.Generic;
using System.Text;

namespace MyTiskTask4.Model
{
    /// <summary>
    /// Статусы задачи не возможно изменить
    /// </summary>
    public enum UserTaskStatus
    {
        /// <summary>
        /// Все (используется только для фильтра)
        /// </summary>
        All = -1,

        /// <summary>
        /// Новая задача
        /// </summary>
        New = 0,

        /// <summary>
        /// В работе
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// На паузе
        /// </summary>
        Paused = 2,

        /// <summary>
        /// Завершена
        /// </summary>
        Completed = 3
    }
}
