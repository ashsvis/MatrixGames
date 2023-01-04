namespace XonixWfApp
{
    partial class WellcomeUC
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
            this.startButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(170)))), ((int)(((byte)(102)))));
            this.startButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.startButton.FlatAppearance.BorderSize = 3;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startButton.Location = new System.Drawing.Point(256, 344);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(148, 43);
            this.startButton.TabIndex = 0;
            this.startButton.TabStop = false;
            this.startButton.Text = "Начать";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // WellcomeUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::XonixWfApp.Properties.Resources.WellcomeScreen;
            this.Controls.Add(this.startButton);
            this.Name = "WellcomeUC";
            this.Size = new System.Drawing.Size(639, 510);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startButton;
    }
}
