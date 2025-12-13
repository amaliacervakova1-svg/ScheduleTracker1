
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;
using ScheduleClient;


namespace ScheduleClient
{
    public partial class Form1 : Form
    {
        private readonly HttpClient http = new HttpClient();
        private List<Group> allGroups = new List<Group>();
<<<<<<< HEAD
        private bool isLoading = false;
        private Label statusLabel;

=======
>>>>>>> master

        public Form1()
        {
            InitializeComponent();
<<<<<<< HEAD

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
=======
>>>>>>> master
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Text = "Расписание занятий";
            await LoadGroupsAsync();

            // Заполняем дни недели по-русски
            string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            cmbDay.Items.AddRange(days);

            // Сегодняшний день
            int todayIndex = (int)DateTime.Today.DayOfWeek - 1;
            if (todayIndex < 0) todayIndex = 6; // воскресенье
            cmbDay.SelectedIndex = todayIndex;

<<<<<<< HEAD
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
=======
            // Числитель / знаменатель по текущей неделе
            int weekNum = GetWeekNumber(DateTime.Today);
            if (weekNum % 2 == 1)
                rbNumerator.Checked = true;
            else
                rbDenominator.Checked = true;
        }

        private async System.Threading.Tasks.Task LoadGroupsAsync()
>>>>>>> master
        {
            try
            {
                allGroups = await http.GetFromJsonAsync<List<Group>>("https://localhost:7233/api/groups")
                           ?? new List<Group>();

<<<<<<< HEAD
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
=======
                var directions = allGroups
                    .Select(g => g.Name.Length >= 2 ? g.Name.Substring(0, 2).ToUpper() : g.Name)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();

                cmbDirection.Items.Clear();
                cmbDirection.Items.Add("Все направления");
                foreach (var d in directions)
                    cmbDirection.Items.Add(d);
                cmbDirection.SelectedIndex = 0;
>>>>>>> master
            }
            catch
            {
<<<<<<< HEAD
                MessageBox.Show($"Не удалось подключиться к серверу.\nАдрес: {AppConfig.BaseUrl}\n\n{ex.Message}",
                    "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                statusLabel.Text = "Нет подключения к серверу";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки групп: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = "Ошибка загрузки данных";
=======
                MessageBox.Show("Не удалось загрузить группы. Запустите сервер (F5).", "Ошибка подключения",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
>>>>>>> master
            }
        }

        private void cmbDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDir = cmbDirection.SelectedItem?.ToString() ?? "";

            cmbGroup.Items.Clear();

            var filtered = string.IsNullOrEmpty(selectedDir) || selectedDir == "Все направления"
                ? allGroups
                : allGroups.Where(g => g.Name.StartsWith(selectedDir));

            foreach (var g in filtered.OrderBy(g => g.Name))
                cmbGroup.Items.Add(g.Name);

            if (cmbGroup.Items.Count > 0) cmbGroup.SelectedIndex = 0;
        }

        private async void btnShow_Click(object sender, EventArgs e)
        {
            if (cmbGroup.SelectedItem == null || cmbDay.SelectedItem == null)
            {
<<<<<<< HEAD
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
=======
                MessageBox.Show("Выберите группу и день недели");
>>>>>>> master
                return;
            }

            string groupName = cmbGroup.SelectedItem.ToString();
            int dayIndex = cmbDay.SelectedIndex;                    // 0 = Понедельник … 6 = Воскресенье
            bool isNumerator = rbNumerator.Checked;

            try
            {
                string url = $"https://localhost:7233/api/schedule?group={Uri.EscapeDataString(groupName)}&day={dayIndex}&numerator={isNumerator.ToString().ToLower()}";

                var lessons = await http.GetFromJsonAsync<List<Lesson>>(url);

                listSchedule.Items.Clear();

<<<<<<< HEAD
                string url = $"{AppConfig.BaseUrl}/api/schedule?group={Uri.EscapeDataString(groupName)}&day={dayOfWeekValue}&numerator={isNumerator}";
                Console.WriteLine($"Запрос к: {url}");
=======
                string weekType = isNumerator ? "ЧИСЛИТЕЛЬ" : "ЗНАМЕНАТЕЛЬ";
                listSchedule.Items.Add($" {groupName} — {cmbDay.SelectedItem} ({weekType})");
                listSchedule.Items.Add(new string('═', 70));
>>>>>>> master

                if (lessons == null || lessons.Count == 0)
                {
                    listSchedule.Items.Add("     Пар нет — можно отдыхать!");
                    return;
                }

                foreach (var l in lessons.OrderBy(x => x.Time))
                {
                    listSchedule.Items.Add($" {l.Time}   {l.Subject}");
                    listSchedule.Items.Add($"            ┗ {l.Teacher}  ·  {l.Room}");
                    listSchedule.Items.Add("");
                }
            }
<<<<<<< HEAD
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP ошибка: {ex.Message}");
                ShowErrorInListBox($"Ошибка подключения: {ex.Message}\n\nПроверьте, запущен ли сервер:\n{AppConfig.BaseUrl}");
                statusLabel.Text = "Нет подключения к серверу";
            }
=======
>>>>>>> master
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка связи с сервером:\n" + ex.Message);
            }
        }

<<<<<<< HEAD
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

=======
        // Простой расчёт номера недели с 1 сентября
>>>>>>> master
        private int GetWeekNumber(DateTime date)
        {
            DateTime start = new DateTime(date.Year, 9, 1);
            if (date < start) start = start.AddYears(-1);
            return (int)((date - start).TotalDays / 7) + 1;
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            string password = "12345"; // ← твой пароль

            using (var form = new Form())
            {
                form.Text = "Требуется пароль";
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent;
                form.MaximizeBox = false;
                form.MinimizeBox = false;
                form.Size = new System.Drawing.Size(300, 150);

                var label = new Label() { Left = 20, Top = 20, Width = 250, Text = "Введите пароль:" };
                var textBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
                textBox.UseSystemPasswordChar = true; // скрывает символы
                var buttonOk = new Button() { Text = "ОК", Left = 170, Width = 80, Top = 85, DialogResult = DialogResult.OK };

                buttonOk.Click += (s, ev) => form.Close();

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(buttonOk);
                form.AcceptButton = buttonOk;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (textBox.Text == password)
                    {
                        new AdminForm().ShowDialog();
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

   
}