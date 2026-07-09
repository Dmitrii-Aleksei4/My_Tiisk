using Microsoft.EntityFrameworkCore;
using MyTiskTask4.DataBase;
using MyTiskTask4.Model;
using MyTiskTask4.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyTiskTask4
{
    public partial class Form1 : Form
    {
        // Сервис для работы с БД
        private readonly DbService _userService;
        // ID текущего пользователя
        private int? _currentUserId = null;
        // Имя текущего пользователя
        private string _currentUserName = null;
        //Сервис для работы со статусами
        private readonly TaskService _taskService;
        //ID выбранной задачи в списке
        private int? _selectedTaskId = null;  // ← ID выбранной задачи в списке

        // ДОБАВЬ ТАЙМЕР
        private System.Windows.Forms.Timer _updateTimer;


        public Form1()
        {
            // создаем форму
            InitializeComponent();
            //создаем БД если её нет
            _userService = new DbService();

            LoadUsersToComboBox();

           
            // Загружаем статусы в фильтр
            LoadStatusFilter();


            // ✅ СОЗДАЁМ И ЗАПУСКАЕМ ТАЙМЕР
            _updateTimer = new System.Windows.Forms.Timer();
            _updateTimer.Interval = 1000;  // 1 секунда
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();


            // УСТАНАВЛИВАЕМ НАЧАЛЬНЫЙ ЗАГОЛОВОК
            this.Text = "TiskTask Desktop";


        }
        // ============================================================
        //  =====  Работа с заголовком =====
        // ============================================================

        /// <summary>
        /// Обновить заголовок формы с именем активного пользователя
        /// </summary>
        private void UpdateWindowTitle()
        {
            if (_currentUserId == null)
            {
                this.Text = "TiskTask Desktop";
            }
            else
            {
                this.Text = $"TiskTask Desktop — Пользователь: {_currentUserName}";
            }
        }
        // ============================================================
        //  =====  РАБОТА С ШАПКОЙ =====
        // ============================================================

        /// <summary>
        /// Загрузить всех пользователей в ComboBox
        /// </summary>
        private void LoadUsersToComboBox()
        {
            usersComboBox.Items.Clear();

            var users = _userService.GetAllUsers();

            if (users.Count == 0)
            {
                usersComboBox.Items.Add("-- Создать нового --");
                usersComboBox.SelectedIndex = 0;
                UpdateButtonState();
                return;
            }

            usersComboBox.Items.Add("-- Создать нового --");

            foreach (var user in users)
            {
                usersComboBox.Items.Add(user.Name);
            }

            usersComboBox.SelectedIndex = 0;
            UpdateButtonState();
        }

        /// <summary>
        /// Обработчик выбора пользователя
        /// </summary>
        private void usersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonState();  // ← Обновляем кнопку при выборе
        }

        /// <summary>
        /// Обновить состояние кнопки и текста ВХОДА/РЕГИСТРАЦИИ
        /// </summary>
        private void UpdateButtonState()
        {
            if (usersComboBox.SelectedItem == null)
            {
                addUserButton.Text = "Вход";
                addUserButton.Enabled = false;
                return;
            }

            string selectedItem = usersComboBox.SelectedItem.ToString();

            if (selectedItem == "-- Создать нового --")
            {
                addUserButton.Text = "Регистрация";  // ← Меняем текст
                addUserButton.Enabled = true;
            }
            else
            {
                addUserButton.Text = "Вход";  // ← Меняем текст
                addUserButton.Enabled = true;
            }
        }

        /// <summary>
        /// Обработчик кнопки (Вход / Регистрация)
        /// </summary>
        private void addUserButton_Click(object sender, EventArgs e)
        {
            if (usersComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите действие!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = usersComboBox.SelectedItem.ToString();

            // ===== РЕЖИМ РЕГИСТРАЦИИ =====
            if (selectedItem == "-- Создать нового --")
            {
                ShowRegistrationDialog();
                return;
            }

            // ===== РЕЖИМ ВХОДА =====
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ищем пользователя
            var user = _userService.FindUserByLogin(selectedItem);

            if (user == null)
            {
                MessageBox.Show("Пользователь не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверяем пароль
            if (_userService.VerifyPassword(user, password))
            {
                _userService.UpdateLastLoginDate(user);
                MessageBox.Show($"Добро пожаловать, {user.Name}!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                txtPassword.Clear();               //<- Очищаем поле пароля 
                _currentUserName = selectedItem;   //<- Сохраняем имя пользователя
                _currentUserId = user.Id;          //<- Сохраняем ID пользователя
                UpdateWindowTitle();               //<- Обновляем заголовок
                LoadTasks();                       //<- Загружаем ТАБЛИЦУ ЗАДАЧ
            }
            else
            {
                MessageBox.Show("Неверный пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        /// <summary>
        /// Создаем нового пользователя
        /// </summary>
        private void ShowRegistrationDialog()
        {
            using (var form = new FormNewUser())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    string login = form.UserLogin;
                    string password = form.UserPassword;

                    // Проверяем, существует ли пользователь
                    if (_userService.UserExists(login))
                    {
                        MessageBox.Show($"Пользователь '{login}' уже существует!",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Создаём пользователя
                    _userService.CreateUser(login, password);

                    MessageBox.Show($"Пользователь '{login}' создан!",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Обновляем список пользователей в ComboBox
                    RefreshUsersList();

                    // Выбираем созданного пользователя
                    int index = usersComboBox.Items.IndexOf(login);
                    if (index >= 0)
                    {
                        usersComboBox.SelectedIndex = index;
                    }

                    // Очищаем поле пароля
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
        }

        /// <summary>
        /// Обновить список пользователей
        /// </summary>
        private void RefreshUsersList()
        {
            string selected = usersComboBox.SelectedItem?.ToString();
            LoadUsersToComboBox();

            if (!string.IsNullOrEmpty(selected) && usersComboBox.Items.Contains(selected))
            {
                usersComboBox.SelectedItem = selected;
            }
        }

        // ============================================================
        //  =====  ФИЛЬТР =====
        // ============================================================

        /// <summary>
        /// Загрузить статусы в выпадающий список фильтра
        /// </summary>
        private void LoadStatusFilter()
        {
            // Очищаем список
            statusFilterComboBox.Items.Clear();

            // Получаем все статусы из enum
            var statuses = TaskService.GetAllStatusesForFilter();

            // Добавляем статусы в ComboBox (только названия)
            foreach (var status in statuses)
            {
                statusFilterComboBox.Items.Add(status.Value);
            }

            // Сохраняем полный список с ключами в Tag
            statusFilterComboBox.Tag = statuses;

            // Выбираем первый элемент ("Все")
            if (statusFilterComboBox.Items.Count > 0)
            {
                statusFilterComboBox.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// Обработчик нажатия выбора фильтра
        /// </summary>
        
        private void statusFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTasks();
        }



        /// <summary>
        /// ОЧИСТКА ДЕТАЛЕЙ ЗАДАЧИ
        /// </summary>
        private void ClearTaskDetails()
        {
            titleTextBox.Clear();
            descriptionTextBox.Clear();
            
                saveTaskButton.Visible = true;
                updateTaskButton.Visible = false;
            
        }

        

        // ============================================================
        //  =====  РАБОТА ТАБИЛЕЙ ЗАДАЧ =====
        // ============================================================

        /// <summary>
        /// Загрузить задачи в tasksListView с учетом выбранного фильтра
        /// </summary>
        private void LoadTasks()
        {
            // Проверяем, выбран ли пользователь
            if (_currentUserId == null)
            {
                tasksListView.Items.Clear();
                ClearTaskDetails();
                return;
            }

            // Получаем выбранный статус из фильтра
            string selectedStatusName = statusFilterComboBox.SelectedItem?.ToString() ?? "Все";

            // Преобразуем строку в enum (через TaskService)
            var selectedStatus = TaskService.ParseStatus(selectedStatusName);

            List<UserTask> tasks;

            // Если выбран "Все" (null) — показываем все задачи
            if (selectedStatus == null)
            {
                tasks = _userService.GetUserTasks(_currentUserId.Value);
            }
            else
            {
                // Иначе показываем задачи с выбранным статусом
                tasks = _userService.GetTasksByStatus(_currentUserId.Value, selectedStatus.Value);
            }

            // Очищаем список
            tasksListView.Items.Clear();

            // Добавляем задачи в ListView
            foreach (var task in tasks)
            {
                var item = new ListViewItem(_currentUserName ?? "---");
                item.SubItems.Add(task.Title);
                item.SubItems.Add(TaskService.GetStatusDisplayName(task.Status));

                // ВЫЧИСЛЯЕМ ТЕКУЩЕЕ ВРЕМЯ
                TimeSpan currentTime = task.TimeSpent;
                if (task.IsRunning && task.StartedAtUtc.HasValue)
                {
                    currentTime += DateTime.Now - task.StartedAtUtc.Value;
                }
                item.SubItems.Add(currentTime.ToString(@"hh\:mm\:ss"));

                item.Tag = task.Id;
                tasksListView.Items.Add(item);
            }

            // Обновляем информацию об активной задаче
            UpdateActiveTask();
        }

        /// <summary>
        /// Обновить информацию об активной задаче
        /// </summary>
        private void UpdateActiveTask()
        {
            if (_currentUserId == null)
            {
                activeTaskValueLabel.Text = "Нет активной задачи";
                return;
            }

            var activeTask = _userService.GetActiveTask(_currentUserId.Value);
            if (activeTask != null)
            {
                // ВЫЧИСЛЯЕМ ТЕКУЩЕЕ ВРЕМЯ
                TimeSpan currentTime = activeTask.TimeSpent;
                if (activeTask.StartedAtUtc.HasValue)
                {
                    currentTime += DateTime.Now - activeTask.StartedAtUtc.Value;
                }
                activeTaskValueLabel.Text = $"{activeTask.Title} ({currentTime.ToString(@"hh\:mm\:ss")})";
            }
            else
            {
                activeTaskValueLabel.Text = "Нет активной задачи";
            }
        }

        
        /// <summary>
        /// Работа с выбранным элементом в таблице
        /// </summary>
       
        private void tasksListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tasksListView.SelectedItems.Count == 0)
            {
                ClearTaskDetails();
                _selectedTaskId = null;  // ← Очищаем
                return;
            }
            var item = tasksListView.SelectedItems[0]; // выбирает выделенный элемент

            // ✅ СОХРАНЯЕМ ID ВЫБРАННОЙ ЗАДАЧИ
            _selectedTaskId = (int)item.Tag;

            var task = _userService.GetTaskById(_selectedTaskId.Value);
            if (task != null)
            {
                saveTaskButton.Visible = false;
                updateTaskButton.Visible = true;
                createdLabel.Text = $"Создана {task.Created}";
                titleTextBox.Text = task.Title;
                descriptionTextBox.Text = task.Description;
            }
            if (_selectedTaskId == null)
            {
                saveTaskButton.Visible = true;
                updateTaskButton.Visible = false;
            }
        }

        // ============================================================
        //  =====  РАБОТА КНОПКАМИ =====
        // ============================================================


        /// <summary>
        /// КНОПКА Новая задача
        /// </summary>
        private void newTaskButton_Click(object sender, EventArgs e)
        {
            saveTaskButton.Visible = true;
            updateTaskButton.Visible = false;
            ClearTaskDetails();
        }

        
        //  ==  КНОПКА СОХРАНЕНИЕ НОВОЙ ЗАДАЧИ  ==
        /// <summary>
        /// Кнопка Сохранить (СОХРАНЕНИЕ ЗАДАЧИ В БАЗУ ДАННЫХ)
        /// </summary>
        private void saveTaskButton_Click(object sender, EventArgs e)
        {
            if (_currentUserId == null)
            {
                MessageBox.Show("Сначала войдите в систему!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string title = titleTextBox.Text.Trim();
            string description = descriptionTextBox.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Введите название задачи!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                titleTextBox.Focus();
                return;
            }

            try
            {
                // СОЗДАЁМ ЗАДАЧУ В БАЗЕ ДАННЫХ
                var task = _userService.CreateTask(
                   _currentUserId.Value,
                   title,
                   description
               );

                MessageBox.Show($"Задача '{title}' создана!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Обновляем список задач
                LoadTasks();
                // Очищаем поля
                ClearTaskDetails();
            }
            catch (Exception ex)
            {
                // ✅ ПОКАЗЫВАЕМ ПОЛНУЮ ОШИБКУ
                MessageBox.Show($"Ошибка: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Обновление
        private void updateTaskButton_Click(object sender, EventArgs e)
        {
            if (_selectedTaskId == null)
            {
                saveTaskButton.Visible = true;
                updateTaskButton.Visible = false;
            }
            //  Получаем данные из полей
            string title = titleTextBox.Text.Trim();
            string description = descriptionTextBox.Text.Trim();

            //  Проверяем, что название не пустое
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Введите название задачи!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                titleTextBox.Focus();
                return;
            }

            try
            {
                //  ОБНОВЛЯЕМ ЗАДАЧУ ПО ЕЁ ID (НЕ ПО ID ПОЛЬЗОВАТЕЛЯ!)
                _userService.UpdateTask(_selectedTaskId.Value, title, description);
                //                         ↑ ID задачи, а не пользователя!

                MessageBox.Show($"Задача '{title}' обновлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTasks();
                ClearTaskDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// КНОПКА Удаление Выбранной задачи
        /// </summary>
        private void deleteTaskButton_Click(object sender, EventArgs e)
        {
            // Проверяем, выбран ли пользователь
            if (_currentUserId == null)
            {
                MessageBox.Show("Пользователь не авторизован", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            // 2. Проверяем, выбрана ли задача в списке
            if (_selectedTaskId == null)
            {
                MessageBox.Show("Выберите задачу для удаления!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Запрашиваем подтверждение у пользователя
            DialogResult result = MessageBox.Show(
                "Вы уверены, что хотите удалить выбранную задачу?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // 4. Если пользователь нажал "Нет" — выходим
            if (result == DialogResult.No)
                return;

            try
            {
                // 5. Удаляем задачу из базы данных
                _userService.DeleteTask(_selectedTaskId.Value);

                // 6. Показываем сообщение об успехе
                MessageBox.Show("Задача успешно удалена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 7. Обновляем список задач
                LoadTasks();

                // 8. Очищаем поля деталей
                ClearTaskDetails();
            }
            catch (Exception ex)
            {
                // 9. Если произошла ошибка — показываем её
                MessageBox.Show($"Ошибка при удалении задачи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Кнопка Завершить
        /// </summary>
     
        private void completeTaskButton_Click(object sender, EventArgs e)
        {
            if (_currentUserId == null)
            {
                MessageBox.Show("Сначала войдите в систему!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedTaskId == null)
            {
                MessageBox.Show("Выберите задачу!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _userService.CompleteTask(_selectedTaskId.Value);
                MessageBox.Show("Задача завершена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Работа кнопки Старт
        /// </summary>
      
        private void startTaskButton_Click(object sender, EventArgs e)
        {
            // 1. Проверяем, авторизован ли пользователь
            if (_currentUserId == null)
            {
                MessageBox.Show("Сначала войдите в систему!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Проверяем, выбрана ли задача
            if (_selectedTaskId == null)
            {
                MessageBox.Show("Выберите задачу для запуска!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. Проверяем, не завершена ли задача
            var task = _userService.GetTaskById(_selectedTaskId.Value);
            if (task == null)
            {
                MessageBox.Show("Задача не найдена!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (task.IsCompleted)
            {
                MessageBox.Show("Нельзя запустить завершённую задачу!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. Проверяем, есть ли уже активная задача
            if (_userService.HasActiveTask(_currentUserId.Value))
            {
                DialogResult result = MessageBox.Show(
                    "У вас уже есть активная задача! Остановить её и начать новую?",
                    "Внимание",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.No)
                    return;

                // Останавливаем текущую активную задачу
                var activeTask = _userService.GetActiveTask(_currentUserId.Value);
                if (activeTask != null)
                {
                    _userService.PauseTask(activeTask.Id);
                }
            }

            try
            {
                // 5. ✅ ЗАПУСКАЕМ ЗАДАЧУ
                _userService.StartTask(_selectedTaskId.Value);

                // 6. Показываем сообщение об успехе
                MessageBox.Show("Задача запущена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 7. Обновляем список задач
                LoadTasks();

                // 8. Обновляем информацию об активной задаче
                UpdateActiveTask();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске задачи: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        /// <summary>
        /// Обновление времени каждую секунду
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {

            // Если пользователь не авторизован — выходим
            if (_currentUserId == null) return;
            if (tasksListView.Items.Count == 0) return;

            // ✅ Проходим по ВСЕМ строкам в ListView
            for (int i = 0; i < tasksListView.Items.Count; i++)
            {
                var item = tasksListView.Items[i];

                // Достаём ID задачи из Tag
                int taskId = (int)item.Tag;

                // Получаем задачу по ID из БД (один запрос на задачу)
                var task = _userService.GetTaskById(taskId);
                if (task == null) continue;

                // Вычисляем текущее время
                TimeSpan currentTime = task.TimeSpent;
                if (task.IsRunning && task.StartedAtUtc.HasValue)
                {
                    currentTime += DateTime.Now - task.StartedAtUtc.Value;
                }

                // Обновляем ячейку "Время" (индекс 3)
                if (item.SubItems.Count > 3)
                {
                    item.SubItems[3].Text = currentTime.ToString(@"hh\:mm\:ss");
                }
            }

            // Обновляем активную задачу
            UpdateActiveTask();
        }
        /// <summary>
        /// Кнопка Пауза
        /// </summary>
 
        private void pauseTaskButton_Click(object sender, EventArgs e)
        {
            if (_currentUserId == null)
            {
                MessageBox.Show("Сначала войдите в систему!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_selectedTaskId == null)
            {
                MessageBox.Show("Выберите задачу!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _userService.PauseTask(_selectedTaskId.Value);
                MessageBox.Show("Задача на паузе!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
  
    }
}
