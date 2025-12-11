
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace ScheduleClient
{
    public partial class AdminForm : Form
    {
        private readonly HttpClient http;
        private List<Group> groups;
        private List<Lesson> lessons;

        public AdminForm()
        {
            InitializeComponent();
            http = new HttpClient();
            groups = new List<Group>();
            lessons = new List<Lesson>();
            InitializeComponent();
            this.Text = "Администрирование расписания";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new System.Drawing.Size(1000, 700);
        }

        private async void AdminForm_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async System.Threading.Tasks.Task LoadData()
        {
            try
            {
                groups = await http.GetFromJsonAsync<List<Group>>("https://localhost:7233/api/groups") ?? new List<Group>();
                lessons = await http.GetFromJsonAsync<List<Lesson>>("https://localhost:7233/api/schedule/all") ?? new List<Lesson>();

                lstGroups.Items.Clear();
                foreach (var g in groups) lstGroups.Items.Add(g.Name);

                cmbAdminGroup.Items.Clear();
                foreach (var g in groups) cmbAdminGroup.Items.Add(g.Name);

                string[] days = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" };
                cmbAdminDay.Items.Clear();
                cmbAdminDay.Items.AddRange(days);

                RefreshLessonsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сервер не запущен или ошибка подключения:\n" + ex.Message);
            }
        }

        private void RefreshLessonsGrid()
        {
            dgvLessons.Rows.Clear();
            foreach (var l in lessons.OrderBy(x => x.GroupId).ThenBy(x => x.DayOfWeek).ThenBy(x => x.Time))
            {
                string groupName = groups.FirstOrDefault(g => g.Id == l.GroupId)?.Name ?? "—";
                string dayName = ((DayOfWeek)l.DayOfWeek).ToString();
                string week = l.IsNumerator ? "Числитель" : "Знаменатель";

                dgvLessons.Rows.Add(groupName, dayName, week, l.Time, l.Subject, l.Teacher, l.Room, l.Id, "Удалить");
            }
        }

        private async void btnAddGroup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGroupName.Text))
            {
                MessageBox.Show("Введите название группы");
                return;
            }

            await http.PostAsJsonAsync("https://localhost:7233/api/groups", new { name = txtGroupName.Text.Trim() });
            txtGroupName.Clear();
            await LoadData();
        }

        private async void btnAddLesson_Click(object sender, EventArgs e)
        {
            if (cmbAdminGroup.SelectedItem == null || cmbAdminDay.SelectedItem == null || string.IsNullOrWhiteSpace(txtTime.Text))
            {
                MessageBox.Show("Заполните все поля: группа, день, время");
                return;
            }

            var group = groups.First(g => g.Name == cmbAdminGroup.SelectedItem.ToString());

            var lesson = new Lesson
            {
                GroupId = group.Id,
                DayOfWeek = cmbAdminDay.SelectedIndex,
                IsNumerator = rbAdminNum.Checked,
                Time = txtTime.Text.Trim(),
                Subject = txtSubject.Text.Trim(),
                Teacher = txtTeacher.Text.Trim(),
                Room = txtRoom.Text.Trim()
            };

            await http.PostAsJsonAsync("https://localhost:7233/api/schedule", lesson);

            txtTime.Clear(); txtSubject.Clear(); txtTeacher.Clear(); txtRoom.Clear();
            await LoadData();
        }

        private async void dgvLessons_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 8 && e.RowIndex >= 0) // колонка "Удалить"
            {
                if (MessageBox.Show("Удалить пару?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int id = (int)dgvLessons.Rows[e.RowIndex].Cells[7].Value;
                    await http.DeleteAsync($"https://localhost:7233/api/schedule/{id}");
                    await LoadData();
                }
            }
        }
    }
}