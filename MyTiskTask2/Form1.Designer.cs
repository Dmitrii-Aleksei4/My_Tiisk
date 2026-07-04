namespace MyTiskTask2
{
    partial class Form1
    {
        

        /// <summary>
        ///  Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private TextBox txtPassword; //пароль
        private Label activeTaskCaptionLabel;
        private Label activeTaskValueLabel;
        private Label userCaptionLabel;
        private ComboBox usersComboBox;
        private Button addUserButton;
        private Label statusFilterLabel;
        private ComboBox statusFilterComboBox;
        private ListView tasksListView;
        private ColumnHeader UserColumnHeader;
        private ColumnHeader titleColumnHeader;
        private ColumnHeader statusColumnHeader;
        private ColumnHeader timeColumnHeader;
        private Label titleLabel;
        private TextBox titleTextBox;
        private Label descriptionLabel;
        private TextBox descriptionTextBox;
        private Label createdLabel;
        private Label createdValueLabel;
        private Button newTaskButton;
        private Button saveTaskButton;
        private Button deleteTaskButton;
        private Button completeTaskButton;
        private Button startTaskButton;
        private Button pauseTaskButton;
        private TableLayoutPanel rootLayout;
        private TableLayoutPanel detailsLayout;
        private FlowLayoutPanel actionsPanel;
        private System.Windows.Forms.Timer refreshTimer;

        /// <summary>
        /// Очистите все используемые ресурсы.
        /// </summary>

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            activeTaskCaptionLabel = new Label();
            txtPassword = new TextBox();
            activeTaskValueLabel = new Label();
            userCaptionLabel = new Label();
            usersComboBox = new ComboBox();
            addUserButton = new Button();
            statusFilterLabel = new Label();
            statusFilterComboBox = new ComboBox();
            tasksListView = new ListView();
            UserColumnHeader = new ColumnHeader();
            titleColumnHeader = new ColumnHeader();
            statusColumnHeader = new ColumnHeader();
            timeColumnHeader = new ColumnHeader();
            titleLabel = new Label();
            titleTextBox = new TextBox();
            descriptionLabel = new Label();
            descriptionTextBox = new TextBox();
            createdLabel = new Label();
            createdValueLabel = new Label();
            newTaskButton = new Button();
            saveTaskButton = new Button();
            deleteTaskButton = new Button();
            completeTaskButton = new Button();
            startTaskButton = new Button();
            pauseTaskButton = new Button();
            rootLayout = new TableLayoutPanel();
            detailsLayout = new TableLayoutPanel();
            actionsPanel = new FlowLayoutPanel();
            refreshTimer = new System.Windows.Forms.Timer(components);
            rootLayout.SuspendLayout();
            detailsLayout.SuspendLayout();
            actionsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // activeTaskCaptionLabel
            // 
            activeTaskCaptionLabel.AutoSize = true;
            activeTaskCaptionLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            activeTaskCaptionLabel.Location = new Point(800, 12);
            activeTaskCaptionLabel.Name = "activeTaskCaptionLabel";
            activeTaskCaptionLabel.Size = new Size(147, 21);
            activeTaskCaptionLabel.TabIndex = 5;
            activeTaskCaptionLabel.Text = "Активная задача:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(290, 11);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // activeTaskValueLabel
            // 
            activeTaskValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            activeTaskValueLabel.AutoEllipsis = true;
            activeTaskValueLabel.Font = new Font("Segoe UI", 12F);
            activeTaskValueLabel.Location = new Point(963, 12);
            activeTaskValueLabel.Name = "activeTaskValueLabel";
            activeTaskValueLabel.Size = new Size(203, 21);
            activeTaskValueLabel.TabIndex = 6;
            activeTaskValueLabel.Text = "Нет активной задачи";
            // 
            // userCaptionLabel
            // 
            userCaptionLabel.AutoSize = true;
            userCaptionLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            userCaptionLabel.Location = new Point(12, 13);
            userCaptionLabel.Name = "userCaptionLabel";
            userCaptionLabel.Size = new Size(111, 19);
            userCaptionLabel.TabIndex = 0;
            userCaptionLabel.Text = "Пользователь:";
            // 
            // usersComboBox
            // 
            usersComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            usersComboBox.FormattingEnabled = true;
            usersComboBox.Location = new Point(126, 11);
            usersComboBox.Name = "usersComboBox";
            usersComboBox.Size = new Size(150, 23);
            usersComboBox.TabIndex = 1;
            usersComboBox.SelectedIndexChanged += usersComboBox_SelectedIndexChanged;
            // 
            // addUserButton
            // 
            addUserButton.Location = new Point(397, 10);
            addUserButton.Name = "addUserButton";
            addUserButton.Size = new Size(136, 25);
            addUserButton.TabIndex = 2;
            addUserButton.Text = "Войти";
            addUserButton.UseVisualStyleBackColor = true;
            addUserButton.Click += addUserButton_Click;
            // 
            // statusFilterLabel
            // 
            statusFilterLabel.AutoSize = true;
            statusFilterLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            statusFilterLabel.Location = new Point(555, 13);
            statusFilterLabel.Name = "statusFilterLabel";
            statusFilterLabel.Size = new Size(65, 19);
            statusFilterLabel.TabIndex = 3;
            statusFilterLabel.Text = "Фильтр:";
            // 
            // statusFilterComboBox
            // 
            statusFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statusFilterComboBox.FormattingEnabled = true;
            statusFilterComboBox.Location = new Point(626, 11);
            statusFilterComboBox.Name = "statusFilterComboBox";
            statusFilterComboBox.Size = new Size(160, 23);
            statusFilterComboBox.TabIndex = 4;
            statusFilterComboBox.SelectedIndexChanged += statusFilterComboBox_SelectedIndexChanged;
            // 
            // tasksListView
            // 
            tasksListView.Columns.AddRange(new ColumnHeader[] { UserColumnHeader, titleColumnHeader, statusColumnHeader, timeColumnHeader });
            tasksListView.Dock = DockStyle.Fill;
            tasksListView.FullRowSelect = true;
            tasksListView.GridLines = true;
            tasksListView.Location = new Point(3, 3);
            tasksListView.MultiSelect = false;
            tasksListView.Name = "tasksListView";
            tasksListView.Size = new Size(547, 564);
            tasksListView.TabIndex = 0;
            tasksListView.UseCompatibleStateImageBehavior = false;
            tasksListView.View = View.Details;
            tasksListView.SelectedIndexChanged += tasksListView_SelectedIndexChanged;
            // 
            // UserColumnHeader
            // 
            UserColumnHeader.Text = "Пользователь";
            UserColumnHeader.Width = 120;
            // 
            // titleColumnHeader
            // 
            titleColumnHeader.Text = "Задача";
            titleColumnHeader.Width = 230;
            // 
            // statusColumnHeader
            // 
            statusColumnHeader.Text = "Статус";
            statusColumnHeader.Width = 120;
            // 
            // timeColumnHeader
            // 
            timeColumnHeader.Text = "Время";
            timeColumnHeader.Width = 120;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(3, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(45, 15);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Задача";
            // 
            // titleTextBox
            // 
            titleTextBox.Dock = DockStyle.Top;
            titleTextBox.Location = new Point(3, 18);
            titleTextBox.Name = "titleTextBox";
            titleTextBox.Size = new Size(589, 23);
            titleTextBox.TabIndex = 1;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new Point(3, 44);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(102, 15);
            descriptionLabel.TabIndex = 2;
            descriptionLabel.Text = "Описание задачи";
            // 
            // descriptionTextBox
            // 
            descriptionTextBox.Dock = DockStyle.Fill;
            descriptionTextBox.Location = new Point(3, 62);
            descriptionTextBox.Multiline = true;
            descriptionTextBox.Name = "descriptionTextBox";
            descriptionTextBox.ScrollBars = ScrollBars.Vertical;
            descriptionTextBox.Size = new Size(589, 430);
            descriptionTextBox.TabIndex = 3;
            // 
            // createdLabel
            // 
            createdLabel.AutoSize = true;
            createdLabel.Location = new Point(3, 495);
            createdLabel.Name = "createdLabel";
            createdLabel.Size = new Size(52, 15);
            createdLabel.TabIndex = 4;
            createdLabel.Text = "Создана";
            // 
            /* createdValueLabel
            /
            createdValueLabel.AutoSize = true;
            createdValueLabel.Location = new Point(3, 510);
            createdValueLabel.Name = "createdValueLabel";
            createdValueLabel.Size = new Size(80, 15);
            createdValueLabel.TabIndex = 5;
            createdValueLabel.Text = "Новая задача";
            */ 
            // newTaskButton
            // 
            newTaskButton.AutoSize = true;
            newTaskButton.Location = new Point(3, 3);
            newTaskButton.Name = "newTaskButton";
            newTaskButton.Size = new Size(90, 27);
            newTaskButton.TabIndex = 0;
            newTaskButton.Text = "Новая задача";
            newTaskButton.UseVisualStyleBackColor = true;
            newTaskButton.Click += newTaskButton_Click;
            // 
            // saveTaskButton
            // 
            saveTaskButton.AutoSize = true;
            saveTaskButton.Location = new Point(99, 3);
            saveTaskButton.Name = "saveTaskButton";
            saveTaskButton.Size = new Size(93, 27);
            saveTaskButton.TabIndex = 1;
            saveTaskButton.Text = "Сохранить";
            saveTaskButton.UseVisualStyleBackColor = true;
            saveTaskButton.Click += saveTaskButton_Click;
            // 
            // deleteTaskButton
            // 
            deleteTaskButton.AutoSize = true;
            deleteTaskButton.Location = new Point(198, 3);
            deleteTaskButton.Name = "deleteTaskButton";
            deleteTaskButton.Size = new Size(81, 27);
            deleteTaskButton.TabIndex = 2;
            deleteTaskButton.Text = "Удалить";
            deleteTaskButton.UseVisualStyleBackColor = true;
            deleteTaskButton.Click += deleteTaskButton_Click;
            // 
            // completeTaskButton
            // 
            completeTaskButton.AutoSize = true;
            completeTaskButton.Location = new Point(285, 3);
            completeTaskButton.Name = "completeTaskButton";
            completeTaskButton.Size = new Size(87, 27);
            completeTaskButton.TabIndex = 3;
            completeTaskButton.Text = "Завершить";
            completeTaskButton.UseVisualStyleBackColor = true;
            completeTaskButton.Click += completeTaskButton_Click;
            // 
            // startTaskButton
            // 
            startTaskButton.AutoSize = true;
            startTaskButton.Location = new Point(378, 3);
            startTaskButton.Name = "startTaskButton";
            startTaskButton.Size = new Size(87, 27);
            startTaskButton.TabIndex = 4;
            startTaskButton.Text = "Старт";
            startTaskButton.UseVisualStyleBackColor = true;
            startTaskButton.Click += startTaskButton_Click;
            // 
            // pauseTaskButton
            // 
            pauseTaskButton.AutoSize = true;
            pauseTaskButton.Location = new Point(471, 3);
            pauseTaskButton.Name = "pauseTaskButton";
            pauseTaskButton.Size = new Size(67, 27);
            pauseTaskButton.TabIndex = 5;
            pauseTaskButton.Text = "Пауза";
            pauseTaskButton.UseVisualStyleBackColor = true;
            pauseTaskButton.Click += pauseTaskButton_Click;
            // 
            // rootLayout
            // 
            rootLayout.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            rootLayout.Controls.Add(tasksListView, 0, 0);
            rootLayout.Controls.Add(detailsLayout, 1, 0);
            rootLayout.Location = new Point(12, 41);
            rootLayout.Name = "rootLayout";
            rootLayout.RowCount = 1;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(1154, 570);
            rootLayout.TabIndex = 5;
            // 
            // detailsLayout
            // 
            detailsLayout.ColumnCount = 1;
            detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            detailsLayout.Controls.Add(titleLabel, 0, 0);
            detailsLayout.Controls.Add(titleTextBox, 0, 1);
            detailsLayout.Controls.Add(descriptionLabel, 0, 2);
            detailsLayout.Controls.Add(descriptionTextBox, 0, 3);
            detailsLayout.Controls.Add(createdLabel, 0, 4);
            detailsLayout.Controls.Add(createdValueLabel, 0, 5);
            detailsLayout.Controls.Add(actionsPanel, 0, 6);
            detailsLayout.Dock = DockStyle.Fill;
            detailsLayout.Location = new Point(556, 3);
            detailsLayout.Name = "detailsLayout";
            detailsLayout.RowCount = 7;
            detailsLayout.RowStyles.Add(new RowStyle());
            detailsLayout.RowStyles.Add(new RowStyle());
            detailsLayout.RowStyles.Add(new RowStyle());
            detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            detailsLayout.RowStyles.Add(new RowStyle());
            detailsLayout.RowStyles.Add(new RowStyle());
            detailsLayout.RowStyles.Add(new RowStyle());
            detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            detailsLayout.Size = new Size(595, 564);
            detailsLayout.TabIndex = 1;
            // 
            // actionsPanel
            // 
            actionsPanel.AutoSize = true;
            actionsPanel.Controls.Add(newTaskButton);
            actionsPanel.Controls.Add(saveTaskButton);
            actionsPanel.Controls.Add(deleteTaskButton);
            actionsPanel.Controls.Add(completeTaskButton);
            actionsPanel.Controls.Add(startTaskButton);
            actionsPanel.Controls.Add(pauseTaskButton);
            actionsPanel.Dock = DockStyle.Fill;
            actionsPanel.Location = new Point(3, 528);
            actionsPanel.Name = "actionsPanel";
            actionsPanel.Size = new Size(589, 33);
            actionsPanel.TabIndex = 6;
            // 
            // refreshTimer
            // 
            refreshTimer.Interval = 1000;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1178, 621);
            Controls.Add(rootLayout);
            Controls.Add(activeTaskValueLabel);
            Controls.Add(activeTaskCaptionLabel);
            Controls.Add(statusFilterComboBox);
            Controls.Add(statusFilterLabel);
            Controls.Add(addUserButton);
            Controls.Add(txtPassword);
            Controls.Add(usersComboBox);
            Controls.Add(userCaptionLabel);
            MinimumSize = new Size(1000, 620);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TiskTask Desktop";
            rootLayout.ResumeLayout(false);
            detailsLayout.ResumeLayout(false);
            detailsLayout.PerformLayout();
            actionsPanel.ResumeLayout(false);
            actionsPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}