
using MyTiskTask4.Model;
using MyTiskTask4.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TiskTask4.ConsoleUI;

public class MainWindowConsole
{
  private readonly string _login;
  private static readonly DbService _tasksManager = new DbService();
  private readonly List<string> _menu;
  private int _userId;

  public MainWindowConsole(User user)
  {
    _login = user.Name;
    _userId = user.Id;
    //_tasksManager = tasksManager ?? throw new ArgumentNullException(nameof(tasksManager));

    _menu = new List<string>
        {
            "1. Управление задачей (Запуск / Переключение)",
            "2. Приостановить активную задачу",
            "3. Завершить задачу окончательно",
            "4. Добавить задачу",
            "5. Удалить задачу",
            "6. Выход"
        };
  }

  public void Menu()
  {
    while (true)
    {
      Console.Clear();
      Console.WriteLine($"LOGIN: {_login}");

      var activeTask = _tasksManager.GetActiveTask(_userId);
      if (activeTask != null)
      {
        Console.WriteLine($"АКТИВНАЯ ЗАДАЧА: {activeTask.Title} (В работе: {_tasksManager.GetCurrentTimeSpent(activeTask).ToString(@"hh\:mm\:ss")})");
      }
      else
      {
        Console.WriteLine("АКТИВНАЯ ЗАДАЧА: Нет");
      }

      Console.WriteLine("\nMENU:");
      foreach (var item in _menu)
      {
        Console.WriteLine(item);
      }
      Console.Write("Выберите пункт меню: ");
      var choice = Console.ReadLine();

      switch (choice)
      {
        case "1":
          StartOrSwitchTask();
          break;
        case "2":
          PauseTask();
          break;
        case "3":
          CompleteTask();
          break;
        case "4":
          AddTask();
          break;
        case "5":
          DeleteTask();
          break;
        case "6":
          return;
        default:
          Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
          Console.ReadKey();
          break;
      }
    }
  }
    /// <summary>
    /// Получение всех задач
    /// </summary>
  private void StartOrSwitchTask()
  {
    Console.Clear();
    var userTasks = _tasksManager.GetUserTasks((int)_userId);

    var availableTasks = userTasks.Where(t => t.Status != UserTaskStatus.Completed).ToList();

    if (availableTasks.Count == 0)
    {
      Console.WriteLine("Нет доступных для запуска задач. Сначала добавьте задачу.");
      Console.ReadKey();
      return;
    }
    ///запуск задачи
    PrintTasksList(availableTasks);
    Console.WriteLine();
    Console.Write("Выберите номер задачи для запуска: ");

    if (!int.TryParse(Console.ReadLine(), out int taskNumber) || taskNumber < 1 || taskNumber > availableTasks.Count)
    {
      Console.WriteLine("Неверный номер задачи!");
      Console.ReadKey();
      return;
    }

    var selectedTask = availableTasks[taskNumber - 1];

    try
    {
    
     var activeOrPausedTasks = userTasks.Where(t => t.Status == UserTaskStatus.InProgress).ToList();
     if (activeOrPausedTasks.Count > 0)
     {
        _tasksManager.PauseTask(activeOrPausedTasks[0].Id);
     }

     _tasksManager.StartTask(selectedTask.Id);

      Console.Clear();
      Console.WriteLine($"✅ Задача '{selectedTask.Title}' успешно запущена!");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"❌ Ошибка при переключении задачи: {ex.Message}");
    }

