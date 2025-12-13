using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScheduleClient
{
    public partial class Form1 : Form
    {
        private readonly HttpClient http = new HttpClient();
        private List<Group> allGroups = new List<Group>();
        private bool isLoading = false;
        private Label statusLabel;


        public Form1()
        {
            InitializeComponent();

            // Логирование конфигурации
            AppConfig.LogConfiguration();

            CreateStatusLabel();
            SetupToolTips();
            BindEventHandlers();
        }

        private void CreateStatusLabel()
        {
            statusLabel = new Label
            {
                Text = "Готово",
                Dock = DockStyle.Bottom,
                Height = AppConfig.StatusLabelHeight,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = SystemColors.Control
            };

            this.Controls.Add(statusLabel);
        }

        private void SetupToolTips()
        {
            var toolTip = new ToolTip();
            toolTip.SetToolTip(cmbDirection, "Выберите направление обучения");
            toolTip.SetToolTip(cmbGroup, "Выберите вашу группу");
            toolTip.SetToolTip(cmbDay, "Выберите день недели");
            toolTip.SetToolTip(rbNumerator, "Числитель - верхняя неделя");
            toolTip.SetToolTip(rbDenominator, "Знаменатель - нижняя неделя");
            toolTip.SetToolTip(btnAdmin, "Вход для администратора (требуется пароль)");
        }

        private void BindEventHandlers()
        {
            cmbDirection.SelectedIndexChanged += CmbDirection_SelectedIndexChanged;
            cmbGroup.SelectedIndexChanged += CmbGroup_SelectedIndexChanged;
            cmbDay.SelectedIndexChanged += CmbDay_SelectedIndexChanged;
            rbNumerator.CheckedChanged += RbNumerator_CheckedChanged;
            rbDenominator.CheckedChanged += RbDenominator_CheckedChanged;
            btnAdmin.Click += BtnAdmin_Click;
            this.Load += Form1_Load;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Text = "Расписание занятий - Студенческий портал";

            try
            {
                isLoading = true;
                SetControlsEnabled(false);
                statusLabel.Text = "Загрузка данных...";

                await LoadGroupsAsync();

                // Заполняем дни недели
                string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
                cmbDay.Items.Clear();
                cmbDay.Items.AddRange(AppConfig.DaysOfWeek);

                // Устанавливаем сегодняшний день
                int todayIndex = (int)DateTime.Today.DayOfWeek;
                if (todayIndex == 0) todayIndex = 6; // Воскресенье -> 6
                else todayIndex -= 1; // Понедельник -> 0
                cmbDay.SelectedIndex = todayIndex;

                // Определяем тип недели
                int weekNum = GetWeekNumber(DateTime.Today);
                rbNumerator.Checked = (weekNum % 2 == 1);
                rbDenominator.Checked = !rbNumerator.Checked;

                statusLabel.Text = "Готово";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Ошибка загрузки";
            }
            finally
            {
                isLoading = false;
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            cmbDirection.Enabled = enabled;
            cmbGroup.Enabled = enabled;
            cmbDay.Enabled = enabled;
            rbNumerator.Enabled = enabled;
            rbDenominator.Enabled = enabled;
            btnAdmin.Enabled = enabled;
        }

        private async Task LoadGroupsAsync()
        {
            try
            {
                statusLabel.Text = "Загрузка групп...";
                http.Timeout = TimeSpan.FromSeconds(10);

                var response = await http.GetAsync($"{AppConfig.BaseUrl}/api/groups");

                if (response.IsSuccessStatusCode)
                {
                    allGroups = await response.Content.ReadFromJsonAsync<List<Group>>() ?? new List<Group>();

                    // Получаем уникальные направления
                    var directions = allGroups
                        .Select(g =>
                        {
                            var parts = g.Name.Split('-');
                            return parts.Length > 0 ? parts[0].Trim().ToUpper() : g.Name;
                        })
                        .Distinct()
                        .OrderBy(x => x)
                        .ToList();

                    cmbDirection.Items.Clear();
                    cmbDirection.Items.Add("Выберете направления");
                    foreach (var d in directions)
                        cmbDirection.Items.Add(d);

                    if (cmbDirection.Items.Count > 0)
                        cmbDirection.SelectedIndex = 0;

                    statusLabel.Text = $"Загружено {allGroups.Count} групп";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Ошибка загрузки групп: {error}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Ошибка загрузки групп";
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Не удалось подключиться к серверу.\nАдрес: {AppConfig.BaseUrl}\n\n{ex.Message}",
                    "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Нет подключения к серверу";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки групп: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Ошибка загрузки данных";
            }
        }

        private void UpdateGroupsList()
        {
            if (isLoading) return;

            string selectedDir = cmbDirection.SelectedItem?.ToString() ?? "";

            cmbGroup.Items.Clear();

            if (allGroups == null || allGroups.Count == 0)
            {
                cmbGroup.Items.Add("Группы не загружены");
                cmbGroup.SelectedIndex = 0;
                return;
            }

            var filtered = string.IsNullOrEmpty(selectedDir) || selectedDir == "Все направления"
                ? allGroups
                : allGroups.Where(g => g.Name.StartsWith(selectedDir, StringComparison.OrdinalIgnoreCase));

            foreach (var g in filtered.OrderBy(g => g.Name))
                cmbGroup.Items.Add(g.Name);

            if (cmbGroup.Items.Count > 0)
                cmbGroup.SelectedIndex = 0;
            else
            {
                cmbGroup.Items.Add("Группы не найдены");
                cmbGroup.SelectedIndex = 0;
            }
        }

        private async void CmbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;

            UpdateGroupsList();

            // Если есть выбранная группа, загружаем расписание
            if (cmbGroup.SelectedItem != null)
            {
                await LoadScheduleAsync();
            }
        }

        private async void CmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || cmbGroup.SelectedItem == null ||
                cmbGroup.SelectedItem.ToString() == "Группы не загружены" ||
                cmbGroup.SelectedItem.ToString() == "Группы не найдены")
                return;

            await LoadScheduleAsync();
        }

        private async void CmbDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading || cmbGroup.SelectedItem == null ||
                cmbGroup.SelectedItem.ToString() == "Группы не загружены" ||
                cmbGroup.SelectedItem.ToString() == "Группы не найдены")
                return;

            await LoadScheduleAsync();
        }

        private async void RbNumerator_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading || cmbGroup.SelectedItem == null || !rbNumerator.Checked ||
                cmbGroup.SelectedItem.ToString() == "Группы не загружены" ||
                cmbGroup.SelectedItem.ToString() == "Группы не найдены")
                return;

            await LoadScheduleAsync();
        }

        private async void RbDenominator_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading || cmbGroup.SelectedItem == null || !rbDenominator.Checked ||
                cmbGroup.SelectedItem.ToString() == "Группы не загружены" ||
                cmbGroup.SelectedItem.ToString() == "Группы не найдены")
                return;

            await LoadScheduleAsync();
        }

        private async Task LoadScheduleAsync()
        {
            if (cmbGroup.SelectedItem == null ||
                cmbGroup.SelectedItem.ToString() == "Группы не загружены" ||
                cmbGroup.SelectedItem.ToString() == "Группы не найдены")
            {
                MessageBox.Show("Выберите группу из списка", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbDay.SelectedItem == null)
            {
                MessageBox.Show("Выберите день недели", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string groupName = cmbGroup.SelectedItem.ToString();

            // Преобразование дня недели
            int dayIndex = cmbDay.SelectedIndex;
            int dayOfWeekValue = (dayIndex == 6) ? 0 : dayIndex + 1;

            bool isNumerator = rbNumerator.Checked;

            try
            {
                isLoading = true;
                SetControlsEnabled(false);
                statusLabel.Text = "Загрузка расписания...";
                listSchedule.Items.Clear();
                listSchedule.Items.Add("Загрузка расписания...");
                listSchedule.Refresh();

                string url = $"{AppConfig.BaseUrl}/api/schedule?group={Uri.EscapeDataString(groupName)}&day={dayOfWeekValue}&numerator={isNumerator}";
                Console.WriteLine($"Запрос к: {url}");

                var response = await http.GetAsync(url);
                Console.WriteLine($"Статус ответа: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ сервера: {responseString.Substring(0, Math.Min(responseString.Length, 200))}...");

                    try
                    {
                        var lessons = await response.Content.ReadFromJsonAsync<List<Lesson>>();
                        Console.WriteLine($"Успешно десериализовано {lessons?.Count ?? 0} занятий");
                        DisplaySchedule(groupName, dayIndex, isNumerator, lessons);
                        statusLabel.Text = "Расписание загружено";
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Ошибка десериализации JSON: {ex.Message}");
                        ShowErrorInListBox($"Ошибка формата данных: {ex.Message}\n\nОтвет сервера:\n{responseString}");
                        statusLabel.Text = "Ошибка формата данных";
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка сервера: {error}");
                    ShowErrorInListBox($"Ошибка сервера ({(int)response.StatusCode}): {error}");
                    statusLabel.Text = "Ошибка загрузки";
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP ошибка: {ex.Message}");
                ShowErrorInListBox($"Ошибка подключения: {ex.Message}\n\nПроверьте, запущен ли сервер:\n{AppConfig.BaseUrl}");
                statusLabel.Text = "Нет подключения к серверу";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}\n{ex.StackTrace}");
                ShowErrorInListBox($"Ошибка: {ex.Message}");
                statusLabel.Text = "Ошибка загрузки";
            }
            finally
            {
                isLoading = false;
                SetControlsEnabled(true);
            }
        }

        private void DisplaySchedule(string groupName, int dayIndex, bool isNumerator, List<Lesson> lessons)
        {
            listSchedule.Items.Clear();
            listSchedule.BackColor = SystemColors.Window;

            string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            string dayName = days[dayIndex];
            string weekType = isNumerator ? "ЧИСЛИТЕЛЬ" : "ЗНАМЕНАТЕЛЬ";

            // Заголовок
            listSchedule.Items.Add($" {groupName} — {dayName} ({weekType})");
            listSchedule.Items.Add(new string('═', 70));

            if (lessons == null || lessons.Count == 0)
            {
                listSchedule.Items.Add("");
                listSchedule.Items.Add("     Пар нет — можно отдыхать!");
                listSchedule.Items.Add("");

                // Если сегодня, показываем специальное сообщение
                if (dayIndex == ((int)DateTime.Today.DayOfWeek - 1) ||
                    (DateTime.Today.DayOfWeek == DayOfWeek.Sunday && dayIndex == 6))
                {
                    listSchedule.Items.Add("     Отличный день для учебы или отдыха!");
                }
                return;
            }

            // Счетчик пар
            int pairCount = 1;

            foreach (var l in lessons.OrderBy(x => x.Time))
            {
                // Номер пары с иконкой
                string pairIcon;
                switch (pairCount)
                {
                    case 1:
                        pairIcon = "①";
                        break;
                    case 2:
                        pairIcon = "②";
                        break;
                    case 3:
                        pairIcon = "③";
                        break;
                    case 4:
                        pairIcon = "④";
                        break;
                    case 5:
                        pairIcon = "⑤";
                        break;
                    case 6:
                        pairIcon = "⑥";
                        break;
                    default:
                        pairIcon = "⓿";
                        break;
                }

                listSchedule.Items.Add($" {pairIcon} {l.Time}   {l.Subject}");
                listSchedule.Items.Add($"       👨‍🏫 {l.Teacher}");
                listSchedule.Items.Add($"       🏫 {l.Room}");
                listSchedule.Items.Add("");

                pairCount++;
            }

            // Итог
            listSchedule.Items.Add(new string('─', 70));
            listSchedule.Items.Add($" Всего пар: {lessons.Count}");

            // Подсветка сегодняшнего дня
            if (dayIndex == ((int)DateTime.Today.DayOfWeek - 1) ||
                (DateTime.Today.DayOfWeek == DayOfWeek.Sunday && dayIndex == 6))
            {
                listSchedule.BackColor = Color.LightYellow;
            }
        }

        private void ShowErrorInListBox(string message)
        {
            listSchedule.Items.Clear();
            listSchedule.Items.Add(" Ошибка загрузки расписания");
            listSchedule.Items.Add(new string('═', 70));
            listSchedule.Items.Add("");

            foreach (var line in message.Split('\n'))
            {
                listSchedule.Items.Add($"   {line}");
            }

            listSchedule.Items.Add("");
            listSchedule.Items.Add(" Попробуйте:");
            listSchedule.Items.Add("   1. Проверить подключение к интернету");
            listSchedule.Items.Add("   2. Убедиться, что сервер запущен");
        }

        private int GetWeekNumber(DateTime date)
        {
            // Простой расчет номера недели от 1 сентября
            DateTime start = new DateTime(date.Year, 9, 1);
            if (date < start) start = start.AddYears(-1);
            return (int)((date - start).TotalDays / 7) + 1;
        }

        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            using (var passwordForm = new PasswordForm())
            {
                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    if (passwordForm.PasswordCorrect)
                    {
                        var adminForm = new AdminForm();
                        adminForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль!", "Доступ запрещён",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }

    // Класс формы для ввода пароля (оставляем как было)
    public class PasswordForm : Form
    {
        private TextBox txtPassword;
        private Button btnOk;
        private Button btnCancel;
        public bool PasswordCorrect { get; private set; } = false;
        private const string CorrectPassword = "12345";

        public PasswordForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Аутентификация администратора";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(350, 180);
            this.BackColor = SystemColors.Control;

            var lblTitle = new Label
            {
                Text = "Введите пароль администратора",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(300, 25)
            };

            txtPassword = new TextBox
            {
                Location = new Point(20, 60),
                Size = new Size(290, 25),
                PasswordChar = '*',
                Font = new Font("Segoe UI", 10)
            };

            btnOk = new Button
            {
                Text = "ОК",
                Location = new Point(150, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.OK
            };

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(240, 100),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            btnOk.Click += BtnOk_Click;
            txtPassword.KeyPress += TxtPassword_KeyPress;

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            PasswordCorrect = (txtPassword.Text == CorrectPassword);
        }

        private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnOk_Click(sender, e);
                if (PasswordCorrect)
                    this.DialogResult = DialogResult.OK;
            }
        }
    }
}