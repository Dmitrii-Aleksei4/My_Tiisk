using Microsoft.EntityFrameworkCore;
using MyTiskTask.DataBase;
using MyTiskTask.Model;
using MyTiskTask.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyTiskTask
{
    public partial class Form1 : Form
    {
        private readonly DbService _userService;

        public Form1()
        {
            // создаем форму
            InitializeComponent();
            //создаем БД если её нет
            _userService = new DbService();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ищем пользователя
            var user = _userService.FindUserByLogin(login);

            if (user == null)
            {
                // Создаем нового пользователя
                user = _userService.CreateUser(login, password);
                MessageBox.Show($"Пользователь '{login}' создан!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Проверяем пароль
                if (_userService.VerifyPassword(user, password))
                {
                    _userService.UpdateLastLoginDate(user);
                    MessageBox.Show($"Добро пожаловать, {login}!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Неверный пароль!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _userService?.Dispose();  // Освобождаем ресурсы
        }
    }
}