    Console.WriteLine("Нажмите любую клавишу для возврата в меню...");
    Console.ReadKey();
  }
    /// <summary>
    /// Ставим на паузу
    /// </summary>
  private void PauseTask()
  {
    Console.Clear();
                

    if (!_tasksManager.HasActiveTask((long)_userId))
    {
      Console.WriteLine("У вас нет активной запущенной задачи!");
    }
    else
    {
      Console.WriteLine(" Активная задача успешно приостановлена (поставлена на паузу).");
    }
    var userTasks = _tasksManager.GetUserTasks(_userId);
    var activeOrPausedTasks = userTasks.Where(t => t.Status == UserTaskStatus.InProgress).ToList();
    if (activeOrPausedTasks.Count > 0)
    {
        _tasksManager.PauseTask(activeOrPausedTasks[0].Id);
    }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey();
  }
    /// <summary>
    /// 3. Завершение активной задачи
    /// </summary>
  private void CompleteTask()
  {
    Console.Clear();
    var userTasks = _tasksManager.GetUserTasks(_userId);
    var activeOrPausedTasks = userTasks.Where(t => t.Status != UserTaskStatus.Completed).ToList();

    if (activeOrPausedTasks.Count == 0)
    {
      Console.WriteLine("Нет задач, которые можно завершить.");
      Console.ReadKey();
      return;
    }

    PrintTasksList(activeOrPausedTasks);
    Console.WriteLine();
    Console.Write("Выберите номер задачи для ОКОНЧАТЕЛЬНОГО завершения: ");

    if (!int.TryParse(Console.ReadLine(), out int taskNumber) || taskNumber < 1 || taskNumber > activeOrPausedTasks.Count)
    {
      Console.WriteLine("Неверный номер задачи!");
      Console.ReadKey();
      return;
    }

    var selectedTask = activeOrPausedTasks[taskNumber - 1];

    _tasksManager.CompleteTask(_userId);

    Console.WriteLine($"\n✅ Задача '{selectedTask.Title}' полностью завершена!");
    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey();
  }
    /// <summary>
    /// добавление задачи
    /// </summary>
  private void AddTask()
  {
    Console.Clear();
    Console.WriteLine("Добавление новой задачи в базу данных");
    Console.WriteLine(new string('-', 30));

    Console.Write("Введите название задачи: ");
    var title = Console.ReadLine();

    Console.Write("Введите описание задачи: ");
    var description = Console.ReadLine();
    if (description == null)
    {
      description = "";
    }

    if (string.IsNullOrWhiteSpace(title))
    {
      Console.WriteLine("Название задачи не может быть пустым!");
      Console.ReadKey();
      return;
    }

    try
    {
      var newTask = _tasksManager.CreateTask(_userId, title, description);
      Console.WriteLine($"\n✅ Задача '{newTask.Title}' успешно добавлена в БД с Id {newTask.Id}!");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"\n❌ Ошибка при добавлении задачи: {ex.Message}");
    }

    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey();
  }
    /// <summary>
    /// Удаление задачи
    /// </summary>
  private void DeleteTask()
  {
    Console.Clear();
    var userTasks = _tasksManager.GetUserTasks(_userId);

    if (userTasks.Count == 0)
    {
      Console.WriteLine("Нет задач для удаления.");
      Console.ReadKey();
      return;
    }

    PrintTasksList(userTasks);
    Console.WriteLine();
    Console.Write("Выберите номер задачи для удаления: ");

    if (!int.TryParse(Console.ReadLine(), out int taskNumber) || taskNumber < 1 || taskNumber > userTasks.Count)
    {
      Console.WriteLine("Неверный номер задачи!");
      Console.ReadKey();
      return;
    }

    var taskToDelete = userTasks[taskNumber - 1];

    if (taskToDelete.Status == UserTaskStatus.InProgress)
    {
      Console.WriteLine("Нельзя удалить задачу, которая сейчас находится в работе! Сначала приостановите её.");
      Console.ReadKey();
      return;
    }

    Console.Write($"Вы уверены, что хотите полностью удалить задачу '{taskToDelete.Title}' из БД? (y/n): ");
    var confirm = Console.ReadLine();

    if (confirm?.ToLower() == "y")
    {
      _tasksManager.DeleteTask(taskToDelete.Id);
      Console.WriteLine("✅ Задача успешно удалена из базы данных!");
    }
    else
    {
      Console.WriteLine("Удаление отменено.");
    }

    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
    Console.ReadKey();
  }

  // Вспомогательный изолированный метод вывода списков
  private void PrintTasksList(List<UserTask> tasks)
  {
    Console.WriteLine("Список задач:");
    Console.WriteLine(new string('-', 60));

    for (int i = 0; i < tasks.Count; i++)
    {
      var task = tasks[i];
      Console.Write($"{i + 1}. ");

      TimeSpan currentTime = _tasksManager.GetCurrentTimeSpent(task);
      Console.WriteLine($"[{task.Status}] {task.Title} ({task.Description}) — Время: {currentTime.ToString(@"hh\:mm\:ss")}");
    }

    Console.WriteLine(new string('-', 60));
  }

  private string FormatTimeSpan(TimeSpan time)
  {
    if (time.TotalHours >= 1)
      return $"{(int)time.TotalHours}ч {time.Minutes}м {time.Seconds}с";
    if (time.TotalMinutes >= 1)
      return $"{time.Minutes}м {time.Seconds}с";

    return $"{time.Seconds}с";
  }
}
