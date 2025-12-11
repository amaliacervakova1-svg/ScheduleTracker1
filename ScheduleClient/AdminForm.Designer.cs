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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGroups = new System.Windows.Forms.TabPage();
            this.tabLessons = new System.Windows.Forms.TabPage();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.lstGroups = new System.Windows.Forms.ListBox();
            this.btnDeleteGroup = new System.Windows.Forms.Button();
            this.cmbAdminGroup = new System.Windows.Forms.ComboBox();
            this.cmbAdminDay = new System.Windows.Forms.ComboBox();
            this.rbAdminNum = new System.Windows.Forms.RadioButton();
            this.rbAdminDen = new System.Windows.Forms.RadioButton();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtTeacher = new System.Windows.Forms.TextBox();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.btnAddLesson = new System.Windows.Forms.Button();
            this.dgvLessons = new System.Windows.Forms.DataGridView();
            this.btnDeleteLesson = new System.Windows.Forms.Button();
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
            this.tabControl1.Size = new System.Drawing.Size(775, 425);
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
            // tabLessons
            // 
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
            this.tabLessons.Size = new System.Drawing.Size(767, 396);
            this.tabLessons.TabIndex = 1;
            this.tabLessons.Text = "Расписание";
            this.tabLessons.UseVisualStyleBackColor = true;
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(32, 18);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(160, 22);
            this.txtGroupName.TabIndex = 0;
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Location = new System.Drawing.Point(32, 57);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(135, 23);
            this.btnAddGroup.TabIndex = 1;
            this.btnAddGroup.Text = "Добавить группу";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            // 
            // lstGroups
            // 
            this.lstGroups.FormattingEnabled = true;
            this.lstGroups.ItemHeight = 16;
            this.lstGroups.Location = new System.Drawing.Point(212, 18);
            this.lstGroups.Name = "lstGroups";
            this.lstGroups.Size = new System.Drawing.Size(453, 356);
            this.lstGroups.TabIndex = 2;
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Location = new System.Drawing.Point(32, 315);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(135, 59);
            this.btnDeleteGroup.TabIndex = 3;
            this.btnDeleteGroup.Text = "Удалить выбранную группу";
            this.btnDeleteGroup.UseVisualStyleBackColor = true;
            // 
            // cmbAdminGroup
            // 
            this.cmbAdminGroup.FormattingEnabled = true;
            this.cmbAdminGroup.Location = new System.Drawing.Point(22, 7);
            this.cmbAdminGroup.Name = "cmbAdminGroup";
            this.cmbAdminGroup.Size = new System.Drawing.Size(121, 24);
            this.cmbAdminGroup.TabIndex = 0;
            this.cmbAdminGroup.Text = "Группа";
            // 
            // cmbAdminDay
            // 
            this.cmbAdminDay.FormattingEnabled = true;
            this.cmbAdminDay.Items.AddRange(new object[] {
            "Понедельник",
            "Вторник",
            "Среда",
            "Четверг",
            "Пятница",
            "Суббота",
            "Воскресенье"});
            this.cmbAdminDay.Location = new System.Drawing.Point(175, 6);
            this.cmbAdminDay.Name = "cmbAdminDay";
            this.cmbAdminDay.Size = new System.Drawing.Size(121, 24);
            this.cmbAdminDay.TabIndex = 1;
            this.cmbAdminDay.Text = "День недели";
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
            this.rbAdminDen.AutoSize = true;
            this.rbAdminDen.Location = new System.Drawing.Point(433, 11);
            this.rbAdminDen.Name = "rbAdminDen";
            this.rbAdminDen.Size = new System.Drawing.Size(116, 20);
            this.rbAdminDen.TabIndex = 3;
            this.rbAdminDen.TabStop = true;
            this.rbAdminDen.Text = "Знаменатель";
            this.rbAdminDen.UseVisualStyleBackColor = true;
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(577, 11);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(100, 22);
            this.txtTime.TabIndex = 4;
            this.txtTime.Text = "Время";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(22, 61);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(100, 22);
            this.txtSubject.TabIndex = 5;
            this.txtSubject.Text = "Предмет";
            // 
            // txtTeacher
            // 
            this.txtTeacher.Location = new System.Drawing.Point(175, 60);
            this.txtTeacher.Name = "txtTeacher";
            this.txtTeacher.Size = new System.Drawing.Size(153, 22);
            this.txtTeacher.TabIndex = 6;
            this.txtTeacher.Text = "Преподаватель";
            // 
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(358, 60);
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(100, 22);
            this.txtRoom.TabIndex = 7;
            this.txtRoom.Text = "Аудитория";
            // 
            // btnAddLesson
            // 
            this.btnAddLesson.Location = new System.Drawing.Point(22, 118);
            this.btnAddLesson.Name = "btnAddLesson";
            this.btnAddLesson.Size = new System.Drawing.Size(100, 45);
            this.btnAddLesson.TabIndex = 8;
            this.btnAddLesson.Text = "Добавить пару";
            this.btnAddLesson.UseVisualStyleBackColor = true;
            // 
            // dgvLessons
            // 
            this.dgvLessons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLessons.Location = new System.Drawing.Point(175, 108);
            this.dgvLessons.Name = "dgvLessons";
            this.dgvLessons.RowHeadersWidth = 51;
            this.dgvLessons.RowTemplate.Height = 24;
            this.dgvLessons.Size = new System.Drawing.Size(586, 282);
            this.dgvLessons.TabIndex = 9;
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
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
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
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Button btnDeleteGroup;
        private System.Windows.Forms.ListBox lstGroups;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.ComboBox cmbAdminDay;
        private System.Windows.Forms.ComboBox cmbAdminGroup;
        private System.Windows.Forms.TextBox txtTeacher;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.RadioButton rbAdminDen;
        private System.Windows.Forms.RadioButton rbAdminNum;
        private System.Windows.Forms.Button btnDeleteLesson;
        private System.Windows.Forms.DataGridView dgvLessons;
        private System.Windows.Forms.Button btnAddLesson;
        private System.Windows.Forms.TextBox txtRoom;
    }
}