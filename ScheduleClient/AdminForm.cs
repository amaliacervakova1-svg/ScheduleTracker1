using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScheduleClient
{
    public partial class AdminForm : Form
    {
        private readonly HttpClient http;
        private List<Group> groups;
        private List<Lesson> lessons;


        // Временные периоды для выпадающего списка
        private readonly string[] timePeriods = AppConfig.TimePeriods;

        // Для хранения подсказок
        private readonly ToolTip toolTip = new ToolTip();
        private bool isDataLoading = false;

        public AdminForm()
        {
            InitializeComponent();

            http = new HttpClient();
            http.Timeout = AppConfig.HttpTimeout;
            groups = new List<Group>();
            lessons = new List<Lesson>();

            this.Text = "Администрирование расписания";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1100, 750);
            this.FormClosing += AdminForm_FormClosing;

            SetupToolTips();
            SetupDataGridView();
            SetupTimeComboBox(); // Заменяем TextBox на ComboBox для времени
            SetupPlaceholderTexts();

            // Привязываем обработчики событий
            BindEventHandlers();
        }

        private void BindEventHandlers()
        {
            btnCreateGroupFromParts.Click += btnCreateGroupFromParts_Click;
            btnDeleteGroup.Click += btnDeleteGroup_Click;
            btnAddLesson.Click += btnAddLesson_Click;
            btnDeleteLesson.Click += btnDeleteLesson_Click;
            btnDeleteAllLessonsForGroup.Click += btnDeleteAllLessonsForGroup_Click;
            cmbDirectionForLesson.SelectedIndexChanged += cmbDirectionForLesson_SelectedIndexChanged;
            cmbGroupForLesson.SelectedIndexChanged += CmbGroupForLesson_SelectedIndexChanged;
            cmbGroupForLesson.SelectedIndexChanged += CmbGroupForLesson_SelectedIndexChanged;
            this.Load += AdminForm_Load;
        }

        private void SetupTimeComboBox()
        {
            // Заполняем ComboBox временными периодами
            cmbTime.Items.AddRange(timePeriods);
            cmbTime.SelectedIndex = 0;
            cmbTime.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetupToolTips()
        {
            // Настройка тултипов
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            // Добавляем подсказки
            toolTip.SetToolTip(txtDirection, "Введите направление (например: ИТ, ИБ, П)");
            toolTip.SetToolTip(txtGroupNum, "Введите номер группы (например: 21, 31, 41)");
            toolTip.SetToolTip(btnCreateGroupFromParts, "Создать группу из введенных направления и номера");
            toolTip.SetToolTip(btnDeleteGroup, "Удалить выбранную группу (если нет занятий)");

            toolTip.SetToolTip(cmbDirectionForLesson, "Фильтр по направлению для выбора группы");
            toolTip.SetToolTip(cmbGroupForLesson, "Выберите группу для добавления занятия");
            toolTip.SetToolTip(cmbAdminDay, "Выберите день недели");
            toolTip.SetToolTip(rbAdminNum, "Числитель - верхняя неделя");
            toolTip.SetToolTip(rbAdminDen, "Знаменатель - нижняя неделя");
            toolTip.SetToolTip(cmbTime, "Выберите время занятия");
            toolTip.SetToolTip(txtSubject, "Название предмета");
            toolTip.SetToolTip(txtTeacher, "ФИО преподавателя");
            toolTip.SetToolTip(txtRoom, "Номер аудитории");
            toolTip.SetToolTip(btnAddLesson, "Добавить занятие в расписание");
            toolTip.SetToolTip(btnDeleteLesson, "Удалить выбранное занятие из таблицы");
            toolTip.SetToolTip(btnDeleteAllLessonsForGroup, "Удалить ВСЕ занятия для выбранной группы");
        }

        private void SetupDataGridView()
        {
            // Настройка внешнего вида DataGridView
            dgvLessons.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLessons.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLessons.MultiSelect = false;
            dgvLessons.ReadOnly = true;
            dgvLessons.AllowUserToAddRows = false;
            dgvLessons.RowHeadersVisible = false;
            dgvLessons.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvLessons.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private void SetupPlaceholderTexts()
        {
            // Эмуляция PlaceholderText для старых версий WinForms
            SetupTextBoxPlaceholder(txtRoom, "Аудитория");
            SetupTextBoxPlaceholder(txtTeacher, "Преподаватель");
            SetupTextBoxPlaceholder(txtSubject, "Предмет*");
            SetupTextBoxPlaceholder(txtDirection, "Напр. (ИТ, ИБ...)");
            SetupTextBoxPlaceholder(txtGroupNum, "Номер (21, 31...)");
        }

        private void SetupTextBoxPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = Color.Gray;

            textBox.Enter += (s, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = "";
                    textBox.ForeColor = SystemColors.WindowText;
                }
            };

            textBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder;
                    textBox.ForeColor = Color.Gray;
                }
            };
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            http.Dispose();
            toolTip.Dispose();
        }

        private async void AdminForm_Load(object sender, EventArgs e)
        {
            // Заполняем предопределенные значения
            InitializePredefinedValues();

            // Показываем индикатор загрузки
            isDataLoading = true;
            SetControlsEnabled(false);

            try
            {
                await LoadData();
            }
            finally
            {
                isDataLoading = false;
                SetControlsEnabled(true);
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            tabControl1.Enabled = enabled;
            foreach (Control control in tabGroups.Controls)
                control.Enabled = enabled;
            foreach (Control control in tabLessons.Controls)
                control.Enabled = enabled;
        }

        private void InitializePredefinedValues()
        {
            // Заполняем дни недели в расписании
            string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
            cmbAdminDay.Items.Clear();
            cmbAdminDay.Items.AddRange(AppConfig.DaysOfWeek);
            cmbAdminDay.SelectedIndex = 0;

            // Выбираем числитель по умолчанию
            rbAdminNum.Checked = true;
        }

        private async Task LoadData()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                statusLabel.Text = "Загрузка данных...";
                Console.WriteLine("Начинаю загрузку данных...");

                // Загружаем группы
                var groupsResponse = await http.GetAsync($"{AppConfig.BaseUrl}/api/groups");
                Console.WriteLine($"Статус загрузки групп: {groupsResponse.StatusCode}");

                if (groupsResponse.IsSuccessStatusCode)
                {
                    var groupsString = await groupsResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ групп: {groupsString.Substring(0, Math.Min(groupsString.Length, 200))}...");

                    groups = await groupsResponse.Content.ReadFromJsonAsync<List<Group>>() ?? new List<Group>();
                    Console.WriteLine($"Загружено {groups.Count} групп");
                }
                else
                {
                    var error = await groupsResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка загрузки групп: {error}");
                    ShowError("Ошибка загрузки групп", error);
                    groups = new List<Group>();
                }

                // Загружаем занятия
                var lessonsResponse = await http.GetAsync($"{AppConfig.BaseUrl}/api/schedule/all");
                Console.WriteLine($"Статус загрузки занятий: {lessonsResponse.StatusCode}");

                if (lessonsResponse.IsSuccessStatusCode)
                {
                    var lessonsString = await lessonsResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ занятий: {lessonsString.Substring(0, Math.Min(lessonsString.Length, 200))}...");

                    try
                    {
                        lessons = await lessonsResponse.Content.ReadFromJsonAsync<List<Lesson>>() ?? new List<Lesson>();
                        Console.WriteLine($"Загружено {lessons.Count} занятий");
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Ошибка десериализации занятий: {ex.Message}");
                        ShowError("Ошибка формата данных", $"Неверный формат JSON занятий: {ex.Message}");
                        lessons = new List<Lesson>();
                    }
                }
                else
                {
                    var error = await lessonsResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка загрузки занятий: {error}");
                    ShowError("Ошибка загрузки расписания", error);
                    lessons = new List<Lesson>();
                }

                // Обновляем UI
                UpdateGroupsList();
                UpdateDirectionsListFromDatabase();
                UpdateGroupSelectionForLessons();
                FilterLessonsGrid();

                statusLabel.Text = $"Загружено: {groups.Count} групп, {lessons.Count} занятий";
                Console.WriteLine("Загрузка данных завершена успешно");
            }
            catch (HttpRequestException ex)
            {
                string errorMsg = $"Ошибка подключения к серверу.\nУбедитесь, что сервер запущен по адресу: {AppConfig.BaseUrl}\n\nДетали: {ex.Message}";
                Console.WriteLine($"HTTP ошибка: {errorMsg}");
                ShowError("Ошибка подключения к серверу", errorMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}\n{ex.StackTrace}");
                ShowError("Ошибка загрузки данных", ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateGroupsList()
        {
            lstGroups.Items.Clear();
            foreach (var g in groups.OrderBy(g => g.Name))
                lstGroups.Items.Add($"{g.Name} (ID: {g.Id})");

            Console.WriteLine($"Обновлен список групп: {groups.Count} записей");
        }

        private void UpdateGroupSelectionForLessons()
        {
            if (isDataLoading) return;

            cmbGroupForLesson.Items.Clear();
            cmbGroupForLesson.Items.Add("-- Выберите группу --");

            string selectedDirection = cmbDirectionForLesson.SelectedItem?.ToString();

            var filteredGroups = groups.AsEnumerable();
            if (selectedDirection != "-- Все --" && !string.IsNullOrEmpty(selectedDirection))
            {
                filteredGroups = filteredGroups.Where(g => g.Name.StartsWith(selectedDirection));
            }

            foreach (var g in filteredGroups.OrderBy(g => g.Name))
                cmbGroupForLesson.Items.Add(g.Name);

            cmbGroupForLesson.SelectedIndex = 0;
            Console.WriteLine($"Обновлен список групп для выбора: {filteredGroups.Count()} групп");

            // Автоматически фильтруем таблицу после обновления списка групп
            FilterLessonsGrid();
        }

        private void RefreshLessonsGrid()
        {
            dgvLessons.Rows.Clear();
            string[] russianDays = { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };

            foreach (var l in lessons.OrderBy(x => x.GroupId).ThenBy(x => x.DayOfWeek).ThenBy(x => x.Time))
            {
                string groupName = groups.FirstOrDefault(g => g.Id == l.GroupId)?.Name ?? "—";

                int dayIndex = l.DayOfWeek;
                string dayName = (dayIndex >= 0 && dayIndex <= 6) ? russianDays[dayIndex] : $"День {dayIndex}";

                string week = l.IsNumerator ? "Числитель" : "Знаменатель";

                dgvLessons.Rows.Add(
                    groupName,     // colGroup
                    dayName,       // colDay  
                    week,          // colWeek
                    l.Time,        // colTime
                    l.Subject,     // colSubject
                    l.Teacher,     // colTeacher
                    l.Room         // colRoom
                );
                // Сохраняем ID в Tag строки (альтернативный способ хранения ID)
                dgvLessons.Rows[dgvLessons.Rows.Count - 1].Tag = l.Id;
            }

            // Обновляем статистику
            UpdateStatistics();
            Console.WriteLine($"Обновлена таблица занятий: {lessons.Count} записей");
        }

        private void UpdateStatistics()
        {
            UpdateFilteredStatistics(lessons.Count); // Показываем все по умолчанию
            lblStats.Text = $"Всего занятий: {lessons.Count} | Групп: {groups.Count}";
        }

        // ========== ВКЛАДКА "ГРУППЫ" ==========

        private async void btnCreateGroupFromParts_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажата кнопка 'Создать группу'");

            // Получаем значения из текстовых полей
            string direction = txtDirection.Text.Trim();
            string number = txtGroupNum.Text.Trim();

            // Проверяем, не являются ли значения плейсхолдерами
            if (direction == "Напр. (ИТ, ИБ...)" || string.IsNullOrWhiteSpace(direction))
            {
                ShowWarning("Введите направление группы");
                txtDirection.Focus();
                return;
            }

            if (number == "Номер (21, 31...)" || string.IsNullOrWhiteSpace(number))
            {
                ShowWarning("Введите номер группы");
                txtGroupNum.Focus();
                return;
            }

            string groupName = $"{direction.ToUpper()}-{number}";
            Console.WriteLine($"Создание группы: {groupName}");

            await CreateGroup(groupName);
        }

        private async Task CreateGroup(string groupName)
        {
            // Проверяем формат
            if (!groupName.Contains('-'))
            {
                ShowWarning("Название группы должно быть в формате: НАПРАВЛЕНИЕ-НОМЕР\nПример: ИТ-21, ИБ-31");
                return;
            }

            // Проверяем, не существует ли уже такая группа
            if (groups.Any(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase)))
            {
                ShowInfo($"Группа '{groupName}' уже существует");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                statusLabel.Text = "Создание группы...";
                Console.WriteLine($"Отправка запроса на создание группы: {groupName}");

                var group = new Group { Name = groupName };
                var response = await http.PostAsJsonAsync($"{AppConfig.BaseUrl}/api/groups", group);

                Console.WriteLine($"Ответ сервера: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    // Обновляем данные
                    await LoadData();

                    ShowSuccess($"Группа '{groupName}' успешно создана");

                    // Автоматически выбираем созданную группу в расписании
                    var createdGroup = await response.Content.ReadFromJsonAsync<Group>();
                    if (response.IsSuccessStatusCode)
                    {
                        // Обновляем данные
                        await LoadData(); // Теперь здесь обновится и список направлений

                        ShowSuccess($"Группа '{groupName}' успешно создана");

                        // Автоматически выбираем созданное направление
                        string directionPart = groupName.Split('-')[0];
                        for (int i = 0; i < cmbDirectionForLesson.Items.Count; i++)
                        {
                            if (cmbDirectionForLesson.Items[i].ToString() == directionPart)
                            {
                                cmbDirectionForLesson.SelectedIndex = i;
                                break;
                            }
                        }

                        // Очищаем поля ввода
                        txtDirection.Text = "Напр. (ИТ, ИБ...)";
                        txtDirection.ForeColor = Color.Gray;
                        txtGroupNum.Text = "Номер (21, 31...)";
                        txtGroupNum.ForeColor = Color.Gray;
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка сервера: {error}");
                    ShowError("Ошибка создания группы", error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение при создании группы: {ex.Message}");
                ShowError("Ошибка подключения", ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                statusLabel.Text = "Готово";
            }
        }

        private async void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажата кнопка 'Удалить группу'");

            if (lstGroups.SelectedItem == null)
            {
                ShowWarning("Выберите группу для удаления");
                return;
            }

            var selectedText = lstGroups.SelectedItem.ToString();
            var groupName = selectedText.Split('(')[0].Trim();

            Console.WriteLine($"Удаление группы: {groupName}");

            // Извлекаем ID из скобок
            int groupId = 0;
            var match = System.Text.RegularExpressions.Regex.Match(selectedText, @"ID: (\d+)");
            if (match.Success)
            {
                groupId = int.Parse(match.Groups[1].Value);
            }
            else
            {
                // Если не нашли ID, ищем по имени
                var group = groups.FirstOrDefault(g => g.Name == groupName);
                if (group == null)
                {
                    ShowError("Ошибка", "Не удалось определить ID группы");
                    return;
                }
                groupId = group.Id;
            }

            var result = MessageBox.Show($"Удалить группу '{groupName}'?\n\nВнимание: удалятся все занятия этой группы!",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    statusLabel.Text = "Удаление группы...";

                    // Сначала удаляем все занятия группы
                    Console.WriteLine($"Удаление занятий группы ID: {groupId}");
                    var deleteLessonsResponse = await http.DeleteAsync($"{AppConfig.BaseUrl}/api/schedule/group/{groupId}");

                    if (!deleteLessonsResponse.IsSuccessStatusCode)
                    {
                        var error = await deleteLessonsResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Ошибка удаления занятий: {error}");
                        ShowError("Ошибка удаления занятий группы", error);
                        return;
                    }

                    // Затем удаляем саму группу
                    Console.WriteLine($"Удаление группы ID: {groupId}");
                    var response = await http.DeleteAsync($"{AppConfig.BaseUrl}/api/groups/{groupId}");

                    if (response.IsSuccessStatusCode)
                    {
                        await LoadData();
                        ShowSuccess($"Группа '{groupName}' успешно удалена");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Ошибка удаления группы: {error}");
                        ShowError("Ошибка удаления группы", error);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Исключение при удалении группы: {ex.Message}");
                    ShowError("Ошибка", ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                    statusLabel.Text = "Готово";
                }
            }
        }

        // ========== ВКЛАДКА "РАСПИСАНИЕ" ==========

        private void cmbDirectionForLesson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDataLoading) return;

            Console.WriteLine($"Изменено направление: {cmbDirectionForLesson.SelectedItem}");

            // Обновляем список групп для выбора
            UpdateGroupSelectionForLessons();

            // Фильтруем таблицу
            FilterLessonsGrid();
        }
        private void CmbGroupForLesson_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isDataLoading) return;

            Console.WriteLine($"Изменена группа: {cmbGroupForLesson.SelectedItem}");

            // Фильтруем таблицу
            FilterLessonsGrid();
        }


        private async void btnAddLesson_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажата кнопка 'Добавить пару'");

            // Валидация полей
            if (!ValidateLessonFields())
                return;

            // Получаем выбранную группу
            string selectedGroupName = cmbGroupForLesson.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedGroupName) || selectedGroupName == "-- Выберите группу --")
            {
                ShowWarning("Сначала выберите группу из списка");
                cmbGroupForLesson.Focus();
                return;
            }

            var group = groups.FirstOrDefault(g => g.Name == selectedGroupName);
            if (group == null)
            {
                ShowError("Группа не найдена", "Выбранная группа не найдена в базе данных. Попробуйте обновить список.");
                return;
            }

            // Преобразование дня недели
            int dayIndex = cmbAdminDay.SelectedIndex; // 0=Понедельник, 6=Воскресенье
            int dayOfWeekValue = (dayIndex == 6) ? 0 : dayIndex + 1; // 0=Воскресенье, 1=Понедельник

            // Создаем DTO для отправки (ВАЖНО!)
            var lessonDto = new CreateLessonDto
            {
                GroupId = group.Id,
                DayOfWeek = dayOfWeekValue,
                IsNumerator = rbAdminNum.Checked,
                Time = cmbTime.SelectedItem?.ToString() ?? cmbTime.Text,
                Subject = txtSubject.Text.Trim(),
                Teacher = txtTeacher.Text.Trim(),
                Room = txtRoom.Text.Trim()
            };

            try
            {
                Cursor = Cursors.WaitCursor;
                statusLabel.Text = "Добавление занятия...";

                // Логируем отправляемые данные для отладки
                Console.WriteLine($"Отправка занятия: Группа={group.Name}(ID:{group.Id}), День={dayOfWeekValue}, Время={lessonDto.Time}");
                Console.WriteLine($"JSON: {System.Text.Json.JsonSerializer.Serialize(lessonDto)}");

                // Отправляем DTO, а не Lesson
                var response = await http.PostAsJsonAsync($"{AppConfig.BaseUrl}/api/schedule", lessonDto);

                Console.WriteLine($"Ответ сервера: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    // Очищаем поля ввода
                    ClearLessonFields();

                    // Обновляем данные
                    await LoadData();

                    ShowSuccess("Занятие успешно добавлено");

                    // Прокручиваем таблицу к новому элементу
                    ScrollToLatestLesson();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка сервера: {errorContent}");
                    ShowError($"Ошибка сервера ({(int)response.StatusCode})", errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP ошибка: {ex.Message}");
                ShowError("Ошибка подключения", $"Не удалось подключиться к серверу.\nАдрес: {AppConfig.BaseUrl}\n\n{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}");
                ShowError("Ошибка", ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                statusLabel.Text = "Готово";
            }
            Console.WriteLine($"Отправка занятия: Группа={group.Name}(ID:{group.Id}), День={dayOfWeekValue}, Время={lessonDto.Time}");
            var json = JsonSerializer.Serialize(lessonDto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            Console.WriteLine($"JSON: {json}");
        }

        private bool ValidateLessonFields()
        {
            bool isValid = true;

            // Проверка предмета
            if (string.IsNullOrWhiteSpace(txtSubject.Text) || txtSubject.Text == "Предмет*")
            {
                errorProvider.SetError(txtSubject, "Введите название предмета");
                isValid = false;
            }
            else if (txtSubject.Text.Length < 2)
            {
                errorProvider.SetError(txtSubject, "Название слишком короткое");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtSubject, "");
            }

            // Проверка преподавателя
            if (string.IsNullOrWhiteSpace(txtTeacher.Text) || txtTeacher.Text == "Преподаватель")
            {
                errorProvider.SetError(txtTeacher, "Введите ФИО преподавателя");
                isValid = false;
            }
            else
            {
                errorProvider.SetError(txtTeacher, "");
            }

            return isValid;
        }

        private void ClearLessonFields()
        {
            cmbTime.SelectedIndex = 0;

            txtSubject.Text = "Предмет*";
            txtSubject.ForeColor = Color.Gray;

            txtTeacher.Text = "Преподаватель";
            txtTeacher.ForeColor = Color.Gray;

            txtRoom.Text = "Аудитория";
            txtRoom.ForeColor = Color.Gray;

            errorProvider.Clear();
        }

        private void ScrollToLatestLesson()
        {
            if (dgvLessons.Rows.Count > 0)
            {
                dgvLessons.FirstDisplayedScrollingRowIndex = dgvLessons.Rows.Count - 1;
                dgvLessons.ClearSelection();
                dgvLessons.Rows[dgvLessons.Rows.Count - 1].Selected = true;
            }
        }

        private async void btnDeleteLesson_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажата кнопка 'Удалить выбранную пару'");

            if (dgvLessons.SelectedRows.Count == 0)
            {
                ShowWarning("Выберите пару для удаления в таблице");
                return;
            }

            DataGridViewRow row = dgvLessons.SelectedRows[0];

            // Получаем ID из Tag строки
            if (row.Tag == null || !(row.Tag is int lessonId))
            {
                ShowError("Ошибка", "Не удалось получить ID пары");
                return;
            }

            Console.WriteLine($"Удаление занятия ID: {lessonId}");
            Console.WriteLine($"URL запроса: {AppConfig.BaseUrl}/api/schedule/{lessonId}");

            if (MessageBox.Show($"Удалить эту пару из расписания?\n\nID: {lessonId}",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    statusLabel.Text = "Удаление занятия...";

                    // Используем основной HttpClient
                    var response = await http.DeleteAsync($"{AppConfig.BaseUrl}/api/schedule/{lessonId}");

                    if (response.IsSuccessStatusCode)
                    {
                        ShowSuccess($"Пара ID {lessonId} успешно удалена!");

                        // Обновляем данные
                        await LoadData();

                        // Прокручиваем к началу
                        if (dgvLessons.Rows.Count > 0)
                        {
                            dgvLessons.FirstDisplayedScrollingRowIndex = 0;
                        }
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Ошибка сервера: {error}");
                        ShowError($"Ошибка удаления ({(int)response.StatusCode})", error);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP ошибка: {ex.Message}");
                    ShowError("Ошибка подключения",
                        $"Не удалось подключиться к серверу.\nАдрес: {AppConfig.BaseUrl}\n\nДетали: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Общая ошибка: {ex.Message}\n{ex.StackTrace}");
                    ShowError("Ошибка удаления", ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                    statusLabel.Text = "Готово";
                }
            }
        }

        private async Task DeleteLesson(int id)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Console.WriteLine($"Удаление занятия ID: {id}");

                var response = await http.DeleteAsync($"{AppConfig.BaseUrl}/api/schedule/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await LoadData();
                    ShowSuccess("Занятие успешно удалено");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка удаления: {error}");
                    ShowError("Ошибка удаления", error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Исключение при удалении: {ex.Message}");
                ShowError("Ошибка", ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnDeleteAllLessonsForGroup_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажата кнопка 'Удалить все пары группы'");

            if (cmbGroupForLesson.SelectedItem == null || cmbGroupForLesson.SelectedIndex == 0)
            {
                ShowWarning("Сначала выберите группу");
                return;
            }

            string selectedGroupName = cmbGroupForLesson.SelectedItem.ToString();
            var group = groups.FirstOrDefault(g => g.Name == selectedGroupName);

            if (group == null)
            {
                ShowError("Группа не найдена", "Выбранная группа не найдена в базе данных");
                return;
            }

            // Подсчет занятий для этой группы
            int lessonCount = lessons.Count(l => l.GroupId == group.Id);

            if (lessonCount == 0)
            {
                ShowInfo($"Для группы '{selectedGroupName}' нет занятий");
                return;
            }

            var result = MessageBox.Show($"Удалить ВСЕ ({lessonCount}) занятия для группы '{selectedGroupName}'?\n\nЭто действие нельзя отменить!",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    statusLabel.Text = $"Удаление {lessonCount} занятий...";
                    Console.WriteLine($"Удаление всех занятий группы: {selectedGroupName}");

                    var response = await http.DeleteAsync($"{AppConfig.BaseUrl}/api/schedule/group/{group.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        await LoadData();
                        ShowSuccess($"Удалено {lessonCount} занятий для группы '{selectedGroupName}'");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Ошибка удаления: {error}");
                        ShowError("Ошибка удаления", error);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Исключение при удалении: {ex.Message}");
                    ShowError("Ошибка при удалении", ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                    statusLabel.Text = "Готово";
                }
            }
        }

        private void FilterLessonsGrid()
        {
            try
            {
                Console.WriteLine("Фильтрация таблицы занятий...");

                // Получаем выбранные фильтры
                string selectedDirection = cmbDirectionForLesson.SelectedItem?.ToString();
                string selectedGroup = cmbGroupForLesson.SelectedItem?.ToString();

                Console.WriteLine($"Фильтры: Направление='{selectedDirection}', Группа='{selectedGroup}'");

                // Фильтруем занятия
                var filteredLessons = lessons.AsEnumerable();

                // Фильтр по направлению
                if (!string.IsNullOrEmpty(selectedDirection) && selectedDirection != "-- Все --")
                {
                    // Находим группы, которые начинаются с выбранного направления
                    var directionGroups = groups
                        .Where(g => g.Name.StartsWith(selectedDirection, StringComparison.OrdinalIgnoreCase))
                        .Select(g => g.Id)
                        .ToList();

                    filteredLessons = filteredLessons.Where(l => directionGroups.Contains(l.GroupId));
                    Console.WriteLine($"Фильтр по направлению '{selectedDirection}': найдено {directionGroups.Count} групп");
                }

                // Фильтр по конкретной группе
                if (!string.IsNullOrEmpty(selectedGroup) && selectedGroup != "-- Выберите группу --")
                {
                    var group = groups.FirstOrDefault(g => g.Name == selectedGroup);
                    if (group != null)
                    {
                        filteredLessons = filteredLessons.Where(l => l.GroupId == group.Id);
                        Console.WriteLine($"Фильтр по группе '{selectedGroup}' (ID: {group.Id})");
                    }
                }

                // Очищаем таблицу
                dgvLessons.Rows.Clear();

                // Заполняем таблицу отфильтрованными данными
                string[] russianDays = { "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };

                foreach (var l in filteredLessons.OrderBy(x => x.GroupId).ThenBy(x => x.DayOfWeek).ThenBy(x => x.Time))
                {
                    string groupName = groups.FirstOrDefault(g => g.Id == l.GroupId)?.Name ?? "—";

                    int dayIndex = (int)l.DayOfWeek;
                    string dayName = (dayIndex >= 0 && dayIndex <= 6) ? russianDays[dayIndex] : $"День {dayIndex}";

                    string week = l.IsNumerator ? "Числитель" : "Знаменатель";

                    dgvLessons.Rows.Add(
                        groupName,     // colGroup
                        dayName,       // colDay  
                        week,          // colWeek
                        l.Time,        // colTime
                        l.Subject,     // colSubject
                        l.Teacher,     // colTeacher
                        l.Room         // colRoom
                    );

                    // Сохраняем ID в Tag строки
                    dgvLessons.Rows[dgvLessons.Rows.Count - 1].Tag = l.Id;
                }

                // Обновляем статистику
                UpdateFilteredStatistics(filteredLessons.Count());
                Console.WriteLine($"Отображено {filteredLessons.Count()} записей после фильтрации");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при фильтрации таблицы: {ex.Message}");
            }
        }

        private void UpdateFilteredStatistics(int filteredCount)
        {
            lblStats.Text = $"Всего занятий: {lessons.Count} | Групп: {groups.Count} | Отфильтровано: {filteredCount}";
        }

        private void UpdateDirectionsListFromDatabase()
        {
            try
            {
                Console.WriteLine("Обновление списка направлений из базы данных...");

                cmbDirectionForLesson.Items.Clear();
                cmbDirectionForLesson.Items.Add("-- Все --");

                if (groups == null || groups.Count == 0)
                {
                    Console.WriteLine("Список групп пуст, направления не могут быть извлечены");
                    return;
                }

                // Извлекаем направления из названий групп (формат: "НАПР-НОМЕР")
                var directions = groups
                    .Select(g => g.Name.Split('-').FirstOrDefault()?.Trim().ToUpper())
                    .Where(dir => !string.IsNullOrEmpty(dir))
                    .Distinct()
                    .OrderBy(dir => dir)
                    .ToList();

                Console.WriteLine($"Найдено направлений в базе: {directions.Count}");

                foreach (var direction in directions)
                {
                    cmbDirectionForLesson.Items.Add(direction);
                    Console.WriteLine($"Добавлено направление: {direction}");
                }

                if (cmbDirectionForLesson.Items.Count > 0)
                    cmbDirectionForLesson.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении списка направлений: {ex.Message}");
            }
        }



        // ========== ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ==========

        private void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Console.WriteLine($"[ERROR] {title}: {message}");
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Кнопка обновления данных
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Нажата кнопка 'Обновить'");
            await LoadData();
        }

        private void AdminForm_Load_1(object sender, EventArgs e)
        {

        }

        private void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}