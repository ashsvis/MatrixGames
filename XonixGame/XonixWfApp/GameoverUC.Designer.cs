namespace XonixWfApp
{
    partial class GameoverUC
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
            this.resumeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // resumeButton
            // 
            this.resumeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(170)))), ((int)(((byte)(102)))));
            this.resumeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.resumeButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.resumeButton.FlatAppearance.BorderSize = 3;
            this.resumeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resumeButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resumeButton.Location = new System.Drawing.Point(257, 313);
            this.resumeButton.Name = "resumeButton";
            this.resumeButton.Size = new System.Drawing.Size(148, 43);
            this.resumeButton.TabIndex = 0;
            this.resumeButton.TabStop = false;
            this.resumeButton.Text = "Заново";
            this.resumeButton.UseVisualStyleBackColor = false;
            this.resumeButton.Click += new System.EventHandler(this.resumeButton_Click);
            // 
            // GameoverUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::XonixWfApp.Properties.Resources.GameOverScreen;
            this.Controls.Add(this.resumeButton);
            this.Name = "GameoverUC";
            this.Size = new System.Drawing.Size(639, 510);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button resumeButton;
    }
}
