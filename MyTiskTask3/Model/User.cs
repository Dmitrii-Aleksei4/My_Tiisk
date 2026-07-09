using System;
using System.Collections.Generic;


namespace MyTiskTask3.Model
{
  /// <summary>
  /// Описывает пользователя системы и его задачи.
  /// </summary>
  public class User
  {
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Отображаемое имя пользователя.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания пользователя.
    /// </summary>
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Связь 1:1 с UserModel
    public virtual UserModel UserModel { get; set; }

    // Связь 1:М с UserTask )
    public virtual ICollection<UserTask> UserTasks { get; set; } = new List<UserTask>();

    public override string ToString()
    {
      return Name;
    }
  }
}
