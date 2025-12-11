
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

        public Form1()
        {
            InitializeComponent();
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

            // Числитель / знаменатель по текущей неделе
            int weekNum = GetWeekNumber(DateTime.Today);
            if (weekNum % 2 == 1)
                rbNumerator.Checked = true;
            else
                rbDenominator.Checked = true;
        }

        private async System.Threading.Tasks.Task LoadGroupsAsync()
        {
            try
            {
                allGroups = await http.GetFromJsonAsync<List<Group>>("https://localhost:7233/api/groups")
                           ?? new List<Group>();

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
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить группы. Запустите сервер (F5).", "Ошибка подключения",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Выберите группу и день недели");
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

                string weekType = isNumerator ? "ЧИСЛИТЕЛЬ" : "ЗНАМЕНАТЕЛЬ";
                listSchedule.Items.Add($" {groupName} — {cmbDay.SelectedItem} ({weekType})");
                listSchedule.Items.Add(new string('═', 70));

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
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка связи с сервером:\n" + ex.Message);
            }
        }

        // Простой расчёт номера недели с 1 сентября
        private int GetWeekNumber(DateTime date)
        {
            DateTime start = new DateTime(date.Year, 9, 1);
            if (date < start) start = start.AddYears(-1);
            return (int)((date - start).TotalDays / 7) + 1;
        }
    }

   
}