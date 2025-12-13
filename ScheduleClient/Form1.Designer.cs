namespace ScheduleClient
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbDirection = new System.Windows.Forms.ComboBox();
            this.cmbGroup = new System.Windows.Forms.ComboBox();
            this.listSchedule = new System.Windows.Forms.ListBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.rbNumerator = new System.Windows.Forms.RadioButton();
            this.rbDenominator = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbDirection
            // 
            this.cmbDirection.FormattingEnabled = true;
            this.cmbDirection.Location = new System.Drawing.Point(12, 12);
            this.cmbDirection.Name = "cmbDirection";
            this.cmbDirection.Size = new System.Drawing.Size(121, 24);
            this.cmbDirection.TabIndex = 0;
            this.cmbDirection.Text = "Направление";
            // 
            // cmbGroup
            // 
            this.cmbGroup.FormattingEnabled = true;
            this.cmbGroup.Location = new System.Drawing.Point(173, 11);
            this.cmbGroup.Name = "cmbGroup";
            this.cmbGroup.Size = new System.Drawing.Size(121, 24);
            this.cmbGroup.TabIndex = 1;
            this.cmbGroup.Text = "Группа";
            // 
            // listSchedule
            // 
            this.listSchedule.FormattingEnabled = true;
            this.listSchedule.ItemHeight = 16;
            this.listSchedule.Location = new System.Drawing.Point(12, 99);
            this.listSchedule.Name = "listSchedule";
            this.listSchedule.Size = new System.Drawing.Size(776, 340);
            this.listSchedule.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(12, 68);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(219, 25);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "Расписание занятий";
            // 
            // cmbDay
            // 
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(320, 10);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(121, 24);
            this.cmbDay.TabIndex = 5;
            this.cmbDay.Text = "День недели";
            // 
            // rbNumerator
            // 
            this.rbNumerator.AutoSize = true;
            this.rbNumerator.Location = new System.Drawing.Point(127, 21);
            this.rbNumerator.Name = "rbNumerator";
            this.rbNumerator.Size = new System.Drawing.Size(98, 20);
            this.rbNumerator.TabIndex = 6;
            this.rbNumerator.TabStop = true;
            this.rbNumerator.Text = "Числитель";
            this.rbNumerator.UseVisualStyleBackColor = true;
            // 
            // rbDenominator
            // 
            this.rbDenominator.AutoSize = true;
            this.rbDenominator.Location = new System.Drawing.Point(0, 21);
            this.rbDenominator.Name = "rbDenominator";
            this.rbDenominator.Size = new System.Drawing.Size(116, 20);
            this.rbDenominator.TabIndex = 7;
            this.rbDenominator.TabStop = true;
            this.rbDenominator.Text = "Знаменатель";
            this.rbDenominator.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbNumerator);
            this.groupBox1.Controls.Add(this.rbDenominator);
            this.groupBox1.Location = new System.Drawing.Point(345, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 53);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Тип недели";
            // 
            // btnAdmin
            // 
            this.btnAdmin.Location = new System.Drawing.Point(618, 392);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(167, 46);
            this.btnAdmin.TabIndex = 9;
            this.btnAdmin.Text = "Администрирование";
            this.btnAdmin.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnAdmin);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbDay);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.listSchedule);
            this.Controls.Add(this.cmbGroup);
            this.Controls.Add(this.cmbDirection);
            this.Name = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDirection;
        private System.Windows.Forms.ComboBox cmbGroup;
        private System.Windows.Forms.ListBox listSchedule;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.ComboBox cmbDay;
        private System.Windows.Forms.RadioButton rbNumerator;
        private System.Windows.Forms.RadioButton rbDenominator;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Button btnRefresh;
    }
}