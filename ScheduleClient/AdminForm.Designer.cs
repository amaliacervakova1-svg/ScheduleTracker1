namespace ScheduleClient
{
    partial class AdminForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGroups = new System.Windows.Forms.TabPage();
            this.btnDeleteGroup = new System.Windows.Forms.Button();
            this.btnCreateGroupFromParts = new System.Windows.Forms.Button();
            this.txtGroupNum = new System.Windows.Forms.TextBox();
            this.lblGroupNum = new System.Windows.Forms.Label();
            this.txtDirection = new System.Windows.Forms.TextBox();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lstGroups = new System.Windows.Forms.ListBox();
            this.tabLessons = new System.Windows.Forms.TabPage();
            this.lblStats = new System.Windows.Forms.Label();
            this.btnDeleteAllLessonsForGroup = new System.Windows.Forms.Button();
            this.btnDeleteLesson = new System.Windows.Forms.Button();
            this.dgvLessons = new System.Windows.Forms.DataGridView();
            this.btnAddLesson = new System.Windows.Forms.Button();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.txtTeacher = new System.Windows.Forms.TextBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.cmbTime = new System.Windows.Forms.ComboBox();
            this.rbAdminDen = new System.Windows.Forms.RadioButton();
            this.rbAdminNum = new System.Windows.Forms.RadioButton();
            this.cmbAdminDay = new System.Windows.Forms.ComboBox();
            this.cmbGroupForLesson = new System.Windows.Forms.ComboBox();
            this.cmbDirectionForLesson = new System.Windows.Forms.ComboBox();
            this.lblSelectGroup = new System.Windows.Forms.Label();
            this.lblDirectionForLesson = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTeacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.tabGroups.SuspendLayout();
            this.tabLessons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLessons)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabGroups);
            this.tabControl1.Controls.Add(this.tabLessons);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1288, 653);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGroups
            // 
            this.tabGroups.Controls.Add(this.btnDeleteGroup);
            this.tabGroups.Controls.Add(this.lstGroups);
            this.tabGroups.Controls.Add(this.btnAddGroup);
            this.tabGroups.Controls.Add(this.txtGroupName);
            this.tabGroups.Location = new System.Drawing.Point(4, 25);
            this.tabGroups.Name = "tabGroups";
            this.tabGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroups.Size = new System.Drawing.Size(767, 396);
            this.tabGroups.TabIndex = 0;
            this.tabGroups.Text = "Группы";
            this.tabGroups.UseVisualStyleBackColor = true;
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDeleteGroup.Location = new System.Drawing.Point(32, 283);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(135, 50);
            this.btnDeleteGroup.TabIndex = 8;
            this.btnDeleteGroup.Text = "Удалить выбранную группу";
            this.btnDeleteGroup.UseVisualStyleBackColor = false;
            // 
            // btnCreateGroupFromParts
            // 
            this.btnCreateGroupFromParts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCreateGroupFromParts.Location = new System.Drawing.Point(32, 224);
            this.btnCreateGroupFromParts.Name = "btnCreateGroupFromParts";
            this.btnCreateGroupFromParts.Size = new System.Drawing.Size(135, 40);
            this.btnCreateGroupFromParts.TabIndex = 7;
            this.btnCreateGroupFromParts.Text = "Создать группу";
            this.btnCreateGroupFromParts.UseVisualStyleBackColor = false;
            // 
            // txtGroupNum
            // 
            this.txtGroupNum.Location = new System.Drawing.Point(32, 179);
            this.txtGroupNum.Name = "txtGroupNum";
            this.txtGroupNum.Size = new System.Drawing.Size(135, 22);
            this.txtGroupNum.TabIndex = 6;
            // 
            // lblGroupNum
            // 
            this.lblGroupNum.AutoSize = true;
            this.lblGroupNum.Location = new System.Drawing.Point(32, 150);
            this.lblGroupNum.Name = "lblGroupNum";
            this.lblGroupNum.Size = new System.Drawing.Size(100, 16);
            this.lblGroupNum.TabIndex = 5;
            this.lblGroupNum.Text = "Номер группы";
            // 
            // txtDirection
            // 
            this.txtDirection.Location = new System.Drawing.Point(32, 120);
            this.txtDirection.Name = "txtDirection";
            this.txtDirection.Size = new System.Drawing.Size(135, 22);
            this.txtDirection.TabIndex = 4;
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Location = new System.Drawing.Point(32, 100);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(97, 16);
            this.lblDirection.TabIndex = 3;
            this.lblDirection.Text = "Направление";
            // 
            // lstGroups
            // 
            this.lstGroups.FormattingEnabled = true;
            this.lstGroups.ItemHeight = 16;
            this.lstGroups.Location = new System.Drawing.Point(212, 50);
            this.lstGroups.Name = "lstGroups";
            this.lstGroups.Size = new System.Drawing.Size(800, 500);
            this.lstGroups.TabIndex = 0;
            this.lstGroups.SelectedIndexChanged += new System.EventHandler(this.lstGroups_SelectedIndexChanged);
            // 
            // tabLessons
            // 
            this.tabLessons.Controls.Add(this.lblStats);
            this.tabLessons.Controls.Add(this.btnDeleteAllLessonsForGroup);
            this.tabLessons.Controls.Add(this.btnDeleteLesson);
            this.tabLessons.Controls.Add(this.dgvLessons);
            this.tabLessons.Controls.Add(this.btnAddLesson);
            this.tabLessons.Controls.Add(this.txtRoom);
            this.tabLessons.Controls.Add(this.txtTeacher);
            this.tabLessons.Controls.Add(this.txtSubject);
            this.tabLessons.Controls.Add(this.txtTime);
            this.tabLessons.Controls.Add(this.rbAdminDen);
            this.tabLessons.Controls.Add(this.rbAdminNum);
            this.tabLessons.Controls.Add(this.cmbAdminDay);
            this.tabLessons.Controls.Add(this.cmbAdminGroup);
            this.tabLessons.Location = new System.Drawing.Point(4, 25);
            this.tabLessons.Name = "tabLessons";
            this.tabLessons.Padding = new System.Windows.Forms.Padding(3);
            this.tabLessons.Size = new System.Drawing.Size(1280, 624);
            this.tabLessons.TabIndex = 1;
            this.tabLessons.Text = "Расписание";
            this.tabLessons.UseVisualStyleBackColor = true;
            // 
            // txtGroupName
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStats.Location = new System.Drawing.Point(212, 591);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(124, 18);
            this.lblStats.TabIndex = 20;
            this.lblStats.Text = "Всего занятий: 0";
            // 
            // btnDeleteAllLessonsForGroup
            // 
            this.btnDeleteAllLessonsForGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDeleteAllLessonsForGroup.Location = new System.Drawing.Point(39, 427);
            this.btnDeleteAllLessonsForGroup.Name = "btnDeleteAllLessonsForGroup";
            this.btnDeleteAllLessonsForGroup.Size = new System.Drawing.Size(110, 50);
            this.btnDeleteAllLessonsForGroup.TabIndex = 18;
            this.btnDeleteAllLessonsForGroup.Text = "Удалить все пары группы";
            this.btnDeleteAllLessonsForGroup.UseVisualStyleBackColor = false;
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteLesson.Location = new System.Drawing.Point(39, 353);
            this.btnDeleteLesson.Name = "btnDeleteLesson";
            this.btnDeleteLesson.Size = new System.Drawing.Size(110, 58);
            this.btnDeleteLesson.TabIndex = 17;
            this.btnDeleteLesson.Text = "Удалить выбранную пару";
            this.btnDeleteLesson.UseVisualStyleBackColor = true;
            // 
            // cmbAdminGroup
            // 
            this.dgvLessons.AllowUserToAddRows = false;
            this.dgvLessons.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.dgvLessons.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLessons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLessons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroup,
            this.colDay,
            this.colWeek,
            this.colTime,
            this.colSubject,
            this.colTeacher,
            this.colRoom});
            this.dgvLessons.Location = new System.Drawing.Point(215, 71);
            this.dgvLessons.Name = "dgvLessons";
            this.dgvLessons.ReadOnly = true;
            this.dgvLessons.RowHeadersWidth = 51;
            this.dgvLessons.RowTemplate.Height = 24;
            this.dgvLessons.Size = new System.Drawing.Size(1059, 500);
            this.dgvLessons.TabIndex = 16;
            // 
            // cmbAdminDay
            // 
            this.btnAddLesson.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnAddLesson.Location = new System.Drawing.Point(39, 275);
            this.btnAddLesson.Name = "btnAddLesson";
            this.btnAddLesson.Size = new System.Drawing.Size(110, 60);
            this.btnAddLesson.TabIndex = 15;
            this.btnAddLesson.Text = "Добавить пару";
            this.btnAddLesson.UseVisualStyleBackColor = false;
            // 
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(22, 238);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(150, 22);
            this.txtRoom.TabIndex = 14;
            // 
            // txtTeacher
            // 
            this.txtTeacher.Location = new System.Drawing.Point(22, 210);
            this.txtTeacher.Name = "txtTeacher";
            this.txtTeacher.Size = new System.Drawing.Size(150, 22);
            this.txtTeacher.TabIndex = 13;
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(22, 182);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(150, 22);
            this.txtSubject.TabIndex = 12;
            // 
            // cmbTime
            // 
            this.cmbTime.FormattingEnabled = true;
            this.cmbTime.Location = new System.Drawing.Point(22, 152);
            this.cmbTime.Name = "cmbTime";
            this.cmbTime.Size = new System.Drawing.Size(150, 24);
            this.cmbTime.TabIndex = 11;
            // 
            // rbAdminDen
            // 
            this.rbAdminDen.AutoSize = true;
            this.rbAdminDen.Location = new System.Drawing.Point(22, 96);
            this.rbAdminDen.Name = "rbAdminDen";
            this.rbAdminDen.Size = new System.Drawing.Size(116, 20);
            this.rbAdminDen.TabIndex = 10;
            this.rbAdminDen.TabStop = true;
            this.rbAdminDen.Text = "Знаменатель";
            this.rbAdminDen.UseVisualStyleBackColor = true;
            // 
            // rbAdminNum
            // 
            this.rbAdminNum.AutoSize = true;
            this.rbAdminNum.Location = new System.Drawing.Point(319, 10);
            this.rbAdminNum.Name = "rbAdminNum";
            this.rbAdminNum.Size = new System.Drawing.Size(98, 20);
            this.rbAdminNum.TabIndex = 2;
            this.rbAdminNum.TabStop = true;
            this.rbAdminNum.Text = "Числитель";
            this.rbAdminNum.UseVisualStyleBackColor = true;
            // 
            // rbAdminDen
            // 
            this.cmbAdminDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdminDay.FormattingEnabled = true;
            this.cmbAdminDay.Location = new System.Drawing.Point(22, 122);
            this.cmbAdminDay.Name = "cmbAdminDay";
            this.cmbAdminDay.Size = new System.Drawing.Size(150, 24);
            this.cmbAdminDay.TabIndex = 8;
            // 
            // txtTime
            // 
            this.cmbGroupForLesson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGroupForLesson.FormattingEnabled = true;
            this.cmbGroupForLesson.Location = new System.Drawing.Point(215, 40);
            this.cmbGroupForLesson.Name = "cmbGroupForLesson";
            this.cmbGroupForLesson.Size = new System.Drawing.Size(192, 24);
            this.cmbGroupForLesson.TabIndex = 7;
            // 
            // txtSubject
            // 
            this.cmbDirectionForLesson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirectionForLesson.FormattingEnabled = true;
            this.cmbDirectionForLesson.Location = new System.Drawing.Point(22, 40);
            this.cmbDirectionForLesson.Name = "cmbDirectionForLesson";
            this.cmbDirectionForLesson.Size = new System.Drawing.Size(150, 24);
            this.cmbDirectionForLesson.TabIndex = 6;
            // 
            // txtTeacher
            // 
            this.lblSelectGroup.AutoSize = true;
            this.lblSelectGroup.Location = new System.Drawing.Point(212, 20);
            this.lblSelectGroup.Name = "lblSelectGroup";
            this.lblSelectGroup.Size = new System.Drawing.Size(54, 16);
            this.lblSelectGroup.TabIndex = 5;
            this.lblSelectGroup.Text = "Группа";
            // 
            // txtRoom
            // 
            this.lblDirectionForLesson.AutoSize = true;
            this.lblDirectionForLesson.Location = new System.Drawing.Point(22, 20);
            this.lblDirectionForLesson.Name = "lblDirectionForLesson";
            this.lblDirectionForLesson.Size = new System.Drawing.Size(97, 16);
            this.lblDirectionForLesson.TabIndex = 4;
            this.lblDirectionForLesson.Text = "Направление";
            // 
            // btnAddLesson
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 668);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1326, 26);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // dgvLessons
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(57, 20);
            this.statusLabel.Text = "Готово";
            // 
            // btnDeleteLesson
            // 
            this.btnDeleteLesson.Location = new System.Drawing.Point(22, 287);
            this.btnDeleteLesson.Name = "btnDeleteLesson";
            this.btnDeleteLesson.Size = new System.Drawing.Size(110, 64);
            this.btnDeleteLesson.TabIndex = 10;
            this.btnDeleteLesson.Text = "Удалить выбранную пару";
            this.btnDeleteLesson.UseVisualStyleBackColor = true;
            // 
            // colGroup
            // 
            this.colGroup.HeaderText = "Группа";
            this.colGroup.MinimumWidth = 6;
            this.colGroup.Name = "colGroup";
            this.colGroup.ReadOnly = true;
            this.colGroup.Width = 125;
            // 
            // colDay
            // 
            this.colDay.HeaderText = "День недели";
            this.colDay.MinimumWidth = 6;
            this.colDay.Name = "colDay";
            this.colDay.ReadOnly = true;
            this.colDay.Width = 125;
            // 
            // colWeek
            // 
            this.colWeek.HeaderText = "Неделя";
            this.colWeek.MinimumWidth = 6;
            this.colWeek.Name = "colWeek";
            this.colWeek.ReadOnly = true;
            this.colWeek.Width = 125;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "Время";
            this.colTime.MinimumWidth = 6;
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.Width = 125;
            // 
            // colSubject
            // 
            this.colSubject.HeaderText = "Предмет";
            this.colSubject.MinimumWidth = 6;
            this.colSubject.Name = "colSubject";
            this.colSubject.ReadOnly = true;
            this.colSubject.Width = 125;
            // 
            // colTeacher
            // 
            this.colTeacher.HeaderText = "Преподаватель";
            this.colTeacher.MinimumWidth = 6;
            this.colTeacher.Name = "colTeacher";
            this.colTeacher.ReadOnly = true;
            this.colTeacher.Width = 125;
            // 
            // colRoom
            // 
            this.colRoom.HeaderText = "Аудитория";
            this.colRoom.MinimumWidth = 6;
            this.colRoom.Name = "colRoom";
            this.colRoom.ReadOnly = true;
            this.colRoom.Width = 125;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 694);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.Name = "AdminForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Администрирование расписания";
            this.Load += new System.EventHandler(this.AdminForm_Load_1);
            this.tabControl1.ResumeLayout(false);
            this.tabGroups.ResumeLayout(false);
            this.tabGroups.PerformLayout();
            this.tabLessons.ResumeLayout(false);
            this.tabLessons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLessons)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGroups;
        private System.Windows.Forms.TabPage tabLessons;
        private System.Windows.Forms.Button btnDeleteGroup;
        private System.Windows.Forms.ListBox lstGroups;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Button btnDeleteAllLessonsForGroup;
        private System.Windows.Forms.Button btnDeleteLesson;
        private System.Windows.Forms.DataGridView dgvLessons;
        private System.Windows.Forms.Button btnAddLesson;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.TextBox txtTeacher;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.ComboBox cmbTime;
        private System.Windows.Forms.RadioButton rbAdminDen;
        private System.Windows.Forms.RadioButton rbAdminNum;
        private System.Windows.Forms.ComboBox cmbAdminDay;
        private System.Windows.Forms.ComboBox cmbGroupForLesson;
        private System.Windows.Forms.ComboBox cmbDirectionForLesson;
        private System.Windows.Forms.Label lblSelectGroup;
        private System.Windows.Forms.Label lblDirectionForLesson;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRoom;
    }
}
