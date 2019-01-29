namespace ImportShopOffers
{
    partial class NumberPropertyEditor
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudNumber = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMinimum = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMaximum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudMaximum);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudMinimum);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 50);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Значение свойства числа";
            // 
            // nudNumber
            // 
            this.nudNumber.Location = new System.Drawing.Point(57, 18);
            this.nudNumber.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nudNumber.Name = "nudNumber";
            this.nudNumber.Size = new System.Drawing.Size(59, 23);
            this.nudNumber.TabIndex = 3;
            this.nudNumber.ValueChanged += new System.EventHandler(this.nudNumber_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Число:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Минимум:";
            // 
            // nudMinimum
            // 
            this.nudMinimum.Location = new System.Drawing.Point(202, 18);
            this.nudMinimum.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nudMinimum.Name = "nudMinimum";
            this.nudMinimum.Size = new System.Drawing.Size(59, 23);
            this.nudMinimum.TabIndex = 3;
            this.nudMinimum.ValueChanged += new System.EventHandler(this.nudNumber_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(267, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Максимум:";
            // 
            // nudMaximum
            // 
            this.nudMaximum.Location = new System.Drawing.Point(343, 18);
            this.nudMaximum.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.nudMaximum.Name = "nudMaximum";
            this.nudMaximum.Size = new System.Drawing.Size(59, 23);
            this.nudMaximum.TabIndex = 3;
            this.nudMaximum.ValueChanged += new System.EventHandler(this.nudNumber_ValueChanged);
            // 
            // NumberPropertyEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "NumberPropertyEditor";
            this.Size = new System.Drawing.Size(420, 50);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinimum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaximum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMaximum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudMinimum;
        private System.Windows.Forms.Label label2;
    }
}
