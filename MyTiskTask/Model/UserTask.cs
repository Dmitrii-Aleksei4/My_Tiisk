using System;

namespace MyTiskTask.Model;

/// <summary>
/// Описывает модель задачи.
/// </summary>
public class UserTask
{
    

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название задачи.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Описание задачи.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания задачи.
    /// </summary>
    public DateTime Created { get; init; }

    /// <summary>
    /// Потраченное время на задачу.
    /// </summary>
    public TimeSpan TimeSpent { get; set; }

    /// <summary>
    /// Признак того, что задача сейчас активна.
    /// </summary>
    public bool IsRunning { get; set; }

    /// <summary>
    /// Момент последнего запуска таймера.
    /// </summary>
    public DateTime? StartedAtUtc { get; set; }

    /// <summary>
    /// Признак того, что задача завершена.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Момент завершения задачи.
    /// </summary>
    public DateTime? CompletedAtUtc { get; set; }
    

    
    public int UserId { get; set; }        // FK напрямую на User!
    public virtual User User { get; set; }  // Навигация на User

    
}